namespace LambdaModifiers001
{
    delegate bool TryParse<T>(string s, out T result);
    internal class Program
    {
        static void Main(string[] args)
        {
            // before C# 14.0, 使用 ref/out/int/ref readonly/scoped 修飾詞的 Lambda 參數必須顯式指定型別
            TryParse<int> tryParse = (string text, out int result) => int.TryParse(text, out result);

            // 從 C# 14.0 開始，可以省略型別，編譯器會自動推斷
            TryParse<int> tryParse2 = (text, out result) => int.TryParse(text, out result);

            
        }
    }
}
