using MediatR;

namespace Catalog.API.Features.Products.CreateProduct;

public static class CreateProductFeature {
    public record Command(string Name, List<string> Categories, string Description, string ImageFile, decimal Price)
        : IRequest<Result>;

    public record Result(Guid Id);

    internal class Handler : IRequestHandler<Command, Result> {
        public Task<Result> Handle(Command request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
