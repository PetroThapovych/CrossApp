
using Core.Abstractions;
using Core.Services;
using Core.Storage;

ICatalogStore store = StoreFactory.Create(args);

store = new CachingCatalogStore(store);

var service = new CatalogService(store);

Console.WriteLine($"Сховище: {store.GetType().Name}");
Console.WriteLine($"Декоратор: {store.GetType().Name}");

try
{
    var created = service.Add(
        "SKU-101",
        "Кабель UTP cat6",
        "м",
        50);

    service.Receive(created.Id, 25);

    foreach (var product in service.All())
    {
        Console.WriteLine(
            $" {product.Id} {product.Sku,-10} " +
            $"{product.Name,-22} {product.Quantity,5} {product.Unit}");
    }

    Console.WriteLine("\nТовари з кількістю від 50:");

    var found = service.Search(p => p.Quantity >= 50);

    foreach (var product in found)
    {
        Console.WriteLine(
            $"{product.Name}: {product.Quantity} {product.Unit}");
    }
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
}
