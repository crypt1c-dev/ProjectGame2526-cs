using System;

namespace ProjectGame2526;

public class ExitGameMenuItem : MenuItem
{
    public ExitGameMenuItem() : base("Exit")
    {
        
    }
    public override void Activate(Game game)
    {
        Environment.Exit(0);
    }
}
