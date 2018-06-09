using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TVim.Client.Helpers;
using TVim.Client.Models;

namespace TVim.Client
{
    public class InstagramToTvimAdapter
    {
        protected const string GetRecentMediaUrl = "https://api.instagram.com/v1/users/self/media/recent/?count=1&access_token=";
        protected const string AccessTokenKeyName = "insagram_access_token";


        public async Task<OperationResult<MediaModel>> GetLastPosts(HttpManager client, DebugAsset debugAsset, CancellationToken token)
        {
            //https://www.instagram.com/developer/authentication/ < how to get access_token (OAuth skiped. hardcoded key used instead... time time..)v
            var fullUrl = GetRecentMediaUrl + debugAsset.InsagramAccessToken;
            if (!string.IsNullOrEmpty(debugAsset.LastLoadedMediaId))
                fullUrl += "&MIN_ID=" + debugAsset.LastLoadedMediaId;
            var responce = await client.GetRequest<InstagramResult>(fullUrl, token);

            if (responce.IsError)
                throw new NotImplementedException(responce.Error.Msg);

            var media = responce.Result.data.FirstOrDefault();
            if (debugAsset.LastLoadedMediaId != null && debugAsset.LastLoadedMediaId.Equals(media.id))
                return null;

            debugAsset.LastLoadedMediaId = media.id;

            //save image to IPFS
            var model = new UploadMediaModel
            {
                VerifyTransaction = debugAsset.AuthorizationTransactionJson,
                Url = media.images.standard_resolution.url
            };

            return await client.UploadMedia("https://steepshot.org/api/v1_1/media/upload", model, token);
        }
    }
}