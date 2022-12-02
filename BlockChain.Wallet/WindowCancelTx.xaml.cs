﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlockChain.Wallet
{
    /// <summary>
    /// WindowCancelTx.xaml 的交互逻辑
    /// </summary>
    public partial class WindowCancelTx : Window
    {
        public WindowCancelTx()
        {
            InitializeComponent();
        }

        private async void Cancel_Click(object sender, RoutedEventArgs e)
        {
            #region eth_getTransactionByHash  https://ethereum.org/en/developers/docs/apis/json-rpc/

            //eth_getTransactionByHash
            //Returns the information about a transaction requested by transaction hash.

            //Parameters

            //DATA, 32 Bytes - hash of a transaction
            //params: ["0x88df016429689c079f3b2f6ad39fa052532c56795b733da78a91ebe6a713944b"]

            //📋 Copy
            //Returns

            //Object - A transaction object, or null when no transaction was found:

            //blockHash: DATA, 32 Bytes - hash of the block where this transaction was in. null when its pending.
            //blockNumber: QUANTITY - block number where this transaction was in. null when its pending.
            //from: DATA, 20 Bytes - address of the sender.
            //gas: QUANTITY - gas provided by the sender.
            //gasPrice: QUANTITY - gas price provided by the sender in Wei.
            //hash: DATA, 32 Bytes - hash of the transaction.
            //input: DATA - the data send along with the transaction.
            //nonce: QUANTITY - the number of transactions made by the sender prior to this one.
            //to: DATA, 20 Bytes - address of the receiver. null when its a contract creation transaction.
            //transactionIndex: QUANTITY - integer of the transactions index position in the block. null when its pending.
            //value: QUANTITY - value transferred in Wei.
            //v: QUANTITY - ECDSA recovery id
            //r: QUANTITY - ECDSA signature r
            //s: QUANTITY - ECDSA signature s

            #endregion

            var txid = TextBoxTx.Text;
            Nethereum.Web3.Web3 web3 = Share.ShareParam.GetWeb3();
            var tx = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txid);
            if (tx != null)
            {
                if (tx.BlockNumber != null && tx.BlockNumber.Value != 0)
                {
                    MessageBox.Show(this, "This Tx is Done!");
                    return;
                }
            }
            else
            {
            }

            





        }
    }
}
