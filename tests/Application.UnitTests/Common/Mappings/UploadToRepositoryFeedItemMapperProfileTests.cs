using AutoMapper;

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
    public void UploadToRepositoryFeedItemMapperProfileTest()
    {
        throw new NotImplementedException();
    }
}