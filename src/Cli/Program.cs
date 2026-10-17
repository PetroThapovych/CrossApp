
using Core.Domain;
using Core.Dto;
using Core.Import;

var dtoResult = ProductCsvImporter.Load("data/sample.csv");
var result = ProductDomainImporter.Load(dtoResult);

Console.WriteLine($"Створено товарів: {result.Items.Count}");
Console.WriteLine($"Усього помилок: {result.Errors.Count}");

foreach (string error in result.Errors)
{
    Console.WriteLine(error);
}

Console.WriteLine("\n=== Перевірка ліміту видач ===");

var library = new LibraryService();
var reader = new Reader("R-001", "Петро");

for (int i = 1; i <= 5; i++)
{
    library.IssueCopy(reader, new BookCopy($"B-{i:000}"));
}

Console.WriteLine($"Відкритих видач: {reader.OpenLoans}");

TryDo(
    "шоста видача",
    () => library.IssueCopy(reader, new BookCopy("B-006")));

Console.WriteLine("\n=== Перевірка станів замовлення ===");

var order = new Order("ORD-001");

Console.WriteLine(order);

order.Confirm();
Console.WriteLine(order);

TryDo("повторне підтвердження", () => order.Confirm());

var cancelledOrder = new Order("ORD-002");

cancelledOrder.Cancel();
Console.WriteLine(cancelledOrder);

TryDo("повторне скасування", () => cancelledOrder.Cancel());

Console.WriteLine("\n=== Сценарiй 1: успiх ===");

Product product = Product.Create(
    "P-001",
    "sku-001",
    "Цемент М400 25кг",
    "шт",
    100);

Console.WriteLine(product);

product.RegisterArrival(50);
product.Issue(30);

Console.WriteLine(product);

Console.WriteLine("\n=== Сценарiй 2: порушення iнварiантiв ===");

TryDo(
    "видача бiльша за залишок",
    () => product.Issue(1000));

TryDo(
    "порожнiй SKU",
    () => Product.Create("P-002", " ", "Пiсок", "т", 10));

TryDo(
    "вiд'ємний залишок",
    () => Product.Create("P-003", "SKU-003", "Цегла", "шт", -5));


static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine(
            $" {title}: виняток НЕ спрацював — iнварiант вiдсутнiй!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $" {title}: {ex.GetType().Name} — {ex.Message}");
    }
}
