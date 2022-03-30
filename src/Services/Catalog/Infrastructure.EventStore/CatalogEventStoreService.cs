using Application.Abstractions.Notifications;
using Application.EventSourcing.EventStore;
using Application.EventSourcing.EventStore.Events;
using Domain.Aggregates;
using Infrastructure.EventStore.Abstractions;
using Infrastructure.EventStore.DependencyInjection.Options;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Infrastructure.EventStore;

public class CatalogEventStoreService : EventStoreService<Catalog, CatalogStoreEvent, CatalogSnapshot, Guid>, ICatalogEventStoreService
{
    public CatalogEventStoreService(IPublishEndpoint publishEndpoint, ICatalogEventStoreRepository repository, INotificationContext notificationContext, IOptionsMonitor<EventStoreOptions> optionsMonitor)
        : base(publishEndpoint, repository, notificationContext, optionsMonitor) { }
}