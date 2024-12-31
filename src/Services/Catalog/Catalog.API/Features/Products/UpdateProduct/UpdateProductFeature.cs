using Catalog.API.Exceptions;
using Catalog.API.Models;
using SharedKernel.Abstractions.CQRS;

namespace Catalog.API.Features.Products.UpdateProduct;

public static class UpdateProductFeature {
    public record Command(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<Result>;

    public record Result(bool IsSuccess);

    internal class Handler(IDocumentSession session) : ICommandHandler<Command, Result> {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken) {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null) {
                throw new ProductNotFoundException(command.Id);
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

    public class Validator : AbstractValidator<Command> {
        public Validator() {
            RuleFor(x => x.Name).NotEmpty().Length(2, 150).WithMessage("Name must be between 2 and 150 characters.");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories are required.");
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
