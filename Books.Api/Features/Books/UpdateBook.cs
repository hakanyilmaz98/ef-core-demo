using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Repositories;
using Books.Api.Constants;
using Books.Api.Extensions;
using Books.Api.Features.Books.Shared.Contracts;
using Books.Api.Features.Books.Shared.Mapping;

namespace Books.Api.Features.Books;

public class UpdateBook
{
    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("books/{id}", Handle)
                .AddValidationFilter<BookRequest>()
                .WithTags(EndpointTag.Books);
        }

        public static async Task<IResult> Handle(Guid id, BookRequest request, IBookRepository bookRepository)
        {
            var book = request.MapToBook(id);

            var updatedBook = await bookRepository.UpdateAsync(book);

            return updatedBook is not null
                ? TypedResults.Ok(updatedBook.MapToResponse())
                : TypedResults.NotFound();
        }
    }
}
