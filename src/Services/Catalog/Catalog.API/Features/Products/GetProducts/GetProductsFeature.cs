using Catalog.API.Models;
using SharedKernel.Abstractions.CQRS;

namespace Catalog.API.Features.Products.GetProducts;

public static class GetProductsFeature {
    public record Query : IQuery<Result>;

    public record Result(IEnumerable<Product> Products);

    internal class Handler(IDocumentSession session) : IQueryHandler<Query, Result> {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken) {
            var products = await session.Query<Product>().ToListAsync(cancellationToken);

            return new Result(products);
        }
    }
}
