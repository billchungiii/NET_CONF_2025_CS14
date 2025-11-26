using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;

namespace CompoundAssignment001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var o = new MyClass { Value = 10 };
            o = o + 6;      // Uses operator + defined in MyClass
            o += 5;          // Uses operator += defined in MyClass
            o -= 3;          // Uses extension operator -= defined in MyExtensions
        }
    }

    public class MyClass
    {
        public BigInteger Value;
        public static MyClass operator +(MyClass a, BigInteger b)
        {
            _ = a.Value + b;
            return a;
        }

        public static MyClass operator -(MyClass a, BigInteger b)
        {
            return new MyClass { Value = a.Value - b };
        }

        public void operator +=(BigInteger b)
        {
            Value += b;
        }
    }

    public static class MyExtensions
    {
        extension(MyClass a)
        {
            public void operator -=(BigInteger b)
            {
                a.Value -= b;
            }
        }
    }

    public static class OtherExtensions
    {
        public static MyClass Subsctrat(this MyClass a, BigInteger b)
        {
            a.Value -= b;
            return a;
        }
        /*  不可以使用舊有的擴充方法 (this-parameter extentions)  */
        //public static void operator -=(this MyClass my, BigInteger b)
        //{
        //    my.Value -= b;
        //}
    }
}
