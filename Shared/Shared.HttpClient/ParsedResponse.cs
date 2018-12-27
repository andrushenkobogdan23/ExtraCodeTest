using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

namespace Shared.HttpClient
{
    public class ParsedResponse
    {
        public ParsedResponse(HttpResponseMessage response)
        {
            Response = response;
            Code = response.StatusCode;
            Version = response.Version.ToString();
            IsSuccessful = response.IsSuccessStatusCode;

            if (!IsSuccessful)// || Code == HttpStatusCode.Created)
            {
                Content = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(Content))
                {
                    Error = JObject.Parse(Content);
                    HasError = true;
                }
            }
        }

        public bool IsSuccessful { get; set; }
        public HttpStatusCode Code { get; set; }
        public string Version { get; set; }

        public HttpResponseMessage Response { get; set; }
        public string Content { get; set; }
        public JObject Error { get; set; }
        public bool HasError { get; set; }
    }
}
