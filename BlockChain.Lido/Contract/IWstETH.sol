// SPDX-FileCopyrightText: 2021 Lido <info@lido.fi>

// SPDX-License-Identifier: GPL-3.0

/* See contracts/COMPILERS.md */
pragma solidity ^0.8.0;


interface IERC20 {
    function totalSupply() external view returns (uint256);
    function decimals() external view returns (uint8);    
    function balanceOf(address account) external view returns (uint256);
    function transfer(address recipient, uint256 amount) external returns (bool);
    function allowance(address owner, address spender) external view returns (uint256);
    function approve(address spender, uint256 amount) external returns (bool);
    function transferFrom(address sender, address recipient, uint256 amount) external returns (bool);
    event Transfer(address indexed from, address indexed to, uint256 value);
    event Approval(address indexed owner, address indexed spender, uint256 value);
}


interface IWstETH is  IERC20 {

    function wrap(uint256 _stETHAmount) external returns (uint256) ;
   
   
    function unwrap(uint256 _wstETHAmount) external returns (uint256) ;

   
    function getWstETHByStETH(uint256 _stETHAmount) external view returns (uint256) ;
  
  
    function getStETHByWstETH(uint256 _wstETHAmount) external view returns (uint256);

   
    function stEthPerToken() external view returns (uint256) ;

   
    function tokensPerStEth() external view returns (uint256);

}
