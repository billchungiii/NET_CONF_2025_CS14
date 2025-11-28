namespace ExtensionMember004
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string message = "Hello, World!";
            Console.WriteLine(string.Join("\n", message.Repeat(3)));
        }
    }

    public static class MyExtensions
    {
        extension<T>(T source)
        {
            public IEnumerable<T> Repeat(int count)
            {
                return Enumerable.Repeat(source, count);
            }
        }
    }
}
