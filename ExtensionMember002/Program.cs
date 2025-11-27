namespace ExtensionMember002
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /* 語意上較不明顯, 需要記得 MyExtensions 這個類別 */
            _ = MyExtensions.Range(5, 10);
            /* 語意上較明顯, 直覺使用 int 的成員 */
            var result = int.Range(5, 10);
            Console.WriteLine(string.Join(", ", result));
        }
    }

    public static class MyExtensions
    {
        extension(int value)
        {
            public static IEnumerable<int> Range(int start, int count)
            {
                return Enumerable.Range(start, count);
            }
        }
    }
}
