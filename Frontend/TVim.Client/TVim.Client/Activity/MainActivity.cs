using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using CheeseBind;
using TVim.Client.Helpers;
using TVim.Client.Models;

namespace TVim.Client.Activity
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        [BindView(Resource.Id.upload_post)]
        private Button _uploadPost;


        [BindView(Resource.Id.post_list)]
        private RecyclerView _postList;

        private PostAdapter _adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Cheeseknife.Bind(this);

            _uploadPost.Click += UploadPostClick;
            var posts = new List<Post>
            {
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
                new Post { Url = "https://steemitimages.com/DQmXCa3t19HZyD1psFPdMr3Vttszi75k1oDV6dx5qAnSocY/Steepshot_framed.png"},
            };

            _adapter = new PostAdapter(this, posts);
            _postList.SetAdapter(_adapter);
            _postList.SetLayoutManager(new LinearLayoutManager(Android.App.Application.Context));
        }

        private async void UploadPostClick(object sender, System.EventArgs e)
        {
            var adapter = new InstagramToTvimAdapter();
            var httpManager = new HttpManager();

            var assetsHelper = new AssetsHelper(Assets);
            var debugAsset = assetsHelper.TryReadAsset<DebugAsset>("DebugAsset.txt");

            var posts = await adapter.GetLastPosts(httpManager, debugAsset, CancellationToken.None);
            if (posts == null)
                return;

            //upload to node
        }

    }
}
