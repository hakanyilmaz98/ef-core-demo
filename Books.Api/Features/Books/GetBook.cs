using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Repositories;
using Books.Api.Constants;
using Books.Api.Features.Books.Shared.Mapping;

namespace Books.Api.Features.Books;

public class GetBook
{
    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("books/{id}", Handle)
                .WithTags(EndpointTags.Books)
                .WithOpenApi();
        }

        public static async Task<IResult> Handle(Guid id, IBookRepository bookRepository)
        {
            var book = await bookRepository.GetByIdAsync(id);

            return book is not null
                ? TypedResults.Ok(book.MapToResponse())
                : TypedResults.NotFound();
        }
    }
}
