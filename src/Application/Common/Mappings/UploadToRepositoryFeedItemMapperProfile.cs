using AutoMapper;
using RssFeeder.Application.Common.Models;

namespace RssFeeder.Application.Common.Mappings;

public class UploadToRepositoryFeedItemMapperProfile : Profile
{
    public UploadToRepositoryFeedItemMapperProfile()
    {
        CreateMap<UploadFeedItem, FeedItemRepositoryDto>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Link))
                  .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(_ => DateTime.Now));
    }
}