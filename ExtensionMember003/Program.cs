using System.Drawing;
using System.Numerics;
using System.Reflection;

namespace ExtensionMember003
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var vector = new Vector2(3, 4);
            vector++;
            Console.WriteLine(vector); // Output: <4, 5>
        }
    }

    /// <summary>
    /// 擴充運算子 (但不是每個運算子都能這樣擴充)
    /// 例如 implicit 和 explicit 無法擴充
    /// </summary>
    public static class MyExtensions
    {
        extension(Vector2 source)
        {
            public static Vector2 operator ++(Vector2 v)
            {
                return new Vector2(v.X + 1, v.Y + 1);
            }

            public static Vector2 operator --(Vector2 v)
            {
                return new Vector2(v.X - 1, v.Y - 1);
            }
        }
    }
}
