using RssFeeder.Application.Common.Interfaces;

namespace RssFeeder.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
