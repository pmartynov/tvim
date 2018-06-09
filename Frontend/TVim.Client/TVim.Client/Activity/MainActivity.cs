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
        //[BindView(Resource.Id.upload_post)]
        private Button _uploadPost;
        
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Cheeseknife.Bind(this);

            _uploadPost.Click += UploadPostClick;
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
