using Catalog.API.Models;

namespace Catalog.API.Features.Products.GetProductsByCategory;

public static class GetProductsByCategoryEndpoint {
    //public record Request();

    public record Response(IEnumerable<Product> Products);

    public class Endpoint : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) => {
                var result = await sender.Send(new GetProductsByCategoryFeature.Query(category));

                if (result.Products?.Any() != true) {
                    return Results.NotFound();
                }

                return Results.Ok(result.Adapt<Response>());
            })
            .WithName("GetProductsByCategory")
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Products by category")
            .WithDescription("Get Products by category");
        }
    };
}

