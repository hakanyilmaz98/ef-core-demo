using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Repositories;
using Books.Api.Constants;
using Books.Api.Extensions;
using Books.Api.Features.Books.Shared.Contracts;
using Books.Api.Features.Books.Shared.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Features.Books;

public class UpdateBook
{
    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("books/{id}", Handle)
                .AddValidationFilter<BookRequest>()
                .WithTags(EndpointTags.Books)
                .WithOpenApi();
        }

        public static async Task<IResult> Handle(
            [FromRoute] Guid id,
            [FromBody] BookRequest request,
            [FromServices] IBookRepository bookRepository)
        {
            var existing = await bookRepository.GetByIdAsync(id);

            if (existing is null)
            {
                return TypedResults.NotFound();
            }

            if (existing.Isbn != request.Isbn)
            {
                var isbnExists = await bookRepository.ExistsAsync(request.Isbn);

                if (isbnExists)
                {
                    return TypedResults.Problem(
                        statusCode: StatusCodes.Status409Conflict,
                        detail: $"ISBN cannot be updated, because a book with the ISBN '{request.Isbn}' already exists");
                }
            }

            var book = request.MapToBook(id);

            var updatedBook = await bookRepository.UpdateAsync(book);

            return TypedResults.Ok(updatedBook.MapToResponse());
        }
    }
}
