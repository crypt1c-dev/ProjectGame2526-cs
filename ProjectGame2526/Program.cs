namespace ProjectGame2526;
using System.IO;

public class Program
{

    static void Main(string[] args)
    {
        
        // create the game
        Game game = new Game(60, 20);

        StreamWriter writer = new StreamWriter("test.txt");
        writer.WriteLine("New game started at");
        writer.WriteLine("Level is starting...");
        writer.Close();

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

    // protected static void Reset(Game game)
    // {
    //     Console.BackgroundColor = ConsoleColor.Black;
    //     Console.ForegroundColor = ConsoleColor.Black;
    //     int uiXOffset = game.GetXOffset();
    //     int uiYOffset = game.GetYOffset();

    //     Console.SetCursorPosition(uiXOffset, uiYOffset);
    //     for (int y = uiYOffset; y < game.GetHeight() + uiYOffset; ++y)
    //     {
    //         for (int x = uiXOffset; x < game.GetWidth() + uiXOffset; ++x)
    //         {
    //             Console.Write(" ");
    //         }
    //         Console.WriteLine();
    //     }
    //     Console.SetCursorPosition(0, 0);
    // }
}