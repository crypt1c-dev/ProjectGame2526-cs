using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace ProjectGame2526;

public class HighscoresScreen : Screen
{
    int scrollOffset = 0;
    public HighscoresScreen() : base("HighscoresMenu.txt", ConsoleColor.White, ConsoleColor.Black)
    {

    }
    public List<Highscores> LoadHighScores()
    {
        string highscoresAsText = "";

        StreamReader reader = null;

        try
        {
            reader = new StreamReader("Highscores.json");

            // lees volledige file
            highscoresAsText = reader.ReadToEnd();

            // zet json om naar lijst van highscores
            List<Highscores> highscores =
                JsonSerializer.Deserialize<List<Highscores>>(highscoresAsText);

            return highscores;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Highscores file not found.");

            return new List<Highscores>();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading highscores: {0}", e.Message);

            return new List<Highscores>();
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
    }

    public void SaveHighScores(List<Highscores> highscores)
    {
        string highscoresAsJson =
            JsonSerializer.Serialize(highscores);

        StreamWriter writer = null;

        try
        {
            writer = new StreamWriter("Highscores.json");

            writer.Write(highscoresAsJson);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error saving highscores: {0}", e.Message);
        }
        finally
        {
            if (writer != null)
            {
                writer.Close();
            }
        }
    }

    public void Draw(List<Highscores> highscores)
    {
        base.Draw();

        Console.ForegroundColor = ConsoleColor.White;

        // INFO TEXT (vaste UI, dus NIET in de loop)
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(45, 16);
        Console.Write("Scroll up/down");

        Console.SetCursorPosition(45, 17);
        Console.Write("to see more");

        Console.SetCursorPosition(45, 19);
        Console.Write("Press [BACKSPACE]");
        Console.SetCursorPosition(45, 20);
        Console.Write("To return to menu");
    // RESET COLOR
        Console.ForegroundColor = ConsoleColor.White;

        int startY = 16;

        // hoeveel highscores zichtbaar zijn
        int visibleScores = 3;

        for (int i = 0; i < visibleScores; i++)
        {
            int highscoreIndex = i + scrollOffset;

            // stop als er geen scores meer zijn
            if (highscoreIndex >= highscores.Count)
            {
                break;
            }

            Highscores highscore = highscores[highscoreIndex];

            int yPosition = startY + (i * 4);

            Console.SetCursorPosition(5, yPosition);
            Console.Write($"Name: {highscore.PlayerName}");

            Console.SetCursorPosition(5, yPosition + 1);
            Console.Write($"Score: {highscore.Score}");
        }
    }

    public void OnInput(ConsoleKey key, List<Highscores> highscores)
    {
        if (key == ConsoleKey.DownArrow)
        {
            if (scrollOffset < highscores.Count - 1)
            {
                scrollOffset++;
            }
        }

        if (key == ConsoleKey.UpArrow)
        {
            if (scrollOffset > 0)
            {
                scrollOffset--;
            }
        }
    }
}
