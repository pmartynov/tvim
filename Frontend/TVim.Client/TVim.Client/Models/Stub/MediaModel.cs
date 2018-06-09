using Newtonsoft.Json;

namespace TVim.Client
{
    public class MediaModel
    {
        public Thumbnails thumbnails { get; set; }

        public string url { get; set; }

        public string ipfs_hash { get; set; }

        public FrameSize size { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class FrameSize
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }


        public FrameSize() { }

        public FrameSize(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public override string ToString()
        {
            return $"{Height}x{Width}";
        }
    }

    public class Thumbnails
    {
        private string _micro;
        private string _mini;

        /// <summary>
        /// *256x256
        /// </summary>
        [JsonProperty("256")]
        public string Micro
        {
            get => _micro ?? Mini;
            set => _micro = value;
        }

        /// <summary>
        /// *1024x1024
        /// </summary>
        [JsonProperty("1024")]
        public string Mini
        {
            get => _mini ?? DefaultUrl;
            set => _mini = value;
        }


        [JsonIgnore]
        public string DefaultUrl { get; set; }
    }
}