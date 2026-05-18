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
        string highscoreAsText = $"{newHighscore.PlayerName}:{newHighscore.Score}";
        File.AppendAllText("highscores.txt", highscoreAsText + Environment.NewLine);
    }

}
