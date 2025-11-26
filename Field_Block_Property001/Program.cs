namespace Field_Block_Property001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sample = new SampleClass();
            sample.X = 10;
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

        /// <summary>
        /// field 基本用法
        /// </summary>
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

        /// <summary>
        /// 延遲初始化
        /// </summary>
        public IReadOnlyList<int> Numbers => field ?? [];
      
    }
}
