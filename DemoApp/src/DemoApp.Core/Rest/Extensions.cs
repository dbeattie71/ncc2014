using System;
using System.IO;
using Newtonsoft.Json;

namespace DemoApp.Core.Rest
{
    public static class Extensions
    {
        public static RestResponse<T> ToRestResponse<T>(this RestResponse response)
        {
            return new RestResponse<T>
            {
                Content = response.Content,
                StatusCode = response.StatusCode,
                ResponseStatus = response.ResponseStatus,
                ErrorMessage = response.ErrorMessage,
                ErrorException = response.ErrorException
            };
        }

        public static string ToJson<T>(this T @this)
        {
            return JsonConvert.SerializeObject(@this);
        }

        public static T FromJson<T>(this string @this)
        {
            return String.IsNullOrEmpty(@this) ? default(T) : JsonConvert.DeserializeObject<T>(@this);
        }

        public static byte[] ToArray(this Stream input)
        {
            var buffer = new byte[16*1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                return ms.ToArray();
            }
        }
    }
}
