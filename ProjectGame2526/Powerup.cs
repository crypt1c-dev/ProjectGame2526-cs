using System;

namespace ProjectGame2526;

public enum PowerupType
{
    MedKit,
    Invincibility,
    Coin,
    Bomb
}

public class Powerup : Sprite
{
    protected PowerupType type;
    public Powerup() : base(0, 0, '?')
    
    }

    public Powerup(PowerupType newType, int newX, int newY, char newSymbol) : base(newX, newY, newSymbol)
    {
        type = newType;
        symbol = newSymbol;
        switch(type)
        {
            case PowerupType.Invincibility:
                symbol = 'I';
                break;
            case PowerupType.Coin:
                symbol = 'O';
                break;
            case PowerupType.MedKit:
                symbol = '+';
                break;
            case PowerupType.Bomb:
                symbol = 'B';
                break;
        }
    }

    public override void Draw(int xOffset, int yOffset)
    {
        ConsoleColor originalColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Green;
        base.Draw(xOffset, yOffset);
        Console.ForegroundColor = originalColor;
    }

    public PowerupType GetPowerupType()
    {
        return type;
    }
}
