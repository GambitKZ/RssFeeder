﻿using System.ServiceModel.Syndication;
using AutoMapper;
using RssFeeder.Domain.Interfaces;

namespace RssFeeder.Domain.Mappings;

internal class FeedItemToSyndicationItemMapperProfile : Profile
{
    public FeedItemToSyndicationItemMapperProfile()
    {
        CreateMap<IFeedItem, SyndicationItem>()
            .ForCtorParam("title", opt => opt.MapFrom(src => src.Title))
            .ForCtorParam("content", opt => opt.MapFrom(src => src.Content))
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("itemAlternateLink", opt => opt.MapFrom(src => new Uri(src.Link!.Trim('\"'))))
            .ForCtorParam("lastUpdatedTime", opt => opt.MapFrom(src => src.Timestamp!));
    }
}