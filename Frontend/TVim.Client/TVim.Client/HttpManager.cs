using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
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


        public async Task<string> Get(string url)
        {
            var response = _client.GetAsync(url);
            return await response.Result.Content.ReadAsStringAsync();
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