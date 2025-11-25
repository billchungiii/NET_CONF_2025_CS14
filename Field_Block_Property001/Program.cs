namespace Field_Block_Property001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    public class SampleClass
    {
        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                if (value >= 0)
                {
                    _x = value;
                }
            }
        }

        public int Y
        {
            get; 
            set
            {
                if (value >= 0)
                {
                    field = value;
                }
            }
        }

    }
}
