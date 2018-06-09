using System.IO;
using Android.Content.Res;
using Newtonsoft.Json;

namespace TVim.Client.Helpers
{
    public class AssetsHelper
    {
        private readonly AssetManager _assetManager;

        public AssetsHelper(AssetManager assetManager)
        {
            _assetManager = assetManager;
        }


        public T TryReadAsset<T>(string file) where T : new()
        {
            try
            {
                string txt;
                var stream = _assetManager.Open(file);
                using (var sr = new StreamReader(stream))
                {
                    txt = sr.ReadToEnd();
                }
                stream.Dispose();
                if (!string.IsNullOrWhiteSpace(txt))
                {
                    return JsonConvert.DeserializeObject<T>(txt);
                }
            }
            catch
            {
                //to do nothing
            }
            return new T();
        }

    }
}