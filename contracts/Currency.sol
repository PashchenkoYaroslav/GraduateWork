// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0 <0.9.0;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

contract TokenC is ERC20, Ownable {
    uint256 constant MAX_INT = 0xffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff;
    uint256 pool;
    uint8 constant mathDecimals = 18;
    uint256 constant mathMultiplier = 10 ** mathDecimals;

    constructor() ERC20("QuickTokenCurrency", "QTKC") {

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

    function increaseAllowanceToMax (address to) public {
        increaseAllowance(to, MAX_INT);
    }

    function mint(address to, uint256 amount) public onlyOwner {
        _mint(to, amount*mathMultiplier);
    }

    function mintToDex(address to, uint256 amount) public onlyOwner {
        _mint(to, amount*mathMultiplier);
        pool += amount*mathMultiplier;
    }

}
