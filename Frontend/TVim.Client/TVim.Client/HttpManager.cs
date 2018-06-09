using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TVim.Client.Helpers;
using TVim.Client.Models;
using TVim.Client.Models.Errors;


namespace TVim.Client
{
    public class HttpManager
    {
        #region Fields

        private readonly System.Net.Http.HttpClient _client;


        #endregion

        #region Constructors


        public HttpManager()
        {
            _client = new HttpClient();
        }

        #endregion

        #region Functions

        public async Task<OperationResult<T>> GetRequest<T>(string url, CancellationToken token)
        {
            try
            {
                var response = await _client.GetAsync(url, token);
                return await CreateResult<T>(response, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<OperationResult<T>> PostRequest<T>(string url, string param, CancellationToken token)
        {
            try
            {
                HttpContent content = null;
                if (!string.IsNullOrEmpty(param))
                {
                    content = new StringContent(param, Encoding.UTF8, "application/json");
                }

                var response = await _client.PostAsync(url, content, token);
                return await CreateResult<T>(response, token);
            }
            catch (Exception e)
            {
                var t = e.Message;
            }

            return null;
        }

        public async Task<OperationResult<T>> PostRequest<T>(string url, object data, CancellationToken token)
        {
            try
            {
                HttpContent content = null;
                if (data != null)
                {
                    var param = JsonConvert.SerializeObject(data);
                    content = new StringContent(param, Encoding.UTF8, "application/json");
                }

                var response = await _client.PostAsync(url, content, token);
                return await CreateResult<T>(response, token);
            }
            catch (Exception e)
            {
                var t = e.Message;
            }

            return null;
        }

        public async Task<string> Get(string url)
        {
            var response = _client.GetAsync(url);
            return await response.Result.Content.ReadAsStringAsync();
        }

        public async Task<OperationResult<MediaModel>> UploadMedia(string url, UploadMediaModel model, CancellationToken token)
        {
            var fTitle = Guid.NewGuid().ToString();

            MemoryStream stream;
            using (WebClient client = new WebClient())
            {
                var bytes = client.DownloadData(new Uri(model.Url));
                stream = new MemoryStream(bytes);
            }

            var file = new StreamContent(stream);
            file.Headers.ContentType = MediaTypeHeaderValue.Parse(model.ContentType);
            var multiContent = new MultipartFormDataContent
            {
                {new StringContent(model.VerifyTransaction), "trx"},
                {file, "file", fTitle},
                {new StringContent(model.GenerateThumbnail.ToString()), "generate_thumbnail"}
            };

            var response = await _client.PostAsync(url, multiContent, token);
            var result = await CreateResult<MediaModel>(response, token);

            if (result.IsError && result.Result == null)
                result.Error = new ErrorBase("Can`t upload image!");

            return result;
        }

        protected virtual async Task<OperationResult<T>> CreateResult<T>(HttpResponseMessage response, CancellationToken ct)
        {
            var result = new OperationResult<T>();

            // HTTP error
            if (response.StatusCode == HttpStatusCode.InternalServerError ||
                response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Created)
            {
                var content = await response.Content.ReadAsStringAsync();
                result.Error = new ServerError(response.StatusCode, content);
                return result;
            }

            if (response.Content == null)
                return result;

            var mediaType = response.Content.Headers?.ContentType?.MediaType.ToLower();
            if (mediaType != null)
            {
                if (mediaType.Equals("application/json"))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                        result.Result = JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }
        #endregion
    }
}