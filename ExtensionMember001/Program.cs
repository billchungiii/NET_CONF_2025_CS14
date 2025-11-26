using System;

namespace ExtensionMember001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    public class MyRectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public double GetArea()
            => Width * Height;

        public double GetPerimeter()
            => 2 * (Width + Height);
    }

    public static class MyRectangleExtensions
    {
        /// <summary>
        /// 根據矩形的寬度與高度計算對角線的長度
        /// </summary>
        /// <param name="rect">用來計算對角線的矩形</param>
        /// <returns>回傳對角線的長度，型別為雙精度浮點數 (double)</returns>
        public static double GetDiagonal(this MyRectangle rect)
        {
            if (rect is null) throw new ArgumentNullException(nameof(rect));
            return Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height);
        }
    }
}
