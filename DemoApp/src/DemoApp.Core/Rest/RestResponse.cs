using System;
using System.Collections.Generic;
using System.Net;

namespace DemoApp.Core.Rest
{
    public class RestResponseBase
    {
        public string Content { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        //public IList<RestResponseCookie> Cookies { get; protected set; }

        //TODO: use Parameter on RestClient
        public IList<Parameter> Headers { get; protected set; }

        public ResponseStatus ResponseStatus
        {
            get { return _responseStatus; }
            set { _responseStatus = value; }
        }

        public string ErrorMessage { get; set; }

        public Exception ErrorException { get; set; }

        public RestResponseBase()
        {
            Headers = new List<Parameter>();
            //Cookies = new List<RestResponseCookie>();
        }

        private ResponseStatus _responseStatus = ResponseStatus.None;
    }

    public class RestResponse : RestResponseBase
    {
    }

    public class RestResponse<T> : RestResponseBase
    {
        public T Data { get; set; }

        public static explicit operator RestResponse<T>(RestResponse response)
        {
            return new RestResponse<T>
            {
                Content = response.Content,
                ResponseStatus = response.ResponseStatus,
                ErrorMessage = response.ErrorMessage,
                ErrorException = response.ErrorException
            };
        }
    }
}
