namespace TVim.Client.Activity
{
    public partial class MainActivity
    {
        public class CreatePostArgs
        {
            public string creator { get; set; }
            public string url_photo { get; set; }
            public string hash_photo { get; set; }
        }


        //    create_account ‘{“account”:“test”}’
        //transfer ‘{“from”:“test”,“to”:“test2",“amount”:“1.0000 VIM”,“url_photo”:“url”,“hash_photo”:“hash”}’
        //create_post ‘{“creator”:“test”,“url_photo”:“url”,“hash_photo”:“hash”}’

        //public async Task CreatePost2(Post post)
        //{
        //    var token = CancellationToken.None;

        //    //1
        //    var getInfo = await _httpManager.PostRequest<GetInfoResult>($"{_debugAsset.ChainUrl}/get_info", null, token);

        //    //2
        //    var blockArgs = new GetBlockArgs
        //    {
        //        block_num_or_id = getInfo.Result.last_irreversible_block_id
        //    };
        //    var getBlock = await _httpManager.PostRequest<GetBlockResult>($"{_debugAsset.ChainUrl}/get_block", blockArgs, token);

        //    //3
        //    var accountArgs = new GetAccountArgs
        //    {
        //        account_name = AccountName
        //    };
        //    var getAccount = await _httpManager.PostRequest<GetAccountResult>($"{_debugAsset.ChainUrl}/get_account", accountArgs, token);

        //    //4
        //    var codeArgs = new GetCodeArgs
        //    {
        //        account_name = ContractName
        //    };
        //    var getCode = await _httpManager.PostRequest<GetCodeResult>($"{_debugAsset.ChainUrl}/get_code", codeArgs, token);

        //    //5
        //    var abiJsonToBinArgs = new AbiJsonToBinParams
        //    {
        //        Code = ContractName,
        //        Action = CreatePostActionName,
        //        Args = new CreatePostArgs
        //        {
        //            url_photo = "test_1_url",
        //            creator = AccountName,
        //            hash_photo = "test_1_hash"
        //        }
        //    };
        //    var abiJsonToBin = await _httpManager.PostRequest<AbiJsonToBinResult>($"{_debugAsset.ChainUrl}/abi_json_to_bin", abiJsonToBinArgs, token);

        //    var t = await _httpManager.PostRequest<JObject>($"{_debugAsset.WalletUrl}/unlock", new[] { AccountName, _debugAsset.MasterPrivateKey }, token);
        //    var tt = t;

        //    var message = new Messages
        //    {
        //        code = AccountName,
        //        type = CreatePostActionName,
        //        permission = new[]
        //         {
        //            new PermissionLevel
        //            {
        //                account = AccountName,
        //                permission = "active"
        //            }
        //        },
        //        data = abiJsonToBin.Result.binargs
        //    };


        //    var pushTransactionArgs = new PushTransactionArgs
        //    {
        //        //ref_block_num = (ushort)(getInfo.Result.head_block_num & 0xffff),
        //        //ref_block_prefix = (uint)BitConverter.ToInt32(Hex.HexToBytes(getInfo.Result.head_block_id), 4),


        //        //ref_block_num = (ushort)(getBlock.Result.block_num & 0xffff),
        //        //ref_block_prefix = (uint)BitConverter.ToInt32(Hex.HexToBytes(getBlock.Result.id), 4),

        //        ref_block_num = getBlock.Result.block_num,
        //        ref_block_prefix = getBlock.Result.ref_block_prefix,
        //        expiration = getBlock.Result.timestamp.AddSeconds(30),
        //        scope = new[] { AccountName },
        //        messages = new[] { message },
        //        signatures = new string[0]
        //    };

        //    var signTransaction = await _httpManager.PostRequest<AbiJsonToBinResult>($"{_debugAsset.WalletUrl}/sign_transaction",
        //        new object[]
        //        {
        //            pushTransactionArgs,
        //            new[]
        //            {
        //                getAccount.Result.permissions.FirstOrDefault(p => p.perm_name == "active").required_auth.keys[0].key
        //            },
        //            ""
        //        }, token);

        //    var r = signTransaction;

        //    //const std::string& acc_name, const std::string& url, const std::string& hash, const std::string& requesting_acc_name, const std::string& key
        //}
    }
}




