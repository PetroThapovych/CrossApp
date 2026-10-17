
namespace Core.Domain;

public sealed class Order
{
    public string Id { get; }
    public OrderStatus Status { get; private set; }

    public Order(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException(
                "Ідентифікатор замовлення обов'язковий",
                nameof(id));

        Id = id.Trim();
        Status = OrderStatus.Draft;
    }

    public void Confirm()
    {
        Status = GetNextStatus(Status, OrderStatus.Confirmed);
    }

    public void Cancel()
    {
        Status = GetNextStatus(Status, OrderStatus.Cancelled);
    }

    private static OrderStatus GetNextStatus(
        OrderStatus current,
        OrderStatus requested)
    {
        return (current, requested) switch
        {
            (OrderStatus.Draft, OrderStatus.Confirmed)
                => OrderStatus.Confirmed,

            (OrderStatus.Draft, OrderStatus.Cancelled)
                => OrderStatus.Cancelled,

            _ => throw new InvalidOperationException(
                $"Недопустимий перехід замовлення: " +
                $"{current} -> {requested}")
        };
    }

    public override string ToString() =>
        $"Замовлення {Id}: {Status}";
}
