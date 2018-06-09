using System.Collections.Generic;
using System.Linq;
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
using TVim.Client.Helpers;
using TVim.Client.Models;

namespace TVim.Client.Activity
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        const string AccountName = "test";
        const string ContractName = "test";
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
            var posts = new List<Post> { };

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
            await GetPost();
            //var post = await _instagramToTvimAdapter.GetLastPost(_httpManager, _debugAsset, CancellationToken.None);
            //if (post == null)
            //    return;

            await CreatePost(null);
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

        public async Task CreatePost(Post post)
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

            var message = new Messages
            {
                code = AccountName,
                type = CreatePostActionName,
                permission = new[]
                 {
                    new PermissionLevel
                    {
                        account = AccountName,
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
                scope = new[] { AccountName },
                messages = new[] { message },
                signatures = new string[0]
            };

            var signTransaction = await _httpManager.PostRequest<AbiJsonToBinResult>($"{_debugAsset.WalletUrl}/sign_transaction",
                new object[]
                {
                    pushTransactionArgs,
                    new[]
                    {
                        getAccount.Result.permissions.FirstOrDefault(p => p.perm_name == "active").required_auth.keys[0].key
                    },
                    ""
                }, token);

            var r = signTransaction;

            //const std::string& acc_name, const std::string& url, const std::string& hash, const std::string& requesting_acc_name, const std::string& key
        }
    }
}




