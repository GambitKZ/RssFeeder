using RssFeeder.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RssFeeder.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedEventHandler> _logger;

    public TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("RssFeeder Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
