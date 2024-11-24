using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Repositories;
using Books.Api.Constants;
using Books.Api.Extensions;
using Books.Api.Features.Books.Shared.Contracts;
using Books.Api.Features.Books.Shared.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Features.Books;

public static class CreateBook
{
    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/books", Handle)
                .AddValidationFilter<BookRequest>()
                .WithTags(EndpointTags.Books)
                .WithOpenApi();
        }

        public static async Task<IResult> Handle(
            [FromBody] BookRequest request,
            [FromServices] IBookRepository bookRepository)
        {
            var isNotUnqique = await bookRepository.ExistsAsync(request.Isbn);

            if (isNotUnqique)
            {
                return TypedResults.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    detail: $"ISBN {request.Isbn} must be unique");
            }

            var book = request.MapToBook();

            await bookRepository.AddAsync(book);

            var response = book.MapToResponse();

            return TypedResults.Created($"books/{book.Id}", response);
        }
    }
}
