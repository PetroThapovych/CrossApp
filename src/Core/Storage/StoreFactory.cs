
using Core.Abstractions;

namespace Core.Storage;

public static class StoreFactory
{
    public static ICatalogStore Create(string[] args)
    {
        bool useFile = args.Contains("--file");

        string dataPath = Path.Combine(
            AppContext.BaseDirectory,
            "data",
            "catalog.json");

        ICatalogStore store = useFile
            ? new FileCatalogStore(dataPath)
            : new InMemoryCatalogStore();

        return store;
    }
}
