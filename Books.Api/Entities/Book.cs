namespace Books.Api.Entities;

public class Book
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public required string Isbn { get; set; }
    public required string Author { get; set; }
}