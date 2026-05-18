using System;

namespace ProjectGame2526;

public class HighscoresMenuItem : MenuItem
{
    public HighscoresMenuItem() : base("Highscores")
    {

    }

    public void LoadHighScores()
    {
        
    }

    public override void Activate(Game game)
    {
        game.CurrentGameState = GameState.HighscoresMenu;
    }
}
