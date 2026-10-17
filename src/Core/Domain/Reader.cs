
namespace Core.Domain;

public sealed class Reader
{
    public string Id { get; }
    public string Name { get; }

    public int OpenLoans { get; private set; }

    public Reader(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException(
                "Ідентифікатор читача обов'язковий", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Iм'я читача обов'язкове", nameof(name));

        Id = id.Trim();
        Name = name.Trim();
    }

    internal void RegisterLoan()
    {
        OpenLoans++;
    }

    internal void CloseLoan()
    {
        if (OpenLoans <= 0)
            throw new InvalidOperationException("У читача немає відкритих видач");

        OpenLoans--;
    }
}
