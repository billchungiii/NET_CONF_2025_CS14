using System;

namespace ExtensionMember001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rect = new MyRectangle { Width = 3, Height = 4 };
            Console.WriteLine($"對角線長度: {rect.GetDiagonal()}");
            Console.WriteLine($"對角線長度另一種 : {MyRectangleExtensions.GetDiagonal(rect)}");
            Console.WriteLine($"周長: {rect.Perimeter}");
            Console.WriteLine($"周長另一種: {MyRectangleExtensions.get_Perimeter(rect)}");
        }
    }

    public class MyRectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public double GetArea()
            => Width * Height;       
    }

    public static class MyRectangleExtensions
    {
        
        extension(MyRectangle rect)
        {
            /// <summary>
            /// 根據矩形的寬度與高度計算對角線的長度 (擴充為方法)
            /// </summary>
            /// <returns>回傳對角線的長度，型別為雙精度浮點數 (double)</returns>
            public double GetDiagonal()
            {
                if (rect is null) { throw new NullReferenceException(); }
                return Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height);
            }

            public double Perimeter
            {
                get
                {
                    if (rect is null ) { throw new NullReferenceException(); }
                    return (rect.Width + rect.Height) * 2;
                }
            }
        }
    }
}
