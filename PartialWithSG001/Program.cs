namespace PartialWithSG001
{
    internal class Program
    {
        public static void Main()
        {
            
            var order = new OrderEntity();
            // 訂閱事件 (也用上了 Null-conditional assignment)
            order?.OrderCreated += (sender, e) =>
            {
                Console.WriteLine($"訂單已建立: {e.OrderId}, 金額: {e.Amount:C}");
            };

            order?.OrderStatusChanged += (sender, e) =>
            {
                Console.WriteLine($"訂單狀態變更: {e.OldStatus} -> {e.NewStatus}");
            };

            // 執行業務邏輯
            order?.Create("ORD-001", 1500.00m);
            order?.ChangeStatus(OrderStatus.Processing);
            order?.ChangeStatus(OrderStatus.Shipped);
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

        // 4. 業務邏輯方法 - 手動觸發事件
        public void Create(string orderId, decimal amount)
        {
            OrderId = orderId;
            TotalAmount = amount;
            Status = OrderStatus.Created;                     
            OnOrderCreated(new OrderCreatedEventArgs(orderId, amount));
        }

        public void ChangeStatus(OrderStatus newStatus)
        {
            var oldStatus = Status;
            Status = newStatus;
           OnOrderStatusChanged(new OrderStatusChangedEventArgs(oldStatus, newStatus));
        }
    }



}
