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

    public class Validator : AbstractValidator<Command> {
        public Validator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories are required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
