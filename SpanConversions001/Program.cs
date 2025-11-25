using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanConversions001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            // 這個以前就可以了
            Span<int> span = array;
            ReadOnlySpan<int> readOnlySpan = span;
            // 以前要這樣 (array 轉換為 ReadOnlySpan<int> )
            new ReadOnlySpan<int>(array).Show();
            // C#14.0 新增，Span<T> 會隱含轉會為 ReadOnlySpan<T>，可以直接呼叫擴充方法 (會隱含轉換)
            array.AsSpan().Show(); /* 這裡的 AsSapn 會轉換為 Span<T> , 只有當參數為 string 才會轉成 ReadOnlySpan<char> */
            // C# 14.0 新增的功能，可以直接呼叫擴充方法 (會隱含轉換)
            array.Show();


            string text = "Hello, World!";
            // 這個以前也可以
            ReadOnlySpan<char> charSpan = text;
            // C# 14.0 新增的功能，可以直接呼叫擴充方法 (會隱含轉換)
            text.Display();
            // 以前要這樣 (當參數為 string 會轉成 ReadOnlySpan<char> )
            text.AsSpan().Display();

        }
    }

    public static class MyExtensions
    {
        public static void Display(this ReadOnlySpan<char> span)
        {
            for (int i = 0; i < span.Length; i++)
            {
                Console.Write(span[i]);
            }
            Console.WriteLine();
        }

        public static void Show<T>(this ReadOnlySpan<T> span)
        {
            foreach (var item in span)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
        }

    }
}
