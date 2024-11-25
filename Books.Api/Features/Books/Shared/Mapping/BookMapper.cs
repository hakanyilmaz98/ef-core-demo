using Books.Api.Entities;
using Books.Api.Features.Books.Shared.Contracts;

namespace Books.Api.Features.Books.Shared.Mapping;

public static class BookMapper
{
    public static BookResponse MapToResponse(this Book book) =>
        new BookResponse(book.Id, book.Title, book.Isbn);

    public static IEnumerable<BookResponse> MapToResponse(this IEnumerable<Book> books) =>
        books.Select(MapToResponse).ToArray();
}
