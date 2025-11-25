namespace PartialWithSG001
{
    internal class Program
    {
        public static void Main()
        {
            // 建立實體 - partial constructor 會自動初始化事件基礎設施
            var order = new OrderEntity();

            // 訂閱事件(可選)
            order.OrderCreated += (sender, e) =>
            {
                Console.WriteLine($"訂單已建立: {e.OrderId}, 金額: {e.Amount:C}");
            };

            order.OrderStatusChanged += (sender, e) =>
            {
                Console.WriteLine($"訂單狀態變更: {e.OldStatus} -> {e.NewStatus}");
            };

            // 執行業務邏輯
            order.Create("ORD-001", 1500.00m);
            order.ChangeStatus(OrderStatus.Processing);
            order.ChangeStatus(OrderStatus.Shipped);
        }

    }

    [DomainEntity]
    public partial class OrderEntity
    {
        public string OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; private set; }

        // 2. 宣告 partial constructor - 由 source generator 生成實作
        public partial OrderEntity();

        // 3. 宣告 partial events - 由 source generator 自動加入發布邏輯
        public partial event EventHandler<OrderCreatedEventArgs>? OrderCreated;
        public partial event EventHandler<OrderStatusChangedEventArgs>? OrderStatusChanged;

        //protected partial void OnOrderCreated(OrderCreatedEventArgs e);
        //protected partial void OnOrderStatusChanged(OrderStatusChangedEventArgs e);


        // 4. 業務邏輯方法 - 手動觸發事件
        public void Create(string orderId, decimal amount)
        {
            OrderId = orderId;
            TotalAmount = amount;
            Status = OrderStatus.Created;

            // 觸發事件 - source generator 會自動加入發布基礎設施
            //_OrderCreated?.Invoke(this, new OrderCreatedEventArgs(orderId, amount));
            OnOrderCreated(new OrderCreatedEventArgs(orderId, amount));
        }

        public void ChangeStatus(OrderStatus newStatus)
        {
            var oldStatus = Status;
            Status = newStatus;

           // _OrderStatusChanged?.Invoke(this, new OrderStatusChangedEventArgs(oldStatus, newStatus));
           OnOrderStatusChanged(new OrderStatusChangedEventArgs(oldStatus, newStatus));
        }
    }

    public enum OrderStatus
    {
        Created,
        Processing,
        Shipped,
        Completed,
        Cancelled
    }

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

    [AttributeUsage(AttributeTargets.Class)]
    public class DomainEntityAttribute : Attribute
    {
    }



}
