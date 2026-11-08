using Core.Events;


namespace Core.Notification;
public interface INotificationSink
{
    void Notify(CatalogChangedEventArgs e);
}