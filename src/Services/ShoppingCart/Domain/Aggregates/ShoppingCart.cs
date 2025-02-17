﻿using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;
using Contracts.Services.ShoppingCart;
using Domain.Abstractions.Aggregates;
using Domain.Entities.CartItems;
using Domain.Entities.PaymentMethods;
using Domain.Enumerations;
using Domain.ValueObjects.Addresses;
using Domain.ValueObjects.PaymentOptions.CreditCards;
using Domain.ValueObjects.PaymentOptions.DebitCards;
using Domain.ValueObjects.PaymentOptions.PayPals;

namespace Domain.Aggregates;

public class ShoppingCart : AggregateRoot<Guid, ShoppingCartValidator>
{
    private readonly List<CartItem> _items = new();
    private readonly List<PaymentMethod> _paymentMethods = new();

    public Guid CustomerId { get; private set; }
    public CartStatus Status { get; private set; } = CartStatus.Active;
    public Address BillingAddress { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    private bool ShippingAndBillingAddressesAreSame { get; set; } = true;

    public decimal Total
        => Items.Sum(item => item.UnitPrice * item.Quantity);

    public decimal TotalPayment
        => PaymentMethods.Sum(method => method.Amount);

    public decimal AmountDue
        => Total - TotalPayment;

    public IEnumerable<CartItem> Items
        => _items;

    public IEnumerable<PaymentMethod> PaymentMethods
        => _paymentMethods;

    public override void Handle(ICommand? command)
        => Handle(command as dynamic);

    public void Handle(Command.CreateCart cmd)
        => RaiseEvent(new DomainEvent.CartCreated(cmd.CartId, cmd.CustomerId, CartStatus.Active));

    public void Handle(Command.AddCartItem cmd)
        => RaiseEvent(_items.SingleOrDefault(cartItem => cartItem.Product == cmd.Product) is { IsDeleted: false } item
            ? new DomainEvent.CartItemIncreased(Id, item.Id, (ushort)(item.Quantity + cmd.Quantity), item.UnitPrice)
            : new DomainEvent.CartItemAdded(cmd.CartId, Guid.NewGuid(), cmd.InventoryId, cmd.Product, cmd.Quantity, cmd.UnitPrice));

    public void Handle(Command.ChangeCartItemQuantity cmd)
    {
        if (_items.SingleOrDefault(cartItem => cartItem.Id == cmd.ItemId) is not { IsDeleted: false } item) return;

        if (cmd.NewQuantity > item.Quantity)
            RaiseEvent(new DomainEvent.CartItemIncreased(Id, item.Id, cmd.NewQuantity, item.UnitPrice));

        else if (cmd.NewQuantity < item.Quantity)
            RaiseEvent(new DomainEvent.CartItemDecreased(Id, item.Id, cmd.NewQuantity, item.UnitPrice));
    }

    public void Handle(Command.RemoveCartItem cmd)
    {
        if (_items.SingleOrDefault(cartItem => cartItem.Id == cmd.ItemId) is not { IsDeleted: false } item) return;
        RaiseEvent(new DomainEvent.CartItemRemoved(cmd.CartId, cmd.ItemId, item.UnitPrice, item.Quantity));
    }

    public void Handle(Command.AddPaymentMethod cmd)
    {
        // TODO - Should cmd.Amount be subtracted from AmountDue?
        if (cmd.Amount > AmountDue) return;
        RaiseEvent(new DomainEvent.PaymentMethodAdded(cmd.CartId, Guid.NewGuid(), cmd.Amount, cmd.Option));
    }

    public void Handle(Command.AddShippingAddress cmd)
    {
        if (ShippingAddress == cmd.Address) return;
        RaiseEvent(new DomainEvent.ShippingAddressAdded(cmd.CartId, cmd.Address));
    }

    public void Handle(Command.AddBillingAddress cmd)
    {
        if (BillingAddress == cmd.Address) return;
        RaiseEvent(new DomainEvent.BillingAddressAdded(cmd.CartId, cmd.Address));
    }

    public void Handle(Command.CheckOutCart cmd)
    {
        if (_items is { Count: 0 } || AmountDue > 0) return;
        RaiseEvent(new DomainEvent.CartCheckedOut(cmd.CartId));
    }

    public void Handle(Command.DiscardCart cmd)
        => RaiseEvent(new DomainEvent.CartDiscarded(cmd.CartId));

    protected override void ApplyEvent(IEvent @event)
        => When(@event as dynamic);

    private void When(DomainEvent.CartCreated @event)
        => (Id, CustomerId, Status) = @event;

    private void When(DomainEvent.CartCheckedOut _)
        => Status = CartStatus.CheckedOut;

    private void When(DomainEvent.CartDiscarded _)
    {
        Status = CartStatus.Abandoned;
        IsDeleted = true;
    }

    private void When(DomainEvent.CartItemIncreased @event)
        => _items
            .First(item => item.Id == @event.ItemId)
            .SetQuantity(@event.NewQuantity);

    private void When(DomainEvent.CartItemDecreased @event)
        => _items
            .First(item => item.Id == @event.ItemId)
            .SetQuantity(@event.NewQuantity);

    private void When(DomainEvent.CartItemRemoved @event)
        => _items.First(item => item.Id == @event.ItemId).Delete();

    private void When(DomainEvent.CartItemAdded @event)
        => _items.Add(new(@event.ItemId, @event.Product, @event.Quantity, @event.UnitPrice));

    private void When(DomainEvent.PaymentMethodAdded @event)
        => _paymentMethods.Add(new(@event.MethodId, @event.Amount, @event.Option switch
        {
            Dto.CreditCard creditCard => (CreditCard)creditCard,
            Dto.DebitCard debitCard => (DebitCard)debitCard,
            Dto.PayPal payPal => (PayPal)payPal,
            _ => default
        }));

    private void When(DomainEvent.BillingAddressAdded @event)
    {
        ShippingAddress = @event.Address;

        if (ShippingAndBillingAddressesAreSame)
            BillingAddress = ShippingAddress;
    }

    private void When(DomainEvent.ShippingAddressAdded @event)
    {
        BillingAddress = @event.Address;
        ShippingAndBillingAddressesAreSame = false;
    }
}