using Catalog.API.Models;
using SharedKernel.Abstractions.CQRS;

namespace Catalog.API.Features.Products.DeleteProduct;

public static class DeleteProductFeature {
    public record Command(Guid Id) : ICommand<Result>;

    public record Result(bool IsSuccess);

    internal class Handler(IDocumentSession session) : ICommandHandler<Command, Result> {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken) {
            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return new Result(true);
        }
    }

    public class Validator : AbstractValidator<Command> {
        public Validator() {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id must be provided.");
        }
    }
}
