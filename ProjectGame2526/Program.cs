namespace ProjectGame2526;
using System.IO;

public class Program
{

    static void Main(string[] args)
    {
        
        // create the game
        Game game = new Game(60, 20);

        // start the game loop
        RunGameLoop(game);
    }

    protected static void RunGameLoop(Game game)
    {
        int refreshRate = 40;

        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;

        Thread.Sleep(1000);
       // game.Draw(refreshRate);


        Console.SetCursorPosition(0, 0);

        while (true)
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                game.OnInput(key.Key);
            }

            System.Threading.Thread.Sleep(1000 / refreshRate);

            // Reset(game);
            
            // game.Reset(game);
            game.Update(1.0f / (float)refreshRate);
            game.Draw(refreshRate);
        }
    }
}