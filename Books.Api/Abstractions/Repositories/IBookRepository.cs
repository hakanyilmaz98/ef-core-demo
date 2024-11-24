using Books.Api.Entities;

namespace Books.Api.Abstractions.Repositories;

public interface IBookRepository
{
    public Task<Book> AddAsync(Book book);
    public Task<IReadOnlyList<Book>> GetAsync();
    public Task<Book?> GetByIdAsync(Guid Id);
    public Task<Book?> UpdateAsync(Book book);
    public Task<bool> DeleteAsync(Guid id);
}
