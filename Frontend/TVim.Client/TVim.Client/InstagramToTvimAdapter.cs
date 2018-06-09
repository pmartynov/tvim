using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using TVim.Client.Helpers;
using TVim.Client.Models;

namespace TVim.Client
{
    public class InstagramToTvimAdapter
    {
        protected const string GetRecentMediaUrl = "https://api.instagram.com/v1/users/self/media/recent/?access_token=";
        protected const string AccessTokenKeyName = "insagram_access_token";


        public async Task<OperationResult<InstagramResult>> GetLastPosts(Context context, HttpManager client, CancellationToken token)
        {
            //https://www.instagram.com/developer/authentication/ < how to get access_token (OAuth skiped. hardcoded key used instead... time time..)
            var assetsHelper = new AssetsHelper(context.Assets);
            var debugAsset = assetsHelper.TryReadAsset<Models.DebugAsset>("DebugAsset.txt");
            var fullUrl = GetRecentMediaUrl + debugAsset.InsagramAccessToken;
            var responce = await client.GetRequest<InstagramResult>(fullUrl, token);
            //now needed only > responce.Result.data.First().images.standard_resolution
            return responce;
        }
    }
}