
namespace Catalog.API.Features.Products.DeleteProduct;

public static class DeleteProductEndpoint {
    public record Request(Guid Id);

    public record Response(bool IsSuccess);

    internal class Endpoint : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) => {
                var command = new DeleteProductFeature.Command(id);
                var result = await sender.Send(command);
                if (!result.IsSuccess) {
                    return Results.NotFound();
                }

                return Results.Ok(result.Adapt<Response>());
            })
            .WithName("DeleteProduct")
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete product")
            .WithDescription("Delete product");
        }
    }
}
