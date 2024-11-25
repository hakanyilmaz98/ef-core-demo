using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Messaging;
using Books.Api.Abstractions.Services;
using Books.Api.Constants;
using Books.Api.Features.Books.Shared.Mapping;
using MediatR;

namespace Books.Api.Features.Books.Queries;

public static class GetBook
{
    public record Query(Guid Id) : IEndpointRequest;

    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("books/{id}", async (Guid id, ISender sender) =>
            {
                var query = new Query(id);
                return await sender.Send(query);
            })
                .WithTags(EndpointTags.Books)
                .WithOpenApi();
        }
    }

    public class Handler(IBookService bookRepository) : IEndpointRequestHandler<Query>
    {
        public async Task<IResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var book = await bookRepository.GetByIdAsync(request.Id);

            return book is null ? TypedResults.NotFound() : TypedResults.Ok(book.MapToResponse());
        }
    }
}
