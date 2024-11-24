using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Repositories;
using Books.Api.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Features.Books;

public class DeleteBook
{
    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("books/{id}", Handle).WithTags(EndpointTags.Books);
        }

        public static async Task<IResult> Handle(
            [FromRoute] Guid id,
            [FromServices] IBookRepository bookRepository)
        {
            var isDeleted = await bookRepository.DeleteAsync(id);

            return isDeleted ? TypedResults.NoContent() : TypedResults.NotFound();
        }
    }
}
