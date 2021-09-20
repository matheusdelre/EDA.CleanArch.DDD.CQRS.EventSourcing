﻿namespace Domain.Abstractions.Entities
{
    public interface IEntity<out TId>
        where TId : struct
    {
        TId Id { get; }
        bool IsDeleted { get; }
    }
}