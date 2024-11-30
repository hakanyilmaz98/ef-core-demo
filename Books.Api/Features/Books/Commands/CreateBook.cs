using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Messaging;
using Books.Api.Abstractions.Services;
using Books.Api.Constants;
using Books.Api.Entities;
using Books.Api.Extensions;
using Books.Api.Features.Books.Shared.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Features.Books.Commands;

public static class CreateBook
{
    public record Command(string Title, string Isbn) : IEndpointRequest;

    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/books", async ([FromBody] BookRequest request, [FromServices] ISender sender) =>
            {
                var command = new Command(request.Title, request.Isbn);

                return await sender.Send(command);
            })
                .AddValidationFilter<BookRequest>()
                .WithTags(EndpointTags.Books)
                .WithOpenApi();
        }
    }

    public class Handler(IBookRepository bookRepository) : IEndpointRequestHandler<Command>
    {
        public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var isNotUnqique = await bookRepository.ExistsAsync(request.Isbn);

            if (isNotUnqique)
            {
                return TypedResults.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    detail: $"ISBN {request.Isbn} must be unique");
            }

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Isbn = request.Isbn,
            };

            await bookRepository.AddAsync(book);

            var response = new BookResponse(book.Id, book.Title, book.Isbn);

            return TypedResults.Created($"books/{book.Id}", response);
        }
    }
}
