﻿using Domain.Abstractions.Aggregates;
using Contracts.Abstractions.Messages;
using Contracts.Services.Identity;
using Domain.Enumerations;
using Domain.ValueObjects.Emails;

namespace Domain.Aggregates;

public class User : AggregateRoot<UserValidator>
{
    private readonly List<Email> _emails = new();

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Password { get; private set; }
    public string? PrimaryEmail { get; private set; }

    public IEnumerable<Email> Emails
        => _emails;

    public override void Handle(ICommand command)
        => Handle(command as dynamic);

    private void Handle(Command.RegisterUser cmd)
        => RaiseEvent(new DomainEvent.UserRegistered(cmd.UserId, cmd.FirstName, cmd.LastName, cmd.Email, cmd.Password));

    private void Handle(Command.ConfirmEmail cmd)
    {
        if (_emails.SingleOrDefault(email => email == cmd.Email) is not { IsVerified: false }) return;
        RaiseEvent(new DomainEvent.EmailConfirmed(cmd.UserId, cmd.Email));
    }

    private void Handle(Command.ChangeEmail cmd)
    {
        if (cmd.Email.Equals(PrimaryEmail, StringComparison.OrdinalIgnoreCase)) return;
        RaiseEvent(new DomainEvent.EmailChanged(cmd.UserId, cmd.Email));
    }

    private void Handle(Command.ChangePassword cmd)
    {
        if (cmd.NewPassword == Password) return;
        RaiseEvent(new DomainEvent.PasswordChanged(cmd.UserId, cmd.NewPassword));
    }

    private void Handle(Command.DeleteUser cmd)
    {
        if (IsDeleted) return;
        RaiseEvent(new DomainEvent.UserDeleted(cmd.UserId));
    }

    private void Handle(Command.ExpiryEmail cmd)
    {
        if (_emails.SingleOrDefault(email => email == cmd.Email) is not { IsVerified: true }) return;
        RaiseEvent(new DomainEvent.EmailExpired(cmd.UserId, cmd.Email));
    }

    private void Handle(Command.DefinePrimaryEmail cmd)
    {
        if (cmd.Email == PrimaryEmail) return;
        RaiseEvent(new DomainEvent.PrimaryEmailDefined(cmd.UserId, cmd.Email));
    }

    protected override void Apply(IEvent @event)
        => Apply(@event as dynamic);

    private void Apply(DomainEvent.UserRegistered @event)
    {
        (Id, FirstName, LastName, var email, Password) = @event;
        _emails.Add(email);
    }

    private void Apply(DomainEvent.PasswordChanged @event)
        => Password = @event.Password;

    private void Apply(DomainEvent.UserDeleted _)
        => IsDeleted = true;

    private void Apply(DomainEvent.EmailChanged @event)
        => _emails.Add(@event.Email);

    private void Apply(DomainEvent.EmailConfirmed @event)
    {
        var (email, index) = _emails
            .Where(email => email == @event.Email)
            .Select((email, index) => (email, index))
            .First();

        _emails[index] = email with { Status = EmailStatus.Verified };
    }

    private void Apply(DomainEvent.EmailExpired @event)
    {
        var (email, index) = _emails
            .Where(email => email == @event.Email)
            .Select((email, index) => (email, index))
            .First();

        _emails[index] = email with { Status = EmailStatus.Expired };
    }

    private void Apply(DomainEvent.PrimaryEmailDefined @event)
        => PrimaryEmail = @event.Email;
}