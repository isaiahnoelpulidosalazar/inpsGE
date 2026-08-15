namespace inpsGE
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using var game = new Engine();
            game.Run();
        }
    }
}