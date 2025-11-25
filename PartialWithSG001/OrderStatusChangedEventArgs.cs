namespace PartialWithSG001
{
    public class OrderStatusChangedEventArgs : EventArgs
    {
        public OrderStatus OldStatus { get; }
        public OrderStatus NewStatus { get; }
        public DateTime ChangedAt { get; }

        public OrderStatusChangedEventArgs(OrderStatus oldStatus, OrderStatus newStatus)
        {
            OldStatus = oldStatus;
            NewStatus = newStatus;
            ChangedAt = DateTime.UtcNow;
        }
    }



}
