
using Core.Abstractions;
using Core.Domain;

namespace Core.Storage;

public sealed class CachingCatalogStore : ICatalogStore
{
    private readonly ICatalogStore _inner;

    private readonly Dictionary<string, Product> _cache =
        new(StringComparer.OrdinalIgnoreCase);

    private bool _loaded;

    public CachingCatalogStore(ICatalogStore inner)
    {
        _inner = inner
            ?? throw new ArgumentNullException(nameof(inner));
    }

    private void EnsureLoaded()
    {
        if (_loaded)
            return;

        foreach (var product in _inner.List())
        {
            _cache[product.Id] = product;
        }

        _loaded = true;
    }

    public IReadOnlyList<Product> List()
    {
        EnsureLoaded();
        return _cache.Values.ToList();
    }

    public Product? GetById(string id)
    {
        EnsureLoaded();
        return _cache.GetValueOrDefault(id);
    }

    public void Add(Product item)
    {
        EnsureLoaded();
        _inner.Add(item);
        _cache.Add(item.Id, item);
    }

    public void Update(Product item)
    {
        EnsureLoaded();
        _inner.Update(item);
        _cache[item.Id] = item;
    }

    public bool Remove(string id)
    {
        EnsureLoaded();

        if (!_inner.Remove(id))
            return false;

        _cache.Remove(id);
        return true;
    }
}
