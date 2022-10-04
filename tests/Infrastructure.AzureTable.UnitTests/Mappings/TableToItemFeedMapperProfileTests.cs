using AutoMapper;
using RssFeeder.Application.Common.Models;
using RssFeeder.Infrastructure.AzureTable.Models;

namespace RssFeeder.Infrastructure.AzureTable.Mappings.Tests;

[TestClass]
public class TableToItemFeedMapperProfileTests
{
    private MapperConfiguration _configuration = null!;
    private IMapper _mapper = null!;

    [TestInitialize]
    public void Initialize()
    {
        _configuration = new MapperConfiguration(config =>
                    config.AddProfile<TableToItemFeedMapperProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [TestMethod("Configuration is correct")]
    public void Configuration_Configured_ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [TestMethod("Mapping is working")]
    public void Mapping_WithCorrectData_CorrectlyMapped()
    {
        var tableData = new FeedItemAzureTableDto()
        {
            Content = "Content1",
            Title = "Title1",
            Link = "https://link1.com",
            PartitionKey = "SomePartitionKey",
            RowKey = "SomeRowKey",
            Timestamp = DateTime.Now,
            ETag = new Azure.ETag(),
            Id = "SomeId"
        };

        var outcomingData = _mapper.Map<FeedItemAzureTableDto, FeedItemRepositoryDto>(tableData);

        Assert.IsNotNull(outcomingData);
        Assert.AreEqual(tableData.RowKey, outcomingData.Id);
        Assert.AreEqual(tableData.Content, outcomingData.Content);
        Assert.AreEqual(tableData.Title, outcomingData.Title);
        Assert.AreEqual(tableData.Link, outcomingData.Link);
        Assert.AreEqual(tableData.Timestamp, outcomingData.Timestamp);
        Assert.AreNotEqual(tableData.Id, outcomingData.Id);
    }
}