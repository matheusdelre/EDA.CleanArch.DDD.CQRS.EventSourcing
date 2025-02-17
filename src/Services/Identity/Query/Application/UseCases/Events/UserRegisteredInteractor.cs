using Application.Abstractions;
using Contracts.Services.Identity;

namespace Application.UseCases.Events;

public class UserRegisteredInteractor : IInteractor<DomainEvent.UserRegistered>
{
    private readonly IProjectionGateway<Projection.UserDetails> _gateway;

    public UserRegisteredInteractor(IProjectionGateway<Projection.UserDetails> gateway)
    {
        _gateway = gateway;
    }

    public Task InteractAsync(DomainEvent.UserRegistered @event, CancellationToken cancellationToken)
    {
        Projection.UserDetails userDetails = 
            new(@event.UserId, @event.FirstName, @event.LastName, @event.Email, @event.Password, false);

        return _gateway.InsertAsync(userDetails, cancellationToken);
    }
}