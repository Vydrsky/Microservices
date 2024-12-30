using Catalog.API.Exceptions;
using Catalog.API.Models;
using SharedKernel.Abstractions.CQRS;

namespace Catalog.API.Features.Products.GetProductById;

public static class GetProductByIdFeature {
    public record Query(Guid Id) : IQuery<Result>;

    public record Result(Product Product);

    internal class Handler(IDocumentSession session) : IQueryHandler<Query, Result> {
        public async Task<Result> Handle(Query query, CancellationToken cancellationToken) {
            var product = await session.LoadAsync<Product>(query.Id);
            if (product == null) {
                throw new ProductNotFoundException();
            }
            return new Result(product);
        }
    }
}
