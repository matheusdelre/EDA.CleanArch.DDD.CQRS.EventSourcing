using System;
using Application.Abstractions.EventSourcing.EventStore;
using Application.EventSourcing.EventStore.Events;
using Domain.Aggregates.Users;

namespace Application.EventSourcing.EventStore
{
    public interface IUserEventStoreRepository : IEventStoreRepository<User, UserStoreEvent, UserSnapshot, Guid> { }
}