using Catalog.API.Exceptions;
using Catalog.API.Models;
using SharedKernel.Abstractions.CQRS;

namespace Catalog.API.Features.Products.UpdateProduct;

public static class UpdateProductFeature {
    public record Command(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<Result>;

    public record Result(bool IsSuccess);

    internal class Handler(IDocumentSession session, ILogger<Handler> logger) : ICommandHandler<Command, Result> {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken) {
            logger.LogInformation("UpdateProductFeature.Handle called for {@Command}", command);

            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null) {
                throw new ProductNotFoundException();
            }

            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.Categories = command.Categories;
            product.ImageFile = command.ImageFile;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new Result(true);
        }
    }
}
