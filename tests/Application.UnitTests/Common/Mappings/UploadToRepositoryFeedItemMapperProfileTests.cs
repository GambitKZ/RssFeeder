using AutoMapper;
using RssFeeder.Application.Common.Models;

namespace RssFeeder.Application.Common.Mappings.Tests;

[TestClass()]
public class UploadToRepositoryFeedItemMapperProfileTests
{
    private MapperConfiguration _configuration = null!;
    private IMapper _mapper = null!;

    [TestInitialize]
    public void Initialize()
    {
        _configuration = new MapperConfiguration(config =>
                    config.AddProfile<UploadToRepositoryFeedItemMapperProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [TestMethod("Configuration is correct")]
    public void Configuration_Configured_ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [TestMethod()]
    public void Mapping_WithCorrectData_CorrectlyMapped()
    {
        var feedItem = new UploadFeedItem
        {
            Content = "Content1",
            Link = "Link1",
            Title = "Title1"
        };

        var mappedObject = _mapper.Map<UploadFeedItem, FeedItemRepositoryDto>(feedItem);

        Assert.IsNotNull(mappedObject);
        Assert.AreEqual(feedItem.Content, mappedObject.Content);
        Assert.AreEqual(feedItem.Title, mappedObject.Title);
        Assert.AreEqual(feedItem.Link, mappedObject.Link);
        Assert.AreEqual(feedItem.Link, mappedObject.Id);
        Assert.IsNotNull(mappedObject.Timestamp);
    }
}