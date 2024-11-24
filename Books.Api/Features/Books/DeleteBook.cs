using Books.Api.Abstractions.Endpoints;
using Books.Api.Abstractions.Repositories;
using Books.Api.Constants;

namespace Books.Api.Features.Books;

public class DeleteBook
{
    public class Endpoint : IMinimalEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("books/{id}", Handle).WithTags(EndpointTags.Books);
        }

        public static async Task<IResult> Handle(Guid id, IBookRepository bookRepository)
        {
            var isDeleted = await bookRepository.DeleteAsync(id);

            return isDeleted ? TypedResults.NoContent() : TypedResults.NotFound();
        }
    }
}
