using Newtonsoft.Json;
using SerilogTimings;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.HttpClient
{
    public static class HttpRequestFactory
    {
        public static async Task<ParsedResponse> Get(string requestUri)
            => await Get(requestUri, null);

        public static async Task<ParsedResponse> Get(string requestUri, string bearerToken)
            => await Get(requestUri, bearerToken, null);

        public static async Task<ParsedResponse> Get(string requestUri, string bearerToken, string apiVersion)
        {
            return await FireAsync(HttpMethod.Get, requestUri, bearerToken, apiVersion);
        }

        public static async Task<ParsedResponse> Post(string requestUri, object value)
            => await Post(requestUri, value, null);

        public static async Task<ParsedResponse> Post(string requestUri, object value, string bearerToken)
            => await Post(requestUri, value, bearerToken, null);

        public static async Task<ParsedResponse> Post(
            string requestUri, object value, string bearerToken, string apiVersion)
        {
            return await FireAsync(HttpMethod.Post, requestUri, bearerToken, apiVersion, value);
        }

        public static async Task<ParsedResponse> Put(string requestUri, object value)
            => await Put(requestUri, value, null);

        public static async Task<ParsedResponse> Put(string requestUri, object value, string bearerToken)
            => await Put(requestUri, value, bearerToken, null);

        public static async Task<ParsedResponse> Put(
            string requestUri, object value, string bearerToken, string apiVersion)
        {
            return await FireAsync(HttpMethod.Put, requestUri, bearerToken, apiVersion, value);
        }

        public static async Task<ParsedResponse> Patch(string requestUri, object value)
            => await Patch(requestUri, value, null);

        public static async Task<ParsedResponse> Patch(
            string requestUri, object value, string bearerToken)
        {
            return await FireAsync(new HttpMethod("PATCH"), requestUri, bearerToken, null, value);
        }

        public static async Task<ParsedResponse> Delete(string requestUri)
            => await Delete(requestUri, null);

        public static async Task<ParsedResponse> Delete(string requestUri, string bearerToke)
            => await Delete(requestUri, bearerToke, null);

        public static async Task<ParsedResponse> Delete(
            string requestUri, string bearerToken, string apiVersion)
        {
            return await FireAsync(HttpMethod.Delete, requestUri, bearerToken, apiVersion);
        }

        public static async Task<ParsedResponse> PostFile(string requestUri,
            string filePath, string apiParamName)
            => await PostFile(requestUri, filePath, apiParamName, "");

        public static async Task<ParsedResponse> PostFile(string requestUri,
            string filePath, string apiParamName, string bearerToken)
        {
            return await FireAsync(HttpMethod.Post, requestUri, bearerToken, null, new FileContent(filePath, apiParamName));
        }


        private static async Task<ParsedResponse> FireAsync(HttpMethod method, string uri, string token = null, string version = null, object obj = null)
        {
            ParsedResponse result;
            var value = new JsonContent(obj);

            var builder = new HttpRequestBuilder()
                                .AddMethod(method)
                                .AddRequestUri(uri)
                                .AddApiVersion(version)
                                .AddBearerToken(token)
                                .AddContent(value);

            using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("Request [{0}]\t[{1}]\t{2}", method, uri, JsonConvert.SerializeObject(obj)))
            {
                var response = await builder.SendAsync();
                result = new ParsedResponse(response);
                op.Complete();
            }
            return result;
        }
    }
}
