
using Core.Dto;
using Core.Domain;

namespace Core.Import;

public static class ProductDomainImporter
{
    public static ImportResult<Product> Load(
        ImportResult<ProductDto> importResult)
    {
        ArgumentNullException.ThrowIfNull(importResult);

        var products = new List<Product>();
        var errors = new List<string>(importResult.Errors);

        for (int i = 0; i < importResult.Items.Count; i++)
        {
            ProductDto dto = importResult.Items[i];

            try
            {
                Product product = Product.FromDto(dto);
                products.Add(product);
            }
            catch (Exception ex) when (
                ex is ArgumentException ||
                ex is InvalidOperationException)
            {
                errors.Add($"Запис {i + 1}: {ex.Message}");
            }
        }

        return new ImportResult<Product>(products, errors);
    }
}
