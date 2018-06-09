using Newtonsoft.Json;

namespace TVim.Client.Models
{
    public class DebugAsset
    {
        [JsonProperty("insagram_access_token")]
        public string InsagramAccessToken { get; set; }

        [JsonProperty("authorization_transaction_json")]
        public string AuthorizationTransactionJson { get; set; }

        [JsonProperty("chain_url")]
        public string ChainUrl { get; set; }

        public string LastLoadedMediaId { get; set; }
    }
}