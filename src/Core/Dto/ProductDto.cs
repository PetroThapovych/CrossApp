namespace Core.Dto;
public record ProductDto(string Id, string? Sku, string? Name, string? Unit, int? Quantity) : IImportItem;

public sealed record ImportResult<T>(IReadOnlyList<T> Items, IReadOnlyList<string> Errors);