﻿using Contracts.Abstractions.Messages;
using Contracts.JsonConverters;
using JsonNet.ContractResolvers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Infrastructure.EventStore.Contexts.Converters;

public class EventConverter : ValueConverter<IEvent?, string>
{
    public EventConverter()
        : base(
            @event => JsonConvert.SerializeObject(@event, typeof(IEvent), SerializerSettings()),
            jsonString => JsonConvert.DeserializeObject<IEvent>(jsonString, DeserializerSettings())) { }

    private static JsonSerializerSettings SerializerSettings()
    {
        JsonSerializerSettings jsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        jsonSerializerSettings.Converters.Add(new DateOnlyJsonConverter());
        jsonSerializerSettings.Converters.Add(new ExpirationDateOnlyJsonConverter());

        return jsonSerializerSettings;
    }

    private static JsonSerializerSettings DeserializerSettings()
    {
        JsonSerializerSettings jsonDeserializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new PrivateSetterContractResolver()
        };

        jsonDeserializerSettings.Converters.Add(new DateOnlyJsonConverter());
        jsonDeserializerSettings.Converters.Add(new ExpirationDateOnlyJsonConverter());

        return jsonDeserializerSettings;
    }
}