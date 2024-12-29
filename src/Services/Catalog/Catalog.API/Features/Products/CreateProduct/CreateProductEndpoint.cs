namespace Catalog.API.Features.Products.CreateProduct;

public static class CreateProductEndpoint {
    public record Request(string Name, List<string> Categories, string Description, string ImageFile, decimal Price);

    public record Response(Guid Id);

    public class Endpoint : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapPost("/products", async (Request request, ISender sender) => {
                var command = request.Adapt<CreateProductFeature.Command>();

                var result = await sender.Send(command);

                return Results.Created($"/products/{result.Id}", result.Adapt<Response>());
            })
            .WithName("CreateProduct")
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("CreateProduct")
            .WithDescription("Create Product");
        }
    }
}
