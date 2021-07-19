using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Events;

namespace Infrastructure.Abstractions.EventSourcing.EventStore
{
    public abstract record StoreEvent<TAggregate, TId>
        where TAggregate : IAggregate<TId>
        where TId : struct
    {
        public int Version { get; }
        public TId AggregateId { get; init; }
        public string AggregateName { get; } = typeof(TAggregate).Name;
        public string EventName { get; init; }
        public IEvent Event { get; init; }
    }
}