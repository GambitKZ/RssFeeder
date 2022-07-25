using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MVP;

public static class GetHardCodedRss
{
    [FunctionName("GetHardCodedRss")]
    [Produces("application/xml")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true
        };

        Rss20FeedFormatter feed = new Rss20FeedFormatter(GetFeed(), false);

        // Stream Builder
        var sb = new StringBuilder();
        XmlWriter rssWriter = XmlWriter.Create(sb, settings);
        feed.WriteTo(rssWriter);
        rssWriter.Close();

        // Issue of Function https://github.com/Azure/azure-functions-host/issues/2896
        return new ContentResult { Content = sb.ToString(), ContentType = "application/xml" };
    }

    private static SyndicationFeed GetFeed()
    {
        // Put a html of the function
        SyndicationFeed feed = new SyndicationFeed("Gambit's Personal RSS",
                "This is my test feed", new Uri("http://SomeURI"));
        feed.Authors.Add(new SyndicationPerson("rusnigdrag@gmail.com"));
        feed.Categories.Add(new SyndicationCategory("Mentoring URLs"));
        feed.Description = new TextSyndicationContent("Hardcoded example of RSS");

        SyndicationItem item1 = new SyndicationItem(
            "Security link #1",
            "This is the content for item one",
            new Uri("https://docs.microsoft.com/en-us/azure/active-directory/develop/custom-rbac-for-developers"),
            "5EE9C738-40B6-42BB-8266-A4977C703BE0",
            DateTime.Now);

        SyndicationItem item2 = new SyndicationItem(
            "Security link #2",
            "This is the content for item two",
            new Uri("https://en.wikipedia.org/wiki/Identity_management"),
            "56312A8F-55B3-42DE-81FA-A10F5B293710",
            DateTime.Now);

        SyndicationItem item3 = new SyndicationItem(
            "Security link #3",
            "This is the content for item three",
            new Uri("https://curity.io/resources/learn/iam-primer"),
            "7C908FED-2D36-4290-ADC0-340F91BF3040",
            DateTime.Now);

        SyndicationItem item4 = new SyndicationItem(
            "Security link #4",
            "This is the content for item three",
            new Uri("https://docs.microsoft.com/en-us/azure/active-directory/develop/security-tokens"),
            "7C908FED-2D36-4290-ADC0-340F91BF3040",
            DateTime.Now);

        SyndicationItem item5 = new SyndicationItem(
            "Security link #5",
            "This is the content for item three",
            new Uri("https://auth0.com/blog/id-token-access-token-what-is-the-difference/"),
            "31D2269E-DC0A-4454-AD03-03EAF29EADB5",
            DateTime.Now);

        SyndicationItem item6 = new SyndicationItem(
            "Security link #6",
            "This is the content for item three",
            new Uri("https://docs.microsoft.com/en-us/azure/active-directory/develop/access-tokens"),
            "5C06B4B3-8004-4AF6-8718-B9A7D71CBDE7",
            DateTime.Now);

        SyndicationItem item7 = new SyndicationItem(
            "Security link #7",
            "This is the content for item three",
            new Uri("https://docs.microsoft.com/en-us/azure/active-directory/develop/id-tokens"),
            "23CD1B93-9D01-4662-ACFF-475DC319EEBC",
            DateTime.Now);

        SyndicationItem item8 = new SyndicationItem(
            "Security link #8",
            "This is the content for item three",
            new Uri("https://docs.microsoft.com/en-us/azure/active-directory/develop/refresh-tokens"),
            "4D728174-5003-44E7-AC00-A8917F4E15C4",
            DateTime.Now);

        SyndicationItem item9 = new SyndicationItem(
            "Security link #9",
            "This is the content for item three",
            new Uri("https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-configurable-token-lifetimes"),
            "7A6A49BD-3578-4407-ADAB-1E861E158BA6",
            DateTime.Now);

        List<SyndicationItem> items = new()
        {
            item1,
            item2,
            item3,
            item4,
            item5,
            item6,
            item7,
            item8,
            item9
        };

        feed.Items = items;

        feed.Language = "en-us";
        feed.LastUpdatedTime = DateTime.Now;

        return feed;
    }
}