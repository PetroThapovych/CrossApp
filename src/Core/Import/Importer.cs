using Core.Dto;

namespace Core.Import;

public static class Importer
{
    public static ImportResult<IImportItem> Load(string path)
    {
        var items = new List<IImportItem>();
        var errors = new List<string>();

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            int number = i + 1;
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;

            switch (ParseLine(line))
            {
                case ProductResult product:
                    items.Add(product.Value);
                    break;

                case WarehouseResult warehouse:
                    items.Add(warehouse.Value);
                    break;

                case ParseFailed failed:
                    errors.Add($"рядок {number}: {failed.Reason}");
                    break;
            }
        }

        return new ImportResult<IImportItem>(items, errors);
    }


    private static ParseOutcome ParseLine(string line)
    {
        string[] parts = line.Split(';', StringSplitOptions.TrimEntries);

        if (string.IsNullOrWhiteSpace(parts[0]))
        {
            return new ParseFailed(
                "відсутня літера типу запису: очікую 'P' (товар) або 'W' (склад)");
        }

        return parts[0].ToUpperInvariant() switch
        {
            "P" => ParseProduct(parts),
            "W" => ParseWarehouse(parts),
            _ => new ParseFailed(
                $"невiдома лiтера '{parts[0]}': очікую 'P' або 'W'")
        };
    }


    private static ParseOutcome ParseProduct(string[] parts)
    {
        return parts switch
        {
            { Length: < 6 } => new ParseFailed($"для товару очікую 6 колонок, отримав {parts.Length}"),

            [_, _, "", _, _, _] or [_, _, _, "", _, _] => new ParseFailed("SKU або назва товару порожні"),

            [_, _, _, _, _, var quantity] when !int.TryParse(quantity, out int q) || q < 0
            => new ParseFailed($"кількість '{quantity}' не є невід'ємним числом"),

            [_, var id, var sku, var name, var unit, var quantity] =>
                new ProductResult(new ProductDto(id,sku,name,unit,int.Parse(quantity))),
            _ => new ParseFailed($"некоректний формат")
        };
    }

    private static ParseOutcome ParseWarehouse(string[] parts)
    {
        if (parts.Length != 4)
            return new ParseFailed(
                $"для складу очікую 4 колонки, отримав {parts.Length}");

        if (string.IsNullOrWhiteSpace(parts[2]))
            return new ParseFailed("назва складу порожня");

        return new WarehouseResult(
            new WarehouseDto(
                parts[1],
                parts[2],
                parts[3]));
    }

    private abstract record ParseOutcome;

    private sealed record ProductResult(ProductDto Value) : ParseOutcome;

    private sealed record WarehouseResult(WarehouseDto Value) : ParseOutcome;

    private sealed record ParseFailed(string Reason) : ParseOutcome;
}