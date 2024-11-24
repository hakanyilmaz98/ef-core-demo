using Books.Api.Entities;

namespace Books.Api.Abstractions.Repositories;

public interface IBookRepository
{
    public Task<Book> AddAsync(Book book);
    public Task<bool> ExistsAsync(string isbn);
    public Task<IReadOnlyList<Book>> GetAsync();
    public Task<Book?> GetByIdAsync(Guid id);
    public Task<Book> UpdateAsync(Book updatedBook);
    public Task<bool> DeleteAsync(Guid id);
}
