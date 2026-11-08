
using Core.Abstractions;
using Core.Events;
using Core.Import;
using Core.Notification;
using Core.Services;
using Core.Storage;

ICatalogStore store = StoreFactory.Create(args);

var service = new CatalogService(store);


var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "app.log");
Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);


void OnCatalogChanged(object? sender, CatalogChangedEventArgs e)
{
    var line = $"{e.At:HH:mm:ss} {e.Kind,-8} {e.Id} {e.Delta,4:+0;-0} → залишок {e.QuantityAfter}";
    Console.WriteLine(line);
    File.AppendAllText(logPath, line + Environment.NewLine);
}


service.Changed += OnCatalogChanged;


int changes = 0;
service.Changed += (_, _) => changes++;


var created = service.Add("SKU-101", "Кабель UTP cat6", "м", 50);
service.Receive(created.Id, 25);
Console.WriteLine($"Подій отримано: {changes}");        

service.Changed -= OnCatalogChanged;                    
service.Receive(created.Id, 5);                        
Console.WriteLine($"Подій отримано: {changes}");        

service.Add("SKU-202", "Конектор RJ45", "шт", 5);


try
{
    service.Receive(created.Id, -10);                   
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
}

try
{
    service.Receive("no-such-id", 5);                  
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
}



var low = service.FindAll(p => p.Quantity < 10);
Console.WriteLine($"Залишок < 10: {low.Count} шт.");
foreach (var p in low)
    Console.WriteLine($"  {p.Id} {p.Name} ({p.Quantity})");



var sinkLogPath = Path.Combine(AppContext.BaseDirectory, "logs", "sink.log");

var sink = new CompositeSink(new ConsoleSink(), new FileSink(sinkLogPath));
var sinkService = new CatalogService(new InMemoryCatalogStore(), sink);   

var item = sinkService.Add("SKU-300", "Патч-корд", "шт", 10);
sinkService.Receive(item.Id, 4);



int skipped = 0;
var csvPath = "src/Cli/data/sample.csv";

var result = ProductCsvImporter.Load(csvPath, msg =>
{
    skipped++;                                   
    Console.WriteLine($"  Пропущено: {msg}");
});

foreach (var dto in result.Items)
{
    try
    {
        service.Add(
            dto.Sku ?? throw new InvalidOperationException("SKU відсутній."),
            dto.Name ?? throw new InvalidOperationException("Назва відсутня."),
            dto.Unit ?? "",
            dto.Quantity ?? throw new InvalidOperationException("Кількість відсутня."));
    }
    catch (Exception ex)
    {
        skipped++;
        Console.WriteLine($"  Пропущено (iнварiант): {dto.Sku}: {ex.Message}");
    }
}

Console.WriteLine($"\nУсього пропущено товарів: {skipped}");
