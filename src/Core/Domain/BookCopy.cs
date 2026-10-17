
namespace Core.Domain;

public sealed class BookCopy
{
    public string Id { get; }
    public bool IsIssued { get; private set; }
    public string? IssuedToReaderId { get; private set; }

    public BookCopy(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException(
                "Ідентифікатор примірника обов'язковий", nameof(id));

        Id = id.Trim();
    }

    internal void IssueTo(string readerId)
    {
        if (IsIssued)
            throw new InvalidOperationException(
                $"Примірник {Id} вже виданий");

        IsIssued = true;
        IssuedToReaderId = readerId;
    }

    internal void Return()
    {
        if (!IsIssued)
            throw new InvalidOperationException(
                $"Примірник {Id} не виданий");

        IsIssued = false;
        IssuedToReaderId = null;
    }
}
