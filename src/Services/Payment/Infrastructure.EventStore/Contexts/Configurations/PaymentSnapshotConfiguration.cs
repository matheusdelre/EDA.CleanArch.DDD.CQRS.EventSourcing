﻿using Domain.StoreEvents;
using Infrastructure.EventStore.Contexts.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EventStore.Contexts.Configurations;

public class PaymentSnapshotConfiguration : IEntityTypeConfiguration<PaymentSnapshot>
{
    public void Configure(EntityTypeBuilder<PaymentSnapshot> builder)
    {
        builder.HasKey(snapshot => new {snapshot.AggregateVersion, snapshot.AggregateId});

        builder
            .Property(snapshot => snapshot.AggregateVersion)
            .IsRequired();

        builder
            .Property(snapshot => snapshot.AggregateId)
            .IsRequired();

        builder
            .Property(snapshot => snapshot.AggregateName)
            .HasMaxLength(30)
            .IsRequired();

        builder
            .Property(snapshot => snapshot.AggregateState)
            .HasConversion<PaymentConverter>()
            .IsRequired();
    }
}