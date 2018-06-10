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

        [JsonProperty("wallet_url")]
        public string WalletUrl { get; set; }

        [JsonProperty("plugin_url")]
        public string PluginUrl { get; set; }

        public string LastLoadedMediaId { get; set; }

        [JsonProperty("master_private_key")]
        public string MasterPrivateKey { get; set; }
    }
}