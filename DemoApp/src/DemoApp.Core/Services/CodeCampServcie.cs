using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Core.Common;
using DemoApp.Core.Models;
using DemoApp.Core.Rest;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace DemoApp.Core.Services
{
    public interface ICodeCampService
    {
        Task<List<Session>> GetSessions();
        Speaker GetSpeaker(int id);
    }

    public class CodeCampService : RestClient, ICodeCampService
    {
        public CodeCampService()
        {
            SetUserAgent<CodeCampService>("DemoApp Client");
            BaseUrl = "http://www.nebraskacodecamp.com";
        }

        //public async Task<SessionsSpeakers> GetSessions()
        //{
        //    var restRequest = new RestRequest("Sessions/json") {ContentType = ContentTypes.Json};

        //    var restResponse = await ExecuteAsync<SessionsSpeakers>(restRequest);
        //    return restResponse.Data;
        //}

        //parse html to get json since Adam botched the json feed :o
        public async Task<List<Session>> GetSessions()
        {
			var restRequest = new RestRequest("Sessions") {ContentType = ContentTypes.Json};
            var restResponse = await ExecuteAsync(restRequest);

            var htmlDocument = new HtmlDocument {OptionFixNestedTags = true};
            htmlDocument.LoadHtml(restResponse.Content);
            var htmlNode =
                htmlDocument.DocumentNode.Descendants("script")
                    .FirstOrDefault(t => t.InnerText.Contains("var allSessions ="));
            if (htmlNode == null) return new List<Session>();

            var innerText = htmlNode.InnerText.Trim();
            var tempJson = innerText.TrimStart("var allSessions =");
            var pos = tempJson.IndexOf("}];", StringComparison.Ordinal);
            var json = tempJson.Substring(0, tempJson.Length - (tempJson.Length - pos) + 2);
            var sessionsSpeakers = JsonConvert.DeserializeObject<List<Session>>(json).ToList();
            _speakers = sessionsSpeakers.Select(s => s.Speaker).ToList();

            return sessionsSpeakers;
        }

        public Speaker GetSpeaker(int id)
        {
            return _speakers.FirstOrDefault(s => s.Id == id);
        }

        private List<Speaker> _speakers;
    }
}
