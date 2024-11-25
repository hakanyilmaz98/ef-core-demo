using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Messaging;
using Books.Api.Abstractions.Services;
using Books.Api.Constants;
using Books.Api.Features.Books.Shared.Mapping;
using MediatR;

namespace Books.Api.Features.Books.Queries;

public static class GetBooks
{
    public record Query() : IEndpointRequest;

    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("books", async (ISender sender) =>
            {
                return await sender.Send(new Query());
            })
                .WithTags(EndpointTags.Books)
                .WithOpenApi();
        }
    }

    public class Handler(IBookService bookRepository) : IEndpointRequestHandler<Query>
    {
        public async Task<IResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var books = await bookRepository.GetAsync();

            return TypedResults.Ok(books.MapToResponse());
        }
    }
}
