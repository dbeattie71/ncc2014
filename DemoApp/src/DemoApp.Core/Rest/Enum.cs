namespace DemoApp.Core.Rest
{
    public enum ParameterType
    {
        Cookie,
        GetOrPost,
        UrlSegment,
        HttpHeader,
        RequestBody,
        QueryString
    }

    public enum Method
    {
        Get,
        Post,
        Put
    }
}
