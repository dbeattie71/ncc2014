namespace DemoApp.Core.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhotoUrl { get; set; }
        public string Bio { get; set; }
        public string WebsiteUrl { get; set; }
        public bool ContactAllowed { get; set; }
        public string ShirtSize { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string GithubUrl { get; set; }
        public string GooglePlusUrl { get; set; }
        public object Sessions { get; set; }
        
        private const string BaseUrl = "http://www.nebraskacodecamp.com/content/images/speakers/";
        public string ImageUrl
        {
            get { return BaseUrl + PhotoUrl; }
        }
    }
}