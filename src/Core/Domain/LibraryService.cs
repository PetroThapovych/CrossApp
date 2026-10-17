
namespace Core.Domain;

public sealed class LibraryService
{
    private const int MaxOpenLoans = 5;

    public void IssueCopy(Reader reader, BookCopy copy)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(copy);

        if (reader.OpenLoans >= MaxOpenLoans)
            throw new InvalidOperationException(
                $"Читач {reader.Name} вже має " +
                $"{MaxOpenLoans} відкритих видач");

        
        copy.IssueTo(reader.Id);
        reader.RegisterLoan();
    }

    public void ReturnCopy(Reader reader, BookCopy copy)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(copy);

        if (copy.IssuedToReaderId != reader.Id)
            throw new InvalidOperationException(
                "Примірник не закріплений за цим читачем");

        copy.Return();
        reader.CloseLoan();
    }
}
