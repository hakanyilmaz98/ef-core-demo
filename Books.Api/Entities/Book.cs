namespace Books.Api.Entities;

public class Book
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Isbn { get; set; }
}