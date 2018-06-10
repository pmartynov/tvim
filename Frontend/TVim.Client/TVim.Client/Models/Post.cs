using Android.Net;
using Newtonsoft.Json;

namespace TVim.Client.Activity
{
    public class Post
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("acc_name")]
        public string AccauntName { get; set; }

        [JsonProperty("ipfs_hash")]
        public string IpfsHash { get; set; }


    }
}