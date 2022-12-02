using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Share.BLL
{
    public static class UserTokenApprove
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        public static void SaveApproveInfo(string _UserAddress, string _token, string _spender, Share.Model.Token TokenModel, double ApproveAmount, string tx)
        {
            var ChainId = (int)ShareParam.GetChainId();
            Model.UserTokenApprove model = DAL.UserTokenApprove.GetModel(Share.ShareParam.DbConStr, ChainId, _UserAddress, _token, _spender);

            if (model == null)
            {
                model = new Model.UserTokenApprove();
                model.ChainId = (int)ShareParam.GetChainId();
                model.UserAddress = _UserAddress;
                model.SpenderAddress = _spender;
                model.TokenAddress = _token;
                model.CurrentAmount = ApproveAmount;
                model.TransactionHash = tx;
                model.TokenSymbol = TokenModel.Symbol;
                model.TokenDecimals = TokenModel.Decimals;
                model.IsDivToken = false;
                model.DivTokenIsWithdrawable = false;
            }
            else
            {
                model.CurrentAmount = ApproveAmount;
            }

            BlockChain.Share.BLL.UserTokenApprove.Save(model);
        }

        //public static void SaveDivApproveInfo1(string _UserAddress, string _token, string _spender, Share.Model.Token TokenModel, double ApproveAmount, string tx)
        //{
        //    var ChainId = (int)ShareParam.GetChainId();
        //    Model.UserTokenApprove model = DAL.UserTokenApprove.GetModel(Share.ShareParam.DbConStr, ChainId, _UserAddress, _token, _spender);

        //    if (model == null)
        //    {
        //        model = new Model.UserTokenApprove();
        //        model.ChainId = (int)ShareParam.GetChainId();
        //        model.UserAddress = _UserAddress;
        //        model.SpenderAddress = _spender;
        //        model.TokenAddress = _token;
        //        model.CurrentAmount = ApproveAmount;
        //        model.TransactionHash = tx;
        //        model.TokenSymbol = TokenModel.Symbol;
        //        model.TokenDecimals = TokenModel.Decimals;
        //        model.IsDivToken = false;
        //        model.DivTokenIsWithdrawable = false;
        //    }
        //    else
        //    {
        //        model.CurrentAmount = ApproveAmount;
        //    }

        //    BlockChain.Share.BLL.UserTokenApprove.Save(model);
        //}

        public static void SaveDivApproveInfo(string _UserAddress, string _token, string _spender, Share.Model.Token TokenModel, double ApproveAmount, bool IsWithdrawable, string tx)
        {
            var ChainId = (int)ShareParam.GetChainId();
            Model.UserTokenApprove model = DAL.UserTokenApprove.GetModel(Share.ShareParam.DbConStr, ChainId, _UserAddress, _token, _spender);

            if (model == null)
            {
                model = new Model.UserTokenApprove();
                model.ChainId = (int)ShareParam.GetChainId();
                model.UserAddress = _UserAddress;
                model.SpenderAddress = _spender;
                model.TokenAddress = _token;
                model.CurrentAmount = ApproveAmount;
                model.TransactionHash = tx;
                model.TokenSymbol = TokenModel.Symbol;
                model.TokenDecimals = TokenModel.Decimals;
                model.IsDivToken = true;
                model.DivTokenIsWithdrawable = IsWithdrawable;
            }
            else
            {
                if(ApproveAmount > 0)
                {
                    model.CurrentAmount = ApproveAmount;
                }
                model.IsDivToken = true;
                model.DivTokenIsWithdrawable = IsWithdrawable;
            }

            BlockChain.Share.BLL.UserTokenApprove.Save(model);
        }

        public static bool Save(Model.UserTokenApprove model)
        {
            try
            {
                if (DAL.UserTokenApprove.Exist(Share.ShareParam.DbConStr, model.ChainId, model.UserAddress, model.TokenAddress, model.SpenderAddress))
                {
                    DAL.UserTokenApprove.Update(Share.ShareParam.DbConStr, model);
                }
                else
                {
                    DAL.UserTokenApprove.Insert(Share.ShareParam.DbConStr, model);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return false;
            }

        }


        public static List<Model.UserTokenApprove> GetAllUserTokenApprove(bool withZeroAmount = true) {
            string sql = @"select * from UserTokenApprove Where ChainId = @ChainId";
            string sql1 = @"select * from UserTokenApprove Where ChainId = @ChainId and CurrentAmount > 0 ";
            if (!withZeroAmount) {
                sql = sql1;
            }

            SqlConnection cn = new SqlConnection(ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@ChainId", SqlDbType.Int, 4).Value = (int)ShareParam.GetChainId();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);

            return Model.UserTokenApprove.DataTable2List( ds.Tables[0]);
        }


        public static bool WriteCancleApprove(string TransactionHash) {

            int ChainId = (int)Share.ShareParam.GetChainId();
            string sql = @"
UPDATE  UserTokenApprove
SET         CurrentAmount = 0
WHERE   (ChainId = @ChainId) AND (TransactionHash = @TransactionHash)
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cm.Parameters.Add("@ChainId", SqlDbType.Int).Value = ChainId;
            cm.Parameters.Add("@TransactionHash", SqlDbType.NVarChar).Value = TransactionHash;
            

            int RecordAffected = -1;
            cn.Open();
            try
            {
                RecordAffected = cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }
            return RecordAffected > 0;

        }


    }

}
