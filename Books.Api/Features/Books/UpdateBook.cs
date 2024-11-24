using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Repositories;
using Books.Api.Constants;
using Books.Api.Entities;
using Books.Api.Extensions;
using Books.Api.Features.Books.Shared.Contracts;

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
            var book = new Book
            {
                Title = request.Title,
                Isbn = request.Isbn
            };

            var updatedBook = await bookRepository.UpdateAsync(id, book);

            return updatedBook is not null
                ? TypedResults.Ok(new BookResponse(updatedBook.Id, updatedBook.Title, updatedBook.Isbn))
                : TypedResults.NotFound();
        }
    }
}
