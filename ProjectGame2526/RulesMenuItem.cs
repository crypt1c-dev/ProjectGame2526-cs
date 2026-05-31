using System;

namespace ProjectGame2526;

public class RulesMenuItem : MenuItem
{
    public RulesMenuItem() : base("Rules")
    {

    }

    public override void Activate(Game game)
    {
        game.CurrentGameState = GameState.Rules;
    }
}
