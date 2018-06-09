using System.Threading;
using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace TVim.Client.Activity
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var adapter = new InstagramToTvimAdapter();
            var httpManager = new HttpManager();
            var posts = await adapter.GetLastPosts(this, httpManager, CancellationToken.None);
        }
    }
}

