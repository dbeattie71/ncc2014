using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DemoApp.Core.Rest
{
    public class RestRequest
    {
        public ContentTypes ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Resource { internal get; set; }

        public Method Method { get; set; }

        public Dictionary<string, object> Headers { get; set; }

        public RestRequest()
        {
            UrlSegments = new List<UrlSegment>();
            Parameters = new List<EncodedParameter>();
            Headers = new Dictionary<string, object>();
            Method = Method.Get;
            Files = new List<FileParameter>();
        }

        public RestRequest(string resource)
            : this()
        {
            Resource = resource;
        }

        public RestRequest(string resource, Method method)
            : this()
        {
            Resource = resource;
            Method = method;
        }

        public void AddHeader(string key, object value)
        {
            Headers.Add(key, value);
        }

        public void AddParameter(object value)
        {
            AddParameter("", value);
        }

        public void AddParameter(string key, object value, ParameterEncoding encoding = ParameterEncoding.UriEncoded)
        {
            Parameters.Add(new EncodedParameter(key, value, encoding));
        }

        public void AddUrlSegment(string key, string value)
        {
            UrlSegments.Add(new UrlSegment(key, value));
        }

        public void AddQueryString(string key, string value)
        {
            UrlSegments.Add(new UrlSegment(key, value, true));
        }

        public void AddQueryString(string key, object value)
        {
            AddQueryString(key, value.ToString());
        }

        public void AddFile(string name, byte[] bytes, string fileName)
        {
            Files.Add(new FileParameter(name, bytes, fileName));
        }

        private List<UrlSegment> UrlSegments { get; set; }

        internal List<FileParameter> Files { get; set; }

        internal List<EncodedParameter> Parameters { get; set; }

        internal Uri GetResourceUri(string baseUrl)
        {
            foreach (var segment in UrlSegments.Where(c => !c.IsQueryString))
                Resource = Resource.Replace("{" + segment.Key + "}", Uri.EscapeUriString(segment.Value));

            if (UrlSegments.Any(c => c.IsQueryString))
            {
                var queryString = UrlSegments.Where(c => c.IsQueryString)
                    .Aggregate(new StringBuilder(),
                               (current, next) =>
                                   current.Append(string.Format("&{0}={1}", Uri.EscapeUriString(next.Key),
                                                                Uri.EscapeDataString(next.Value))))
                    .ToString();

                Resource = string.Format(Resource.Contains("?") ? "{0}{1}" : "{0}?{1}", Resource, queryString);
            }

            if (!string.IsNullOrEmpty(Resource) && Resource.StartsWith("/"))
                Resource = Resource.Substring(1);

            if (!string.IsNullOrEmpty(baseUrl))
                Resource = string.IsNullOrEmpty(Resource) ? baseUrl : string.Format("{0}/{1}", baseUrl, Resource);
            return new Uri(Resource, UriKind.RelativeOrAbsolute);
        }

        internal string GetRequestBody()
        {
            switch (ContentType)
            {
                case ContentTypes.FormUrlEncoded:
                    var parameters = Parameters.Aggregate(new StringBuilder(),
                                                          (current, next) =>
                                                              current.Append(string.Format("{0}{1}={2}",
                                                                                           current.Length > 0 ? "&" : "",
                                                                                           Uri.EscapeDataString(next.Key),
                                                                                           next.GetEncodedValue())));
                    return parameters.ToString();

                default:
                    return Parameters.Count > 0 ? JsonConvert.SerializeObject(Parameters[0].Value) : "";
            }
        }

        internal string GetContentType()
        {
            switch (ContentType)
            {
                case ContentTypes.FormUrlEncoded:
                    return "application/x-www-form-urlencoded";
                case ContentTypes.Json:
                    return "application/json";
                default:
                    return "multipart/form-data";
            }
        }
    }
}
