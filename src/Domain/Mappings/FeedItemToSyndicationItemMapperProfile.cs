using System.ServiceModel.Syndication;
using AutoMapper;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Domain.Mappings;

public class FeedItemToSyndicationItemMapperProfile : Profile
{
    public FeedItemToSyndicationItemMapperProfile()
    {
        CreateMap<IFeedItem, SyndicationItem>()
            .ForCtorParam("title", opt => opt.MapFrom(src => src.Title))
            .ForCtorParam("content", opt => opt.MapFrom(src => src.Content))
            .ForCtorParam("itemAlternateLink", opt => opt.MapFrom(src => new Uri(src.Link.Trim('\"'))))
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("lastUpdatedTime", opt => opt.MapFrom(src => src.Timestamp.Value));
    }
}