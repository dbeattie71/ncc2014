namespace DemoApp.Core.Rest
{
    internal class UrlSegment
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsQueryString { get; set; }

        public UrlSegment(string key, string value)
        {
            Key = key;
            Value = value;
            IsQueryString = false;
        }

        public UrlSegment(string key, string value, bool isQueryString)
        {
            Key = key;
            Value = value;
            IsQueryString = isQueryString;
        }
    }
}
