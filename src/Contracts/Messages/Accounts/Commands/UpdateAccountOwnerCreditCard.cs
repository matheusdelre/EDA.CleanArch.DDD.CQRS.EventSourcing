﻿using System;

namespace Messages.Accounts.Commands
{
    public interface UpdateAccountOwnerCreditCard
    {
        Guid AccountId { get; }
        Guid OwnerId { get; }
        Guid WalletId { get; }
        DateOnly Expiration { get; }
        string HolderName { get; }
        string Number { get; }
        string SecurityNumber { get; }
    }
}