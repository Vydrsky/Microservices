using Catalog.API.Models;
using SharedKernel.Abstractions.CQRS;

namespace Catalog.API.Features.Products.CreateProduct;

public static class CreateProductFeature {
    public record Command(string Name, List<string> Categories, string Description, string ImageFile, decimal Price)
        : ICommand<Result>;

    public record Result(Guid Id);

    internal class Handler : ICommandHandler<Command, Result> {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken) {
            //create product entity from command object
            var product = new Product { Name = command.Name, Categories = command.Categories, Description = command.Description, ImageFile = command.ImageFile, Price = command.Price };
            //add to context

            //save database

            //return result
            return new Result(Guid.NewGuid());
        }
    }
}
