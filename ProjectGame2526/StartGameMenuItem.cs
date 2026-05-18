using System;

namespace ProjectGame2526;

public class StartGameMenuItem : MenuItem
{
    public StartGameMenuItem() : base("Start game")
    {
        
    }

    public override void Activate(Game game)
    {
        game.CurrentGameState = GameState.GameRunning;
    }
}
