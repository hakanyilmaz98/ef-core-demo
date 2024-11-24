using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Repositories;
using Books.Api.Constants;
using Books.Api.Features.Books.Shared.Contracts;

namespace Books.Api.Features.Books;

public class GetBooks
{
    public record Response(IEnumerable<BookResponse> Books);

    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("books", Handle)
                .WithTags(EndpointTag.Books)
                .WithOpenApi();
        }

        public static async Task<IResult> Handle(IBookRepository bookRepository)
        {
            var books = await bookRepository.GetAsync();

            var mapped = books.Select(book =>
                new BookResponse(book.Id, book.Title, book.Isbn, book.Author));

            return TypedResults.Ok(new Response(mapped));
        }
    }
}
