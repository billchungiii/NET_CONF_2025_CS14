namespace PartialWithSG001
{
    public class OrderCreatedEventArgs : EventArgs
    {
        public string OrderId { get; }
        public decimal Amount { get; }
        public DateTime CreatedAt { get; }

        public OrderCreatedEventArgs(string orderId, decimal amount)
        {
            OrderId = orderId;
            Amount = amount;
            CreatedAt = DateTime.UtcNow;
        }
    }



}
