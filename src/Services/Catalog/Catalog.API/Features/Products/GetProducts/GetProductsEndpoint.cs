using Catalog.API.Models;

namespace Catalog.API.Features.Products.GetProducts;

public static class GetProductsEndpoint {
    //public record Request();

    public record Response(IEnumerable<Product> Products);

    public class Endpoint : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/products", async (ISender sender) => {
                var query = new GetProductsFeature.Query();

                var result = await sender.Send(query);

                if (result.Products?.Any() != true) {
                    return Results.NotFound();
                }

                return Results.Ok(result.Adapt<Response>());
            })
            .WithName("GetProducts")
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
        }
    }
}
