using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Messaging;
using Books.Api.Abstractions.Services;
using Books.Api.Constants;
using Books.Api.Entities;
using Books.Api.Extensions;
using Books.Api.Features.Books.Shared.Contracts;
using Books.Api.Features.Books.Shared.Mapping;
using MediatR;

namespace Books.Api.Features.Books.Commands;

public static class UpdateBook
{
    public record Command(Guid Id, string Title, string Isbn) : IEndpointRequest;

    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("books/{id}", async (Guid id, BookRequest request, ISender sender) =>
            {
                var command = new Command(id, request.Title, request.Isbn);
                return await sender.Send(command);
            })
                .AddValidationFilter<BookRequest>()
                .WithTags(EndpointTags.Books)
                .WithOpenApi();
        }
    }

    public class Handler(IBookService bookRepository) : IEndpointRequestHandler<Command>
    {
        public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var existing = await bookRepository.GetByIdAsync(request.Id);

            if (existing is null)
            {
                return TypedResults.NotFound();
            }

            if (existing.Isbn != request.Isbn && await bookRepository.ExistsAsync(request.Isbn))
            {
                return TypedResults.Problem(
                        statusCode: StatusCodes.Status409Conflict,
                        detail: $"ISBN cannot be updated, because a book with the ISBN '{request.Isbn}' already exists");
            }

            var book = new Book
            {
                Id = request.Id,
                Title = request.Title,
                Isbn = request.Isbn,
            };

            var updatedBook = await bookRepository.UpdateAsync(book);

            return TypedResults.Ok(updatedBook.MapToResponse());
        }
    }
}
