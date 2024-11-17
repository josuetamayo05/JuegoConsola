namespace ComprEmojis;

class Program
{
    static void Main(string[] args)
    {
        string[] emojis = { "💰", "⭐", "🏁", "🚀", "👽", "🐱‍🐉", "💣", "▮", "■", "❤" };
        foreach (var emoji in emojis)
        {
            Console.WriteLine($"Emoji: {emoji}, Length: {emoji.Length}"); // .Length cuenta los caracteres        
        }
    }
}
