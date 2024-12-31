using Catalog.API.Models;
using SharedKernel.Abstractions.CQRS;

namespace Catalog.API.Features.Products.GetProductsByCategory;

public static class GetProductsByCategoryFeature {
    public record Query(string Category) : IQuery<Result>;

    public record Result(IEnumerable<Product> Products);

    internal class Handler(IDocumentSession session) : IQueryHandler<Query, Result> {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken) {
            var products = await session.Query<Product>().Where(p => p.Categories.Contains(query.Category)).ToListAsync();

            return new Result(products);
        }
    }
}
