﻿using Application.EventSourcing.Projections;
using ECommerce.Abstractions.Messages.Queries.Responses;
using ECommerce.Contracts.ShoppingCart;
using MassTransit;
using GetShoppingCartItemsQuery = ECommerce.Contracts.ShoppingCart.Queries.GetShoppingCartItems;

namespace Application.UseCases.Queries;

public class GetShoppingCartItemsConsumer : IConsumer<GetShoppingCartItemsQuery>
{
    private readonly IShoppingCartProjectionsService _projectionsService;

    public GetShoppingCartItemsConsumer(IShoppingCartProjectionsService projectionsService)
    {
        _projectionsService = projectionsService;
    }

    public async Task Consume(ConsumeContext<GetShoppingCartItemsQuery> context)
    {
        var shoppingCartItemsProjection = await _projectionsService
            .GetShoppingCartItemsAsync(context.Message.CartId, context.Message.Limit, context.Message.Offset, context.CancellationToken);

        await (shoppingCartItemsProjection is null
            ? context.RespondAsync<NotFound>(new())
            : context.RespondAsync<Responses.ShoppingCartItems>(shoppingCartItemsProjection));
    }
}