using AutoMapper;
using RssFeeder.Infrastructure.AzureTable.Models;

namespace RssFeeder.Infrastructure.AzureTable.Mapper;

public class FeedMapperProfile : Profile
{
    public FeedMapperProfile()
    {
        CreateMap<FeedItemAzureTableDto, FeedItemRepositoryResponse>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RowKey))
                  .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));
    }
}