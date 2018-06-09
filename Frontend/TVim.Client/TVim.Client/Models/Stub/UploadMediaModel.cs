using System.IO;
using Newtonsoft.Json;

namespace TVim.Client.Helpers
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UploadMediaModel
    {
        public string Url { get; set; }

        public string ContentType { get; } = "image/jpeg";

        public string VerifyTransaction { get; set; }

        public bool GenerateThumbnail { get; set; } = false;

    }
}