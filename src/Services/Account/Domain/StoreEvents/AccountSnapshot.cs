﻿using Domain.Abstractions.StoreEvents;
using Domain.Aggregates;

namespace Domain.StoreEvents;

public record AccountSnapshot :  Snapshot<Account, Guid>;
