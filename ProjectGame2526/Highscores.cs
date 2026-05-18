using System;

namespace ProjectGame2526;

public class Highscores
{
    protected int score;
    protected string playerName;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }
    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public Highscores()
    {

    }

    public Highscores(int newScore, string newPlayerName)
    {
        Score = newScore;
        PlayerName = newPlayerName;
    }

    public static void SaveHighScore(Highscores newHighscore)
    {
        string highscoreAsText =
    $"{newHighscore.PlayerName}:{newHighscore.Score}";

        StreamWriter writer = null;

        try
        {
            writer = new StreamWriter("highscores.txt", true); // True = append to file instead of overwriting

            writer.WriteLine(highscoreAsText);
        }
        catch (Exception e)
        {
            Console.WriteLine(
                "Error while saving highscore: {0}",
                e.Message
            );
        }
        finally
        {
            if (writer != null)
            {
                writer.Close();
            }
        }
    }

}
