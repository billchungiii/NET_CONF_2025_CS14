namespace UnboundNameOf001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(nameof(List<int>));
            Console.WriteLine(nameof(List<>));
            Console.WriteLine(nameof(MyClass<int>));
            Console.WriteLine(nameof(MyClass<>));

            var handlerMap = new Dictionary<string, Type>
            {
                [nameof(MyClass<>)] = typeof(MyClass<>),
                [nameof(List<>)] = typeof(List<>)
            };
        }
    }

    /// <summary>
    /// 假設一開始沒有型別約束, 若改變約束可能會導致 Console.WriteLine(nameof(MyClass<int>)); 無法編譯
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyClass<T>
    {

    }


    //public class MyClass<T> where T : class
    //{

    //}
}
