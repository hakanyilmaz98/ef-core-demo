using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Messaging;
using Books.Api.Abstractions.Services;
using Books.Api.Constants;
using MediatR;

namespace Books.Api.Features.Books.Commands;

public static class DeleteBook
{
    public record Command(Guid Id) : IEndpointRequest;

    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("books/{id}", async (Guid id, ISender sender) =>
            {
                var command = new Command(id);
                return await sender.Send(command);
            })
                .WithTags(EndpointTags.Books)
                .WithOpenApi();
        }
    }

    public class Handler(IBookRepository bookRepository) : IEndpointRequestHandler<Command>
    {
        public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var isDeleted = await bookRepository.DeleteAsync(request.Id);

            return isDeleted ? TypedResults.NoContent() : TypedResults.NotFound();
        }
    }
}
