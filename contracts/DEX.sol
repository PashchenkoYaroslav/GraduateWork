// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0 <0.9.0;

import "@openzeppelin/contracts/token/ERC721/IERC721Receiver.sol";
import "@openzeppelin/contracts/token/ERC721/IERC721.sol";
import "@openzeppelin/contracts/token/ERC20/IERC20.sol";
import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Burnable.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Enumerable.sol"; 
import "@openzeppelin/contracts/utils/Counters.sol";

interface ITokenA is IERC721  {
    function mintBatch (uint256 amount, address to) external;
    function mintBatchToDex (uint256 amount, address to) external;
    function isPoolEnough(uint256 amount) external view returns (bool);
    function getPoolTokens (uint256 amount) external returns (uint256[] memory);
    function buyToken (address from, address to, uint256 amount) external;
    function sellToken (address to, uint256 amount) external;
    function setPool (uint256 amount, bool isExpandPool) external returns (uint256);
    function getPool () external view returns (uint256); 
    function _beforeTokenTransfer(address from, address to, uint256 tokenId, uint256 batchSize)
        external;
    function supportsInterface(bytes4 interfaceId)
        external view  returns (bool);
}

interface ITokenB is IERC20 {
    function setPool (uint256 amount, bool isExpandPool) external returns (uint256);
    function mint(address to, uint256 amount) external;
    function mintToDex(address to, uint256 amount) external;
    function getPool () external view returns (uint256); 
}

contract QuickTokenDEX is IERC721Receiver {

    ITokenA private _TokenA;
    ITokenB private _TokenB;

    uint256 feeValue = 3;
    uint256 feeMultiplier = 100;
    uint8 constant mathDecimals = 18;
    uint256 constant mathMultiplier = 10 ** mathDecimals;

    constructor(address _tokenAAddress, address _tokenBAddress) {
        _TokenA = ITokenA(_tokenAAddress);
        _TokenB = ITokenB(_tokenBAddress);
    }

    function swapToTokenA(uint256 YdProvided) public {
       // добавляем множитель для реализации математики вещественных чисел
       YdProvided *= mathMultiplier;
       uint256 tokenALiquidity = _TokenA.getPool();
       uint256 tokenBLiquidity = _TokenB.getPool();

       require(_TokenB.balanceOf(msg.sender) >= YdProvided, "Not enough token B to provide!");

    
       uint256 YafterProvidedFee = YdProvided * (1 * feeMultiplier - feeValue)/feeMultiplier;
       uint256 Y1 = YafterProvidedFee + tokenBLiquidity;
       // Y1/2 section to round up
       
       uint256 X1 = (tokenALiquidity * tokenBLiquidity+Y1/2)/Y1;
       require(X1 < tokenALiquidity, "Not enough token B to change course.");
       uint256 Xd = tokenALiquidity - X1;
    
       uint256 Y1_actual = tokenALiquidity * tokenBLiquidity/X1;
       uint256 Yd = (Y1_actual - tokenBLiquidity) * (1 * feeMultiplier - feeValue)/feeMultiplier;
       uint256 Yfee = (Y1_actual - tokenBLiquidity) * feeValue /feeMultiplier;
     
       
       _TokenB.transfer(payable(address(this)), Yfee);
       _TokenB.transfer(payable(address(this)), Yd);
       _TokenB.setPool(Yd, true);

       _TokenA.buyToken(payable(address(this)), msg.sender, Xd);
        
    }

    function swapToTokenB(uint256 Xd) public {
       uint256 tokenALiquidity = _TokenA.getPool();
       uint256 tokenBLiquidity = _TokenB.getPool();

       require(_TokenA.balanceOf(msg.sender) >= Xd, "Not enough token A to provide!");

       uint256 X1 = Xd + tokenALiquidity;
       uint256 Y1 = tokenALiquidity * tokenBLiquidity/X1;
       uint256 Yd = (tokenBLiquidity - Y1) * (1 * feeMultiplier - feeValue)/feeMultiplier;
       uint256 Yfee = (tokenBLiquidity - Y1) * feeValue/feeMultiplier;

       
       _TokenB.transfer(payable(address(this)), Yfee);
       _TokenB.transfer(msg.sender, Yd);
       _TokenB.setPool(Yd + Yfee, false);

       _TokenA.sellToken(payable(address(this)), Xd);
      
    }
    

    function onERC721Received(address, address, uint256, bytes calldata) external override pure returns (bytes4) {
        return IERC721Receiver.onERC721Received.selector;
    }
}
