﻿using Asp.Versioning.Builder;
using Contracts.Services.Catalog;
using MassTransit;
using WebAPI.Abstractions;
using WebAPI.Validations;

namespace WebAPI.APIs.Catalogs;

public static class CatalogApi
{
    private const string BaseUrl = "/api/v{version:apiVersion}/catalogs/";

    public static IVersionedEndpointRouteBuilder MapCatalogApiV1(this IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(BaseUrl).HasApiVersion(1);

        group.MapGet("/", (IBus bus, ushort? limit, ushort? offset, CancellationToken cancellationToken)
            => ApplicationApi.GetPagedProjectionAsync<Query.GetCatalogs, Projection.Catalog>(bus, new(limit ?? 0, offset ?? 0), cancellationToken));

        group.MapPost("/", ([AsParameters] Requests.CreateCatalog request)
            => ApplicationApi.SendCommandAsync<Command.CreateCatalog>(request));

        group.MapGet("/{catalogId:guid}", (IBus bus, [NotEmpty] Guid catalogId, CancellationToken cancellationToken)
            => ApplicationApi.GetProjectionAsync<Query.GetCatalog, Projection.Catalog>(bus, new(catalogId), cancellationToken));

        group.MapDelete("/{catalogId:guid}", ([AsParameters] Requests.DeleteCatalog request)
            => ApplicationApi.SendCommandAsync<Command.DeleteCatalog>(request));

        group.MapPut("/{catalogId:guid}/activate", ([AsParameters] Requests.ActivateCatalog request)
            => ApplicationApi.SendCommandAsync<Command.ActivateCatalog>(request));

        group.MapPut("/{catalogId:guid}/deactivate", ([AsParameters] Requests.DeactivateCatalog request)
            => ApplicationApi.SendCommandAsync<Command.DeactivateCatalog>(request));

        group.MapPut("/{catalogId:guid}/description", ([AsParameters] Requests.ChangeCatalogDescription request)
            => ApplicationApi.SendCommandAsync<Command.ChangeCatalogDescription>(request));

        group.MapPut("/{catalogId:guid}/title", ([AsParameters] Requests.ChangeCatalogTitle request)
            => ApplicationApi.SendCommandAsync<Command.ChangeCatalogTitle>(request));

        group.MapGet("/items", (IBus bus, ushort? limit, ushort? offset, CancellationToken cancellationToken)
            => ApplicationApi.GetPagedProjectionAsync<Query.GetAllItems, Projection.CatalogItem>(bus, new(limit ?? 0, offset ?? 0), cancellationToken));

        group.MapGet("/{catalogId:guid}/items", (IBus bus, Guid catalogId, ushort? limit, ushort? offset, CancellationToken cancellationToken)
            => ApplicationApi.GetProjectionAsync<Query.GetCatalogItems, Projection.CatalogItem>(bus, new(catalogId, limit ?? 0, offset ?? 0), cancellationToken));

        group.MapPost("/{catalogId:guid}/items", ([AsParameters] Requests.AddCatalogItem request)
            => ApplicationApi.SendCommandAsync<Command.AddCatalogItem>(request));

        group.MapDelete("/{catalogId:guid}/items/{itemId:guid}", ([AsParameters] Requests.RemoveCatalogItem request)
            => ApplicationApi.SendCommandAsync<Command.RemoveCatalogItem>(request));

        return builder;
    }

    public static IVersionedEndpointRouteBuilder MapCatalogApiV2(this IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(BaseUrl).HasApiVersion(2);

        group.MapGet("/", (IBus bus, ushort? limit, ushort? offset, CancellationToken cancellationToken)
            => ApplicationApi.GetPagedProjectionAsync<Query.GetCatalogs, Projection.Catalog>(bus, new(limit ?? 0, offset ?? 0), cancellationToken));

        return builder;
    }
}