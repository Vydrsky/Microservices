namespace Catalog.API.Features.Products.UpdateProduct;

public static class UpdateProductEndpoint {
    public record Request(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price);

    public record Response(bool IsSuccess);

    internal class Endpoint : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapPut("/products", async (Request request, ISender sender) => {
                var command = request.Adapt<UpdateProductFeature.Command>();
                var result = await sender.Send(command);
                return Results.Ok(result.Adapt<Response>());
            })
            .WithName("UpdateProduct")
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update product")
            .WithDescription("Update product");
        }
    };
}

