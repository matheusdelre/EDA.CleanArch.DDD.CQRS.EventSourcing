﻿using System;
using System.Collections.Generic;
using Application.Abstractions.EventSourcing.Projections;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.EventSourcing.Projections;

public record ShoppingCartProjection : IProjection
{
    public IEnumerable<ShoppingCartItemProjection> Items { get; init; }
    public IEnumerable<IPaymentMethodProjection> PaymentMethods { get; init; }
    public Guid CustomerId { get; init; }
    public decimal Total { get; init; }
    public string Status { get; init; }
    public AddressProjection ShippingAddressProjection { get; init; }
    public AddressProjection BillingAddressProjection { get; init; }
    public Guid Id { get; init; }
    public bool IsDeleted { get; init; }
}

public record ShoppingCartItemsProjection : IProjection
{
    public Guid Id { get; init; }
    public IEnumerable<ShoppingCartItemProjection> Items { get; init; }
    public bool IsDeleted { get; init; }
}

public record ShoppingCartItemProjection : IProjection
{
    public Guid Id { get; init; }
    public Guid CartId { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; }
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public string PictureUrl { get; init; }
    public bool IsDeleted { get; init; }
}

public record AddressProjection
{
    public string City { get; init; }
    public string Country { get; init; }
    public int? Number { get; init; }
    public string State { get; init; }
    public string Street { get; init; }
    public string ZipCode { get; init; }
}

public interface IPaymentMethodProjection
{
    decimal Amount { get; }
}

public record CreditCardPaymentMethodProjection : IPaymentMethodProjection
{
    public decimal Amount { get; init; }
    [BsonSerializer(typeof(ExpirationDateOnlyBsonSerializer))]
    public DateOnly Expiration { get; init; }
    public string HolderName { get; init; }
    public string Number { get; init; }
    public string SecurityNumber { get; init; }
}

public record DebitCardPaymentMethodProjection : IPaymentMethodProjection
{
    public decimal Amount { get; init; }

    [BsonSerializer(typeof(ExpirationDateOnlyBsonSerializer))]
    public DateOnly Expiration { get; init; }
    public string HolderName { get; init; }
    public string Number { get; init; }
    public string SecurityNumber { get; init; }
}

public record PayPalPaymentMethodProjection : IPaymentMethodProjection
{
    public decimal Amount { get; init; }
    public string Password { get; init; }
    public string UserName { get; init; }
}