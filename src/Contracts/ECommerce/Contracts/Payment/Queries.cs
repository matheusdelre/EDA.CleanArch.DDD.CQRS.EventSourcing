﻿using System;
using ECommerce.Abstractions.Queries;

namespace ECommerce.Contracts.Payment;

public static class Queries
{
    public record GetPaymentDetails(Guid PaymentId) : Query(CorrelationId: PaymentId);
}