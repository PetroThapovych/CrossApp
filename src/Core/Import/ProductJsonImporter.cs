
using Core.Dto;
using System.Text.Json;
using System.Xml.Linq;

namespace Core.Import;

public static class ProductJsonImporter
{
    public static ImportResult<ProductDto> Load(string path)
    {
        var items = new List<ProductDto>();
        var errors = new List<string>();

        string json = File.ReadAllText(path);

        try
        {
            using JsonDocument document = JsonDocument.Parse(json);

            var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};

            int number = 0;

            foreach (JsonElement element in document.RootElement.EnumerateArray())
            {
                number++;

                try
                {
                    ProductDto? product = element.Deserialize<ProductDto>(options);


                    string? error = product switch
                    {
                        
                        { Sku: null or "" } => "SKU порожній",
                        { Name: null or "" } => "назва товару порожня",
                        { Quantity: < 0 } => "кількість не може бути від'ємною",
                        _ => null
                    };


                    if (error is not null)
                    {
                        errors.Add($"запис {number}: {error}");
                        continue;
                    }

                    items.Add(product!);
                }
                catch (JsonException)
                {
                    errors.Add($"запис {number}: некоректний формат товару");
                }
            }
        }
        catch (JsonException)
        {
            errors.Add("файл містить некоректний JSON");
        }

        return new ImportResult<ProductDto>(items, errors);
    }
}
