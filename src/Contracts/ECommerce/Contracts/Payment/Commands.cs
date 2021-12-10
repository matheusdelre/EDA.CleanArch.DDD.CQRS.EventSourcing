﻿using System;
using System.Collections.Generic;
using ECommerce.Abstractions.Commands;
using ECommerce.Contracts.Common;

namespace ECommerce.Contracts.Payment;

public static class Commands
{
    public record RequestPayment(Guid OrderId, decimal AmountDue, Models.Address BillingAddress, IEnumerable<Models.IPaymentMethod> PaymentMethods) : Command;

    public record CancelPayment(Guid PaymentId, Guid OrderId) : Command;

    public record UpdatePaymentMethod(Guid PaymentId, Guid PaymentMethodId, Guid TransactionId, bool Authorized) : Command;

    public record ProceedWithPayment(Guid PaymentId, Guid OrderId) : Command;
}