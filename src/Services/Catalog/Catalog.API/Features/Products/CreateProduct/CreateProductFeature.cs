using Catalog.API.Models;
using SharedKernel.Abstractions.CQRS;

namespace Catalog.API.Features.Products.CreateProduct;

public static class CreateProductFeature {
    public record Command(string Name, List<string> Categories, string Description, string ImageFile, decimal Price)
        : ICommand<Result>;

    public record Result(Guid Id);

    internal class Handler(IDocumentSession session) : ICommandHandler<Command, Result> {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken) {
            var product = new Product { Name = command.Name, Categories = command.Categories, Description = command.Description, ImageFile = command.ImageFile, Price = command.Price };

            session.Store(product);

            await session.SaveChangesAsync(cancellationToken);

            return new Result(product.Id);
        }
    }
}
