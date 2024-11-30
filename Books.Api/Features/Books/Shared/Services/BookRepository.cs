using Books.Api.Abstractions.Services;
using Books.Api.Entities;

namespace Books.Api.Features.Books.Shared.Services;

public class BookRepository : IBookRepository
{
    private static readonly List<Book> _books = [];

    public async Task<Book> AddAsync(Book book)
    {
        _books.Add(book);

        return book;
    }

    public async Task<bool> ExistsAsync(string isbn)
    {
        return _books.Exists(b => b.Isbn == isbn);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var book = _books.Find(b => b.Id == id);

        return book is not null
            ? _books.Remove(book)
            : false;
    }

    public async Task<IReadOnlyList<Book>> GetAsync()
    {
        return _books.AsReadOnly();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return _books.Find(b => b.Id == id);
    }

    public async Task<Book> UpdateAsync(Book updatedBook)
    {
        var book = _books.First(b => b.Id == updatedBook.Id);

        book.Title = updatedBook.Title;
        book.Isbn = updatedBook.Isbn;

        return book;
    }
}
