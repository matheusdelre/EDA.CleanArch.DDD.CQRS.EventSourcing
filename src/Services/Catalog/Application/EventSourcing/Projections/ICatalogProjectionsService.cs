using Application.Abstractions.EventSourcing.Projections;
using ECommerce.Abstractions.Messages.Queries.Paging;

namespace Application.EventSourcing.Projections;

public interface ICatalogProjectionsService : IProjectionsService
{
    Task<IPagedResult<CatalogProjection>> GetCatalogsAsync(int limit, int offset, CancellationToken cancellationToken);
    Task<IPagedResult<CatalogItemProjection>> GetCatalogItemsAsync(Guid catalogId, int limit, int offset, CancellationToken cancellationToken);
    Task<CatalogProjection> GetCatalogDetailsAsync(Guid catalogId, CancellationToken cancellationToken);
}