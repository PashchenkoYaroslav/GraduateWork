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
    function mintBatch (address to, uint256 amount) external;
    function mintBatchToDex (address to, uint256 amount) external;
    function isPoolEnough(uint256 amount) external view returns (bool);
    function getPoolTokens (uint256 amount) external returns (uint256[] memory);
    function buyToken (address to, uint256 amount) external;
    function sellToken (address from, uint256 amount) external;
    function setPool (uint256 amount, bool isExpandPool) external returns (uint256);
    function getPool () external view returns (uint256); 
    function _beforeTokenTransfer(address from, address to, uint256 tokenId, uint256 batchSize)
        external;
    function supportsInterface(bytes4 interfaceId)
        external view  returns (bool);
}

interface ITokenC is IERC20 {
    function setPool (uint256 amount, bool isExpandPool) external returns (uint256);
    function mint(address to, uint256 amount) external;
    function mintToDex(address to, uint256 amount) external;
    function getPool () external view returns (uint256); 
}

contract QuickTokenDEX is IERC721Receiver {

    ITokenA private _TokenA;
    ITokenC private _TokenC;

    uint256 feeValue = 3;
    uint256 feeMultiplier = 100;
    uint8 constant mathDecimals = 18;
    uint256 constant mathMultiplier = 10 ** mathDecimals;

    constructor(address _tokenAAddress, address _tokenCAddress) {
        _TokenA = ITokenA(_tokenAAddress);
        _TokenC = ITokenC(_tokenCAddress);
    }

    function sellCurrency(uint256 YdProvided) public {
       // добавляем множитель для реализации математики вещественных чисел
       YdProvided *= mathMultiplier;
       uint256 tokenALiquidity = _TokenA.getPool();
       uint256 tokenCLiquidity = _TokenC.getPool();

       require(_TokenC.balanceOf(msg.sender) >= YdProvided, "Not enough token C to provide!");

    
       uint256 YafterProvidedFee = YdProvided * (1 * feeMultiplier - feeValue)/feeMultiplier;
       uint256 Y1 = YafterProvidedFee + tokenCLiquidity;
       // Y1/2 section to round up
       
       uint256 X1 = (tokenALiquidity * tokenCLiquidity+Y1/2)/Y1;
       require(X1 < tokenALiquidity, "Not enough token C to change course.");
       uint256 Xd = tokenALiquidity - X1;
    
       uint256 Y1_actual = tokenALiquidity * tokenCLiquidity/X1;
       uint256 Yd = (Y1_actual - tokenCLiquidity) * (1 * feeMultiplier - feeValue)/feeMultiplier;
       uint256 Yfee = (Y1_actual - tokenCLiquidity) * feeValue /feeMultiplier;
     
       
       _TokenC.transferFrom(msg.sender, payable(address(this)), Yfee);
       _TokenC.transferFrom(msg.sender, payable(address(this)), Yd);
       _TokenC.setPool(Yd, true);

       _TokenA.buyToken(msg.sender, Xd);
        
    }

    function sellAsset(uint256 Xd) public {
       uint256 tokenALiquidity = _TokenA.getPool();
       uint256 tokenCLiquidity = _TokenC.getPool();

       require(_TokenA.balanceOf(msg.sender) >= Xd, "Not enough token A to provide!");

       uint256 X1 = Xd + tokenALiquidity;
       uint256 Y1 = tokenALiquidity * tokenCLiquidity/X1;
       uint256 Yd = (tokenCLiquidity - Y1) * (1 * feeMultiplier - feeValue)/feeMultiplier;
       uint256 Yfee = (tokenCLiquidity - Y1) * feeValue/feeMultiplier;

        // Yfee удерживается из суммы TokenC, которую мог бы получить пользователь, так что достаточно
        // вычесть данную величину из пула, трансфер средств при этом не нужен.

       _TokenC.transfer(msg.sender, Yd);
       _TokenC.setPool(Yd + Yfee, false);

       _TokenA.sellToken(msg.sender, Xd);
      
    }
    

    function onERC721Received(address, address, uint256, bytes calldata) external override pure returns (bytes4) {
        return IERC721Receiver.onERC721Received.selector;
    }
}
