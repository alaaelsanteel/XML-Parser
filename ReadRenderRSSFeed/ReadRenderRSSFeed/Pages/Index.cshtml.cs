using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using System.Threading.Tasks;

namespace ReadRenderRSSFeed.Pages
{
    public class IndexModel : PageModel
    {
        public List<RSSData> RssList { get; set; } = new();

        public async Task OnGet()
        {
            RssList = new List<RSSData>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://scripting.com/rss.xml");
                response.EnsureSuccessStatusCode();

                string xmlContent = await response.Content.ReadAsStringAsync();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                foreach (XmlNode node in xmlDoc.SelectNodes("rss/channel/item").Cast<XmlNode>())
                {
                    RSSData rssDataObj = new RSSData
                    {
                        title = node.SelectSingleNode("title")?.InnerText, // == null) ? "" : node.SelectSingleNode("title").InnerText,
                        description = node.SelectSingleNode("description")?.InnerText,
                        pubDate = node.SelectSingleNode("pubDate")?.InnerText,
                        link = node.SelectSingleNode("link")?.InnerText,
                    };

                    RssList.Add(rssDataObj);
                }
            }
        }
    }

    public class RSSData
    {
        public string? title { get; set; }
        public string? pubDate { get; set; }
        public String? description { get; set; }
        public string? link { get; set; }

    }
}