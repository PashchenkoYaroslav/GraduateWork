// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Burnable.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Enumerable.sol";
import "@openzeppelin/contracts/utils/Counters.sol";

contract TokenA is ERC721, Ownable, ERC721Burnable, ERC721Enumerable {
    using Counters for Counters.Counter;
    Counters.Counter private _tokenIds;
    
    uint256[] public poolIds;
    uint256 pool;
    
    constructor() ERC721("QuickTokenAsset", "QTKA") {
    }

    function mintBatch (address to, uint256 amount) public onlyOwner {
        for (uint256 i=0; i < amount; i++) {
            uint256 newTokenId = _tokenIds.current();
            _mint(to, newTokenId);
            _tokenIds.increment();
        }
    }

    function mintBatchToDex (address to, uint256 amount) public onlyOwner {
        for (uint256 i=0; i < amount; i++) {
            uint256 newTokenId = _tokenIds.current();
            _mint(to, newTokenId);
            poolIds.push(newTokenId);
            pool++;
            _tokenIds.increment();
        }
    }

    function getPool () public view returns (uint256) {
        return pool;
    }

    function setPool (uint256 amount, bool isExpandPool) public returns (uint256) {
        if (isExpandPool) {
            pool += amount;
        } else {
            pool -= amount;
        }
        return pool;
    }

    function getPoolTokens (uint256 amount) internal returns (uint256[] memory) {
        uint256[] memory idArr = new uint256[] (amount);
        for (uint256 i=0; i < amount; i++) {
            uint256 dd = pool - 1;
            idArr[i] = poolIds[dd];
            poolIds.pop();
            pool--;
        }
        return idArr;
    }

    function buyToken (address from, address to, uint256 amount) public  {
        uint256[] memory idArr = getPoolTokens(amount);
        for (uint256 i=0; i < amount; i++) {
           safeTransferFrom(from, to, idArr[i]);
        }
    }

    function sellToken (address from, uint256 amount) public {
        // to pool
        uint256[] memory idArr = new uint256[] (amount);
        for (uint256 i=0; i < amount; i++) {
            idArr[i] = tokenOfOwnerByIndex(from, i);
        }
        
        for (uint256 i=0; i < amount; i++) {
            safeTransferFrom(from, msg.sender, idArr[i]);
            poolIds.push(idArr[i]);
            pool++;
        }
        return;
    }

    function _beforeTokenTransfer(address from, address to, uint256 tokenId, uint256 batchSize)
        internal
        override(ERC721, ERC721Enumerable)
    {
        super._beforeTokenTransfer(from, to, tokenId, batchSize);
    }

    function supportsInterface(bytes4 interfaceId)
        public
        view
        override(ERC721, ERC721Enumerable)
        returns (bool)
    {
        return super.supportsInterface(interfaceId);
    }
}
