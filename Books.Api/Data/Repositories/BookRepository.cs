using Books.Api.Abstractions.Repositories;
using Books.Api.Entities;

namespace Books.Api.Data.Repositories;

public class BookRepository : IBookRepository
{
    private static readonly List<Book> _books = [];

    public async Task<Book> AddAsync(Book book)
    {
        _books.Add(book);

        return book;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book is null)
        {
            return false;
        }

        return _books.Remove(book);
    }

    public async Task<IReadOnlyList<Book>> GetAsync()
    {
        return _books.AsReadOnly();
    }

    public async Task<Book?> GetByIdAsync(Guid Id)
    {
        return _books.FirstOrDefault(b => b.Id == Id);
    }

    public async Task<Book?> UpdateAsync(Guid id, Book updated)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book is null)
        {
            return null;
        }

        book.Title = updated.Title;
        book.Isbn = updated.Isbn;

        return book;
    }
}
