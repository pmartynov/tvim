using Newtonsoft.Json;

namespace TVim.Client.Models
{
    public class DebugAsset
    {
        [JsonProperty("insagram_access_token")]
        public string InsagramAccessToken { get; set; }

        [JsonProperty("authorization_transaction_json")]
        public string AuthorizationTransactionJson { get; set; }

        public string LastLoadedMediaId { get; set; }
    }
}