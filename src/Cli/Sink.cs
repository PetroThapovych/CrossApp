using Core.Events;
namespace Core.Notification;

static class SinkFormat
{
    public static string Line(CatalogChangedEventArgs e) =>
        $"{e.At:HH:mm:ss} {e.Kind,-8} {e.Id} {e.Delta,4:+0;-0} → залишок {e.QuantityAfter}";
}

sealed class ConsoleSink : INotificationSink
{
    public void Notify(CatalogChangedEventArgs e) =>
        Console.WriteLine("[sink] " + SinkFormat.Line(e));
}

sealed class FileSink(string path) : INotificationSink
{
    public void Notify(CatalogChangedEventArgs e) =>
        File.AppendAllText(path, SinkFormat.Line(e) + Environment.NewLine);
}

// Щоб передати сервісу кілька одержувачів через один параметр конструктора
sealed class CompositeSink(params INotificationSink[] sinks) : INotificationSink
{
    public void Notify(CatalogChangedEventArgs e)
    {
        foreach (var s in sinks) s.Notify(e);
    }
}