namespace inpsGE
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using var game = new Engine(args.Length > 0 ? (args[0].Contains("title=") ? args[0]?.Split('=')[1] : "Title") : "Title");
            game.Run();
        }
    }
}