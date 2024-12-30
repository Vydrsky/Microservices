using Catalog.API.Exceptions;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.GetProductById;

public static class GetProductByIdEndpoint {
    //public record Request();

    public record Response(Product Product);

    public class Endpoint : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("products/{id}", async (Guid id, ISender sender) => {
                try {
                    var result = await sender.Send(new GetProductByIdFeature.Query(id));
                    return Results.Ok(result.Adapt<Response>());
                }
                catch (ProductNotFoundException ex) {
                    return Results.NotFound(ex.Message);
                }
            })
            .WithName("GetProductById")
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product by Id")
            .WithDescription("Get Product by Id");
        }
    }
}
