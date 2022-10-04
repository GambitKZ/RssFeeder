using AutoMapper;
using RssFeeder.Application.Common.Models;
using RssFeeder.Infrastructure.AzureTable.Models;

namespace RssFeeder.Infrastructure.AzureTable.Mappings;

public class TableToItemFeedMapperProfile : Profile
{
    public TableToItemFeedMapperProfile()
    {
        CreateMap<FeedItemAzureTableDto, FeedItemRepositoryDto>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RowKey));
    }
}