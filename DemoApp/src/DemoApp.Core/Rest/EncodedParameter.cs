using System;
using System.IO;

namespace DemoApp.Core.Rest
{
    internal class EncodedParameter
    {
        internal string Key { get; set; }
        internal object Value { get; set; }
        internal ParameterEncoding Encoding { get; set; }

        internal EncodedParameter(string key, object value)
        {
            Key = key;
            Value = value;
            Encoding = ParameterEncoding.UriEncoded;
        }

        internal EncodedParameter(string key, object value, ParameterEncoding encoding) : this(key, value)
        {
            Encoding = encoding;
        }

        internal object GetEncodedValue()
        {
            //RWM: This will not work. Need to store encoding with parameter and deal with on output, not input.
            object finalValue = null;

            switch (Encoding)
            {
                case ParameterEncoding.Base64:
                    if (!(Value is Stream))
                        throw new ArgumentException("ByteArray encoded objects must be passed in as a stream.");
                    var bytes1 = (Value as Stream).ToArray();
                    finalValue = Convert.ToBase64String(bytes1);
                    break;
                case ParameterEncoding.ByteArray:
                    if (!(Value is Stream))
                        throw new ArgumentException("ByteArray encoded objects must be passed in as a stream.");
                    finalValue = (Value as Stream).ToArray();
                    break;
                case ParameterEncoding.Unencoded:
                    finalValue = Value.ToString();
                    break;
                case ParameterEncoding.UriEncoded:
                    finalValue = Uri.EscapeDataString(Value.ToString());
                    break;
            }
            return finalValue;
        }
    }
}
