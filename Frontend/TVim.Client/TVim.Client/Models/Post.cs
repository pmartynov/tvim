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
    }
}