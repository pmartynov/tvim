using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using CheeseBind;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TVim.Client.Helpers;
using TVim.Client.Models;

namespace TVim.Client.Activity
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public partial class MainActivity : AppCompatActivity
    {
        const string AccountName = "currency";
        const string ContractName = "currency";
        const string CreatePostActionName = "createpost";

        [BindView(Resource.Id.upload_post)]
        private Button _uploadPost;

        [BindView(Resource.Id.post_list)]
        private RecyclerView _postList;


        private HttpManager _httpManager;
        private InstagramToTvimAdapter _instagramToTvimAdapter;
        private DebugAsset _debugAsset;
        private PostAdapter _adapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Cheeseknife.Bind(this);


            _uploadPost.Click += UploadPostClick;
            var posts = new List<Post>
            {
                new Post
                {
                    Url = "https://static.eos.io/images/Landing/SectionResourceLanding/DevPortLaunch_Social_Eosio-home_opt.jpg",
                    AccauntName = ""
                }
            };

            _adapter = new PostAdapter(this, posts);
            _postList.SetAdapter(_adapter);
            _postList.SetLayoutManager(new LinearLayoutManager(Android.App.Application.Context));
            _httpManager = new HttpManager();
            _instagramToTvimAdapter = new InstagramToTvimAdapter();
            var assetsHelper = new AssetsHelper(Assets);
            _debugAsset = assetsHelper.TryReadAsset<DebugAsset>("DebugAsset.txt");

        }

        private async void UploadPostClick(object sender, System.EventArgs e)
        {
            var operationResult = await _instagramToTvimAdapter.GetLastPost(_httpManager, _debugAsset, CancellationToken.None);
            if (operationResult == null)
                return;

            var post = new Post
            {
                Url = operationResult.Result.url,
                AccauntName = AccountName,
                IpfsHash = operationResult.Result.ipfs_hash
            };
            _adapter.Add(post);


            //await GetPost();
            
            //await CreatePost(operationResult.Result, CancellationToken.None);
            //await CreatePost();

        }

        private async Task GetPost()
        {
            var args = new GetPostArgs()
            {
                scope = "test",
                code = "test",
                table = "posts",
                json = "true"
            };
            var resp = await _httpManager.PostRequest<GetTableRowsResult>($"{_debugAsset.ChainUrl}/get_table_rows", args, CancellationToken.None);
            if (resp.IsError)
                return;

            var posts = resp.Result.rows?.Where(i => !string.IsNullOrWhiteSpace(i.url_photo)).Select(i => new Post { Url = i.url_photo, AccauntName = i.creator }).ToList();
            if (posts == null || !posts.Any())
                return;

            _adapter.Update(posts);
        }

        //public async Task CreatePost(MediaModel model, CancellationToken token)
        //{
        //    var args = new CreatePostArgs
        //    {
        //        creator = AccountName,
        //        url_photo = model.url,
        //        hash_photo = model.ipfs_hash

        //    };
        //    var getInfo = await _httpManager.PostRequest<JObject>($"{_debugAsset.PluginUrl}/create_post", args, token);
        //}


        //create_account ‘{“account”:“test”}’
        //transfer ‘{“from”:“test”,“to”:“test2",“amount”:“1.0000 VIM”,“url_photo”:“url”,“hash_photo”:“hash”}’
        //create_post ‘{“creator”:“test”,“url_photo”:“url”,“hash_photo”:“hash”}’

        public async Task CreatePost()
        {
            var token = CancellationToken.None;

            //1
            var getInfo = await _httpManager.PostRequest<GetInfoResult>($"{_debugAsset.ChainUrl}/get_info", null, token);

            //2
            var blockArgs = new GetBlockArgs
            {
                block_num_or_id = getInfo.Result.last_irreversible_block_id
            };
            var getBlock = await _httpManager.PostRequest<GetBlockResult>($"{_debugAsset.ChainUrl}/get_block", blockArgs, token);

            //3
            var accountArgs = new GetAccountArgs
            {
                account_name = AccountName
            };
            var getAccount = await _httpManager.PostRequest<GetAccountResult>($"{_debugAsset.ChainUrl}/get_account", accountArgs, token);

            //4
            var codeArgs = new GetCodeArgs
            {
                account_name = ContractName
            };
            var getCode = await _httpManager.PostRequest<GetCodeResult>($"{_debugAsset.ChainUrl}/get_code", codeArgs, token);

            //5
            var abiJsonToBinArgs = new AbiJsonToBinParams
            {
                Code = ContractName,
                Action = CreatePostActionName,
                Args = new CreatePostArgs
                {
                    url_photo = "test_1_url",
                    creator = AccountName,
                    hash_photo = "test_1_hash"
                }
            };
            var abiJsonToBin = await _httpManager.PostRequest<AbiJsonToBinResult>($"{_debugAsset.ChainUrl}/abi_json_to_bin", abiJsonToBinArgs, token);

            var t = await _httpManager.PostRequest<JObject>($"{_debugAsset.WalletUrl}/unlock", new[] { AccountName, _debugAsset.MasterPrivateKey }, token);
            var tt = t;

            var action = new Action
            {
                account = AccountName,
                name = CreatePostActionName,
                authorization = new[]
                 {
                    new Authorization()
                    {
                        actor = AccountName,
                        permission = "active"
                    }
                },
                data = abiJsonToBin.Result.binargs
            };


            var pushTransactionArgs = new PushTransactionArgs
            {
                //ref_block_num = (ushort)(getInfo.Result.head_block_num & 0xffff),
                //ref_block_prefix = (uint)BitConverter.ToInt32(Hex.HexToBytes(getInfo.Result.head_block_id), 4),


                //ref_block_num = (ushort)(getBlock.Result.block_num & 0xffff),
                //ref_block_prefix = (uint)BitConverter.ToInt32(Hex.HexToBytes(getBlock.Result.id), 4),

                ref_block_num = getBlock.Result.block_num,
                ref_block_prefix = getBlock.Result.ref_block_prefix,
                expiration = getBlock.Result.timestamp.AddSeconds(30),


                actions = new[] { action },
                signatures = new string[0]
            };

            var t2 = new object[]
            {
                pushTransactionArgs,
                new[]
                {
                    getAccount.Result.permissions.FirstOrDefault(p => p.perm_name == "active").required_auth.keys[0].key
                },
                ""
            };

            var signTransaction = await _httpManager.PostRequest<SignTransactionResult>($"{_debugAsset.WalletUrl}/sign_transaction", t2, token);

            pushTransactionArgs.signatures = signTransaction.Result.signatures.ToArray();
            var t31 = await _httpManager.PostRequest<JObject>($"{_debugAsset.PluginUrl}/get_trx", pushTransactionArgs, token);


            var push_transaction_args = new push_transaction_args
            {
                transaction = pushTransactionArgs,
                signatures = pushTransactionArgs.signatures
            };
            var t3 = await _httpManager.PostRequest<JObject>($"{_debugAsset.ChainUrl}/push_transaction", push_transaction_args, token);

            var tt1 = t3;
            //const std::string& acc_name, const std::string& url, const std::string& hash, const std::string& requesting_acc_name, const std::string& key
        }

        public class push_transaction_args
        {
            public string compression { get; set; } = "none";
            public object transaction { get; set; }
            public string[] signatures { get; set; }

        }
    }
}




