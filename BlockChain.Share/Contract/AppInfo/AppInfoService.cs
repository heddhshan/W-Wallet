using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using BlockChain.Share.Contract.AppInfo.ContractDefinition;

namespace BlockChain.Share.Contract.AppInfo
{
    public partial class AppInfoService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, AppInfoDeployment appInfoDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<AppInfoDeployment>().SendRequestAndWaitForReceiptAsync(appInfoDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, AppInfoDeployment appInfoDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<AppInfoDeployment>().SendRequestAsync(appInfoDeployment);
        }

        public static async Task<AppInfoService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, AppInfoDeployment appInfoDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, appInfoDeployment, cancellationTokenSource);
            return new AppInfoService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public AppInfoService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AdminQueryAsync(AdminFunction adminFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AdminFunction, string>(adminFunction, blockParameter);
        }

        
        public Task<string> AdminQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AdminFunction, string>(null, blockParameter);
        }

        public Task<string> ContactInfoQueryAsync(ContactInfoFunction contactInfoFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ContactInfoFunction, string>(contactInfoFunction, blockParameter);
        }

        
        public Task<string> ContactInfoQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ContactInfoFunction, string>(null, blockParameter);
        }

        public Task<CurAppDownloadOfOutputDTO> CurAppDownloadOfQueryAsync(CurAppDownloadOfFunction curAppDownloadOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<CurAppDownloadOfFunction, CurAppDownloadOfOutputDTO>(curAppDownloadOfFunction, blockParameter);
        }

        public Task<CurAppDownloadOfOutputDTO> CurAppDownloadOfQueryAsync(BigInteger returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var curAppDownloadOfFunction = new CurAppDownloadOfFunction();
                curAppDownloadOfFunction.ReturnValue1 = returnValue1;
                curAppDownloadOfFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryDeserializingToObjectAsync<CurAppDownloadOfFunction, CurAppDownloadOfOutputDTO>(curAppDownloadOfFunction, blockParameter);
        }

        public Task<CurAppVersionOfOutputDTO> CurAppVersionOfQueryAsync(CurAppVersionOfFunction curAppVersionOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<CurAppVersionOfFunction, CurAppVersionOfOutputDTO>(curAppVersionOfFunction, blockParameter);
        }

        public Task<CurAppVersionOfOutputDTO> CurAppVersionOfQueryAsync(BigInteger returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var curAppVersionOfFunction = new CurAppVersionOfFunction();
                curAppVersionOfFunction.ReturnValue1 = returnValue1;
                curAppVersionOfFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryDeserializingToObjectAsync<CurAppVersionOfFunction, CurAppVersionOfOutputDTO>(curAppVersionOfFunction, blockParameter);
        }

        public Task<BigInteger> CurEventIdQueryAsync(CurEventIdFunction curEventIdFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CurEventIdFunction, BigInteger>(curEventIdFunction, blockParameter);
        }

        
        public Task<BigInteger> CurEventIdQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CurEventIdFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> DonationQueryAsync(DonationFunction donationFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DonationFunction, string>(donationFunction, blockParameter);
        }

        
        public Task<string> DonationQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DonationFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> FromBlockQueryAsync(FromBlockFunction fromBlockFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FromBlockFunction, BigInteger>(fromBlockFunction, blockParameter);
        }

        
        public Task<BigInteger> FromBlockQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FromBlockFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> KeyAddressQueryAsync(KeyAddressFunction keyAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<KeyAddressFunction, string>(keyAddressFunction, blockParameter);
        }

        
        public Task<string> KeyAddressQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var keyAddressFunction = new KeyAddressFunction();
                keyAddressFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<KeyAddressFunction, string>(keyAddressFunction, blockParameter);
        }

        public Task<string> KeyStringQueryAsync(KeyStringFunction keyStringFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<KeyStringFunction, string>(keyStringFunction, blockParameter);
        }

        
        public Task<string> KeyStringQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var keyStringFunction = new KeyStringFunction();
                keyStringFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<KeyStringFunction, string>(keyStringFunction, blockParameter);
        }

        public Task<string> PublishNoticeRequestAsync(PublishNoticeFunction publishNoticeFunction)
        {
             return ContractHandler.SendRequestAsync(publishNoticeFunction);
        }

        public Task<TransactionReceipt> PublishNoticeRequestAndWaitForReceiptAsync(PublishNoticeFunction publishNoticeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(publishNoticeFunction, cancellationToken);
        }

        public Task<string> PublishNoticeRequestAsync(BigInteger appId, string subject, string body)
        {
            var publishNoticeFunction = new PublishNoticeFunction();
                publishNoticeFunction.AppId = appId;
                publishNoticeFunction.Subject = subject;
                publishNoticeFunction.Body = body;
            
             return ContractHandler.SendRequestAsync(publishNoticeFunction);
        }

        public Task<TransactionReceipt> PublishNoticeRequestAndWaitForReceiptAsync(BigInteger appId, string subject, string body, CancellationTokenSource cancellationToken = null)
        {
            var publishNoticeFunction = new PublishNoticeFunction();
                publishNoticeFunction.AppId = appId;
                publishNoticeFunction.Subject = subject;
                publishNoticeFunction.Body = body;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(publishNoticeFunction, cancellationToken);
        }

        public Task<string> SuperAdminQueryAsync(SuperAdminFunction superAdminFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SuperAdminFunction, string>(superAdminFunction, blockParameter);
        }

        
        public Task<string> SuperAdminQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SuperAdminFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> GetCurrentBlockTimestampQueryAsync(GetCurrentBlockTimestampFunction getCurrentBlockTimestampFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockTimestampFunction, BigInteger>(getCurrentBlockTimestampFunction, blockParameter);
        }

        
        public Task<BigInteger> GetCurrentBlockTimestampQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCurrentBlockTimestampFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetKeyQueryAsync(GetKeyFunction getKeyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetKeyFunction, BigInteger>(getKeyFunction, blockParameter);
        }

        
        public Task<BigInteger> GetKeyQueryAsync(string strkey, BlockParameter blockParameter = null)
        {
            var getKeyFunction = new GetKeyFunction();
                getKeyFunction.Strkey = strkey;
            
            return ContractHandler.QueryAsync<GetKeyFunction, BigInteger>(getKeyFunction, blockParameter);
        }

        public Task<string> GetKeyAddressQueryAsync(GetKeyAddressFunction getKeyAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetKeyAddressFunction, string>(getKeyAddressFunction, blockParameter);
        }

        
        public Task<string> GetKeyAddressQueryAsync(BigInteger key, BlockParameter blockParameter = null)
        {
            var getKeyAddressFunction = new GetKeyAddressFunction();
                getKeyAddressFunction.Key = key;
            
            return ContractHandler.QueryAsync<GetKeyAddressFunction, string>(getKeyAddressFunction, blockParameter);
        }

        public Task<string> GetKeyAddress1QueryAsync(GetKeyAddress1Function getKeyAddress1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetKeyAddress1Function, string>(getKeyAddress1Function, blockParameter);
        }

        
        public Task<string> GetKeyAddress1QueryAsync(string strkey, BlockParameter blockParameter = null)
        {
            var getKeyAddress1Function = new GetKeyAddress1Function();
                getKeyAddress1Function.Strkey = strkey;
            
            return ContractHandler.QueryAsync<GetKeyAddress1Function, string>(getKeyAddress1Function, blockParameter);
        }

        public Task<string> GetKeyStringQueryAsync(GetKeyStringFunction getKeyStringFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetKeyStringFunction, string>(getKeyStringFunction, blockParameter);
        }

        
        public Task<string> GetKeyStringQueryAsync(BigInteger key, BlockParameter blockParameter = null)
        {
            var getKeyStringFunction = new GetKeyStringFunction();
                getKeyStringFunction.Key = key;
            
            return ContractHandler.QueryAsync<GetKeyStringFunction, string>(getKeyStringFunction, blockParameter);
        }

        public Task<string> GetKeyString1QueryAsync(GetKeyString1Function getKeyString1Function, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetKeyString1Function, string>(getKeyString1Function, blockParameter);
        }

        
        public Task<string> GetKeyString1QueryAsync(string strkey, BlockParameter blockParameter = null)
        {
            var getKeyString1Function = new GetKeyString1Function();
                getKeyString1Function.Strkey = strkey;
            
            return ContractHandler.QueryAsync<GetKeyString1Function, string>(getKeyString1Function, blockParameter);
        }

        public Task<string> PublishAppDownloadRequestAsync(PublishAppDownloadFunction publishAppDownloadFunction)
        {
             return ContractHandler.SendRequestAsync(publishAppDownloadFunction);
        }

        public Task<TransactionReceipt> PublishAppDownloadRequestAndWaitForReceiptAsync(PublishAppDownloadFunction publishAppDownloadFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(publishAppDownloadFunction, cancellationToken);
        }

        public Task<string> PublishAppDownloadRequestAsync(BigInteger appId, BigInteger platformId, BigInteger version, List<string> links)
        {
            var publishAppDownloadFunction = new PublishAppDownloadFunction();
                publishAppDownloadFunction.AppId = appId;
                publishAppDownloadFunction.PlatformId = platformId;
                publishAppDownloadFunction.Version = version;
                publishAppDownloadFunction.Links = links;
            
             return ContractHandler.SendRequestAsync(publishAppDownloadFunction);
        }

        public Task<TransactionReceipt> PublishAppDownloadRequestAndWaitForReceiptAsync(BigInteger appId, BigInteger platformId, BigInteger version, List<string> links, CancellationTokenSource cancellationToken = null)
        {
            var publishAppDownloadFunction = new PublishAppDownloadFunction();
                publishAppDownloadFunction.AppId = appId;
                publishAppDownloadFunction.PlatformId = platformId;
                publishAppDownloadFunction.Version = version;
                publishAppDownloadFunction.Links = links;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(publishAppDownloadFunction, cancellationToken);
        }

        public Task<string> PublishAppVersionRequestAsync(PublishAppVersionFunction publishAppVersionFunction)
        {
             return ContractHandler.SendRequestAsync(publishAppVersionFunction);
        }

        public Task<TransactionReceipt> PublishAppVersionRequestAndWaitForReceiptAsync(PublishAppVersionFunction publishAppVersionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(publishAppVersionFunction, cancellationToken);
        }

        public Task<string> PublishAppVersionRequestAsync(BigInteger appId, BigInteger platformId, BigInteger version, byte[] sha256Value, string appName, string updateInfo, string iconUri)
        {
            var publishAppVersionFunction = new PublishAppVersionFunction();
                publishAppVersionFunction.AppId = appId;
                publishAppVersionFunction.PlatformId = platformId;
                publishAppVersionFunction.Version = version;
                publishAppVersionFunction.Sha256Value = sha256Value;
                publishAppVersionFunction.AppName = appName;
                publishAppVersionFunction.UpdateInfo = updateInfo;
                publishAppVersionFunction.IconUri = iconUri;
            
             return ContractHandler.SendRequestAsync(publishAppVersionFunction);
        }

        public Task<TransactionReceipt> PublishAppVersionRequestAndWaitForReceiptAsync(BigInteger appId, BigInteger platformId, BigInteger version, byte[] sha256Value, string appName, string updateInfo, string iconUri, CancellationTokenSource cancellationToken = null)
        {
            var publishAppVersionFunction = new PublishAppVersionFunction();
                publishAppVersionFunction.AppId = appId;
                publishAppVersionFunction.PlatformId = platformId;
                publishAppVersionFunction.Version = version;
                publishAppVersionFunction.Sha256Value = sha256Value;
                publishAppVersionFunction.AppName = appName;
                publishAppVersionFunction.UpdateInfo = updateInfo;
                publishAppVersionFunction.IconUri = iconUri;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(publishAppVersionFunction, cancellationToken);
        }

        public Task<string> SaveKeyAddressRequestAsync(SaveKeyAddressFunction saveKeyAddressFunction)
        {
             return ContractHandler.SendRequestAsync(saveKeyAddressFunction);
        }

        public Task<TransactionReceipt> SaveKeyAddressRequestAndWaitForReceiptAsync(SaveKeyAddressFunction saveKeyAddressFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(saveKeyAddressFunction, cancellationToken);
        }

        public Task<string> SaveKeyAddressRequestAsync(BigInteger key, string value)
        {
            var saveKeyAddressFunction = new SaveKeyAddressFunction();
                saveKeyAddressFunction.Key = key;
                saveKeyAddressFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(saveKeyAddressFunction);
        }

        public Task<TransactionReceipt> SaveKeyAddressRequestAndWaitForReceiptAsync(BigInteger key, string value, CancellationTokenSource cancellationToken = null)
        {
            var saveKeyAddressFunction = new SaveKeyAddressFunction();
                saveKeyAddressFunction.Key = key;
                saveKeyAddressFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(saveKeyAddressFunction, cancellationToken);
        }

        public Task<string> SaveKeyAddress1RequestAsync(SaveKeyAddress1Function saveKeyAddress1Function)
        {
             return ContractHandler.SendRequestAsync(saveKeyAddress1Function);
        }

        public Task<TransactionReceipt> SaveKeyAddress1RequestAndWaitForReceiptAsync(SaveKeyAddress1Function saveKeyAddress1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(saveKeyAddress1Function, cancellationToken);
        }

        public Task<string> SaveKeyAddress1RequestAsync(string strkey, string value)
        {
            var saveKeyAddress1Function = new SaveKeyAddress1Function();
                saveKeyAddress1Function.Strkey = strkey;
                saveKeyAddress1Function.Value = value;
            
             return ContractHandler.SendRequestAsync(saveKeyAddress1Function);
        }

        public Task<TransactionReceipt> SaveKeyAddress1RequestAndWaitForReceiptAsync(string strkey, string value, CancellationTokenSource cancellationToken = null)
        {
            var saveKeyAddress1Function = new SaveKeyAddress1Function();
                saveKeyAddress1Function.Strkey = strkey;
                saveKeyAddress1Function.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(saveKeyAddress1Function, cancellationToken);
        }

        public Task<string> SaveKeyStringRequestAsync(SaveKeyStringFunction saveKeyStringFunction)
        {
             return ContractHandler.SendRequestAsync(saveKeyStringFunction);
        }

        public Task<TransactionReceipt> SaveKeyStringRequestAndWaitForReceiptAsync(SaveKeyStringFunction saveKeyStringFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(saveKeyStringFunction, cancellationToken);
        }

        public Task<string> SaveKeyStringRequestAsync(BigInteger key, string value)
        {
            var saveKeyStringFunction = new SaveKeyStringFunction();
                saveKeyStringFunction.Key = key;
                saveKeyStringFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(saveKeyStringFunction);
        }

        public Task<TransactionReceipt> SaveKeyStringRequestAndWaitForReceiptAsync(BigInteger key, string value, CancellationTokenSource cancellationToken = null)
        {
            var saveKeyStringFunction = new SaveKeyStringFunction();
                saveKeyStringFunction.Key = key;
                saveKeyStringFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(saveKeyStringFunction, cancellationToken);
        }

        public Task<string> SaveKeyString1RequestAsync(SaveKeyString1Function saveKeyString1Function)
        {
             return ContractHandler.SendRequestAsync(saveKeyString1Function);
        }

        public Task<TransactionReceipt> SaveKeyString1RequestAndWaitForReceiptAsync(SaveKeyString1Function saveKeyString1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(saveKeyString1Function, cancellationToken);
        }

        public Task<string> SaveKeyString1RequestAsync(string strkey, string value)
        {
            var saveKeyString1Function = new SaveKeyString1Function();
                saveKeyString1Function.Strkey = strkey;
                saveKeyString1Function.Value = value;
            
             return ContractHandler.SendRequestAsync(saveKeyString1Function);
        }

        public Task<TransactionReceipt> SaveKeyString1RequestAndWaitForReceiptAsync(string strkey, string value, CancellationTokenSource cancellationToken = null)
        {
            var saveKeyString1Function = new SaveKeyString1Function();
                saveKeyString1Function.Strkey = strkey;
                saveKeyString1Function.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(saveKeyString1Function, cancellationToken);
        }

        public Task<string> SetAdminRequestAsync(SetAdminFunction setAdminFunction)
        {
             return ContractHandler.SendRequestAsync(setAdminFunction);
        }

        public Task<TransactionReceipt> SetAdminRequestAndWaitForReceiptAsync(SetAdminFunction setAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setAdminFunction, cancellationToken);
        }

        public Task<string> SetAdminRequestAsync(string value)
        {
            var setAdminFunction = new SetAdminFunction();
                setAdminFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(setAdminFunction);
        }

        public Task<TransactionReceipt> SetAdminRequestAndWaitForReceiptAsync(string value, CancellationTokenSource cancellationToken = null)
        {
            var setAdminFunction = new SetAdminFunction();
                setAdminFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setAdminFunction, cancellationToken);
        }

        public Task<string> SetContactInfoRequestAsync(SetContactInfoFunction setContactInfoFunction)
        {
             return ContractHandler.SendRequestAsync(setContactInfoFunction);
        }

        public Task<TransactionReceipt> SetContactInfoRequestAndWaitForReceiptAsync(SetContactInfoFunction setContactInfoFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setContactInfoFunction, cancellationToken);
        }

        public Task<string> SetContactInfoRequestAsync(string contactInfo)
        {
            var setContactInfoFunction = new SetContactInfoFunction();
                setContactInfoFunction.ContactInfo = contactInfo;
            
             return ContractHandler.SendRequestAsync(setContactInfoFunction);
        }

        public Task<TransactionReceipt> SetContactInfoRequestAndWaitForReceiptAsync(string contactInfo, CancellationTokenSource cancellationToken = null)
        {
            var setContactInfoFunction = new SetContactInfoFunction();
                setContactInfoFunction.ContactInfo = contactInfo;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setContactInfoFunction, cancellationToken);
        }

        public Task<string> SetDonationRequestAsync(SetDonationFunction setDonationFunction)
        {
             return ContractHandler.SendRequestAsync(setDonationFunction);
        }

        public Task<TransactionReceipt> SetDonationRequestAndWaitForReceiptAsync(SetDonationFunction setDonationFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setDonationFunction, cancellationToken);
        }

        public Task<string> SetDonationRequestAsync(string value)
        {
            var setDonationFunction = new SetDonationFunction();
                setDonationFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(setDonationFunction);
        }

        public Task<TransactionReceipt> SetDonationRequestAndWaitForReceiptAsync(string value, CancellationTokenSource cancellationToken = null)
        {
            var setDonationFunction = new SetDonationFunction();
                setDonationFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setDonationFunction, cancellationToken);
        }

        public Task<string> SetSuperAdminRequestAsync(SetSuperAdminFunction setSuperAdminFunction)
        {
             return ContractHandler.SendRequestAsync(setSuperAdminFunction);
        }

        public Task<TransactionReceipt> SetSuperAdminRequestAndWaitForReceiptAsync(SetSuperAdminFunction setSuperAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setSuperAdminFunction, cancellationToken);
        }

        public Task<string> SetSuperAdminRequestAsync(string value)
        {
            var setSuperAdminFunction = new SetSuperAdminFunction();
                setSuperAdminFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(setSuperAdminFunction);
        }

        public Task<TransactionReceipt> SetSuperAdminRequestAndWaitForReceiptAsync(string value, CancellationTokenSource cancellationToken = null)
        {
            var setSuperAdminFunction = new SetSuperAdminFunction();
                setSuperAdminFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setSuperAdminFunction, cancellationToken);
        }

        public Task<string> WithdrawRequestAsync(WithdrawFunction withdrawFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawFunction);
        }

        public Task<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(WithdrawFunction withdrawFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction, cancellationToken);
        }

        public Task<string> WithdrawRequestAsync(string token)
        {
            var withdrawFunction = new WithdrawFunction();
                withdrawFunction.Token = token;
            
             return ContractHandler.SendRequestAsync(withdrawFunction);
        }

        public Task<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(string token, CancellationTokenSource cancellationToken = null)
        {
            var withdrawFunction = new WithdrawFunction();
                withdrawFunction.Token = token;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction, cancellationToken);
        }
    }
}
