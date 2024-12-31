using Catalog.API.Models;

namespace Catalog.API.Features.Products.GetProductById;

public static class GetProductByIdEndpoint {
    //public record Request();

    public record Response(Product Product);

    internal class Endpoint : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("products/{id}", async (Guid id, ISender sender) => {
                var result = await sender.Send(new GetProductByIdFeature.Query(id));
                return Results.Ok(result.Adapt<Response>());
            })
            .WithName("GetProductById")
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product by Id")
            .WithDescription("Get Product by Id");
        }
    }
}
