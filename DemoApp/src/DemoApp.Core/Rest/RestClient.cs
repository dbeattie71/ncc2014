using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Core.Rest
{
    public class RestClient
    {
        public string BaseUrl { get; set; }

        public string DateFormat { get; set; }

        public string UserAgent { get; set; }

        public CookieContainer CookieContainer { get; set; }

        public RestClient()
        {
            Headers = new List<KeyValuePair<string, string>>();
            CookieContainer = new CookieContainer();
        }

        public async Task<RestResponse> ExecuteAsync(RestRequest restRequest)
        {
            return await ExecuteAsync(restRequest, GetHttpMethod(restRequest.Method));
        }

        public async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest restRequest)
        {
            var restResponse = await ExecuteAsync(restRequest);
            var restResponseT = restResponse.ToRestResponse<T>();

            try
            {
                if (restResponseT.StatusCode == HttpStatusCode.OK)
                    restResponseT.Data = restResponseT.Content.FromJson<T>();

                return restResponseT;
            }
            catch (Exception exception)
            {
                restResponseT.ResponseStatus = ResponseStatus.Error;
                restResponseT.ErrorMessage = exception.Message;
                restResponseT.ErrorException = exception;
            }

            return restResponseT;
        }

        public void AddHeader(string key, string value)
        {
            Headers.Add(new KeyValuePair<string, string>(key, value));
        }

        public void SetUserAgent<T>(string displayName = null)
        {
            var thisAssembly = typeof (T).Assembly;
            var thisAssemblyName = new AssemblyName(thisAssembly.FullName);
            var thisVersion = thisAssemblyName.Version;

            if (displayName == null)
            {
                var attributes = thisAssembly.GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
                if (attributes.Length == 0)
                    throw new Exception(
                        "The assembly containing the class inheriting from PortableRest.RestClient must have an AssemblyTitle attribute specified.");
                displayName = ((AssemblyTitleAttribute) attributes[0]).Title;
            }

            var prAssembly = typeof (RestRequest).Assembly;
            var prAssemblyName = new AssemblyName(prAssembly.FullName);
            var prVersion = prAssemblyName.Version;

            UserAgent = string.Format("{0} {1} (Core.Rest {2})", displayName, thisVersion.ToString(3),
                                      prVersion.ToString(3));
        }

        private List<KeyValuePair<string, string>> Headers { get; set; }

        private static HttpMethod GetHttpMethod(Method method)
        {
            switch (method)
            {
                case Method.Post:
                    return HttpMethod.Post;
                case Method.Put:
                    return HttpMethod.Put;
                default:
                    return HttpMethod.Get;
            }
        }

        private async Task<RestResponse> ConvertToRestResponse(RestRequest restRequest,
                                                               HttpResponseMessage httpResponseMessage)
        {
            var restResponse = new RestResponse
            {
                Content = await httpResponseMessage.Content.ReadAsStringAsync(),
                StatusCode = httpResponseMessage.StatusCode
            };

            var httpResponseHeaders = httpResponseMessage.Headers;
            foreach (var httpResponseHeader in httpResponseHeaders)
                restResponse.Headers.Add(new Parameter
                {
                    Name = httpResponseHeader.Key,
                    Value = httpResponseHeader.Value, //TODO: check this
                    Type = ParameterType.HttpHeader
                });

            //var cookieCollection = CookieContainer.GetCookies(new Uri(restRequest.Resource));
            //foreach (Cookie cookie in cookieCollection)
            //    restResponse.Cookies.Add(new RestResponseCookie
            //    {
            //        Comment = cookie.Comment,
            //        CommentUri = cookie.CommentUri,
            //        Discard = cookie.Discard,
            //        Domain = cookie.Domain,
            //        Expired = cookie.Expired,
            //        Expires = cookie.Expires,
            //        HttpOnly = cookie.HttpOnly,
            //        Name = cookie.Name,
            //        Path = cookie.Path,
            //        Port = cookie.Port,
            //        Secure = cookie.Secure,
            //        TimeStamp = cookie.TimeStamp,
            //        Value = cookie.Value,
            //        Version = cookie.Version
            //    });

            return restResponse;
        }

        private async Task<RestResponse> ExecuteAsync(RestRequest restRequest, HttpMethod httpMethod)
        {
            var restResponse = new RestResponse();
            try
            {
                if (string.IsNullOrWhiteSpace(restRequest.DateFormat) && !string.IsNullOrWhiteSpace(DateFormat))
                    restRequest.DateFormat = DateFormat;

                var handler = new HttpClientHandler {AllowAutoRedirect = true};
                
				if (handler.SupportsAutomaticDecompression)
                    handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                if (CookieContainer != null)
                    handler.CookieContainer = CookieContainer;

                _httpClient = HttpClientFactory.Get();

                if (string.IsNullOrWhiteSpace(UserAgent))
                    SetUserAgent<RestClient>();
				//_httpClient.DefaultRequestHeaders.Add("user-agent", "DemoApp");

                var httpRequestMessage = new HttpRequestMessage(httpMethod, restRequest.GetResourceUri(BaseUrl));

                foreach (var header in Headers)
                    httpRequestMessage.Headers.Add(header.Key, header.Value);

                foreach (var header in restRequest.Headers)
                    httpRequestMessage.Headers.Add(header.Key, header.Value.ToString());

                if (httpMethod != HttpMethod.Get && httpMethod != HttpMethod.Head && httpMethod != HttpMethod.Trace)
                    if (restRequest.ContentType == ContentTypes.ByteArray)
                        httpRequestMessage.Content =
                            new ByteArrayContent(restRequest.Parameters[0].GetEncodedValue() as byte[]);
                    else if (restRequest.ContentType == ContentTypes.MultipartFormData)
                    {
                        var multipartPartFormDataContent = new MultipartFormDataContent();
                        var contentJson = new StringContent(restRequest.GetRequestBody());
                        multipartPartFormDataContent.Add(contentJson);
                        httpRequestMessage.Content = contentJson;

                        if (restRequest.Files.Any())
                            foreach (var fileParameter in restRequest.Files)
                            {
                                var streamContent =
                                    new StreamContent(new MemoryStream(fileParameter.Data, 0, fileParameter.Data.Length));
                                streamContent.Headers.ContentDisposition =
                                    ContentDispositionHeaderValue.Parse("form-data");
                                streamContent.Headers.ContentDisposition.Parameters.Add(new NameValueHeaderValue(
                                                                                            "name",
                                                                                            string.Format("\"{0}\"",
                                                                                                          fileParameter
                                                                                                              .Name)));
                                streamContent.Headers.ContentDisposition.Parameters.Add(
                                    new NameValueHeaderValue("filename",
                                                             string.Format("\"{0}\"", fileParameter.FileName)));

                                multipartPartFormDataContent.Add(streamContent);
                            }

                        httpRequestMessage.Content = multipartPartFormDataContent;
                    }
                    else
                    {
                        var contentString = new StringContent(restRequest.GetRequestBody(), Encoding.UTF8,
                                                              restRequest.GetContentType());
                        httpRequestMessage.Content = contentString;
                    }

                var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
				//httpResponseMessage.EnsureSuccessStatusCode();

                restResponse = await ConvertToRestResponse(restRequest, httpResponseMessage);
                restResponse.ResponseStatus = ResponseStatus.Completed;

                return restResponse;
            }
            catch (Exception exception)
            {
                restResponse.ResponseStatus = ResponseStatus.Error;
                restResponse.ErrorMessage = exception.Message;
                restResponse.ErrorException = exception;

                return restResponse;
            }
        }

        private HttpClient _httpClient;
    }
}
