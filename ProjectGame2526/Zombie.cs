using System;
using System.Security.Cryptography.X509Certificates;

namespace ProjectGame2526;

public class Zombie : Entity
{
    protected int attackDamage;
    protected ConsoleColor color = ConsoleColor.Yellow;
    protected Level gameLevel;
    private int width;
    private int height;

    public int AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }
    public Zombie() : base(2, 2, 'Z', 1, 1, 1)
    {
        attackDamage = 1;
    }
    public Zombie(int newX, int newY, char newSymbol, int newLives, int newAttackDamage, double newXSpeed, double newYSpeed, int newWidth, int newHeight, Level level) : base(newX, newY, newSymbol, newLives, newXSpeed, newYSpeed, newWidth, newHeight)
    {
        gameLevel = level;
        attackDamage = newAttackDamage;
    }

    public override void Draw(int uiXOffset, int uiYOffset)
    {
        ConsoleColor originalColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        base.Draw(uiXOffset, uiYOffset);
        Console.ForegroundColor = originalColor;
    }
    // public void Spawn(out int x, out int y)
    // {
    //     Random rand = new Random();
    //     x = rand.Next(width - 1, 80);
    //     y = rand.Next(0, 25);
    // }


    public override void Update(double dt)
    {
        double previousPositionX = xPos;
        double previousPositionY = yPos;
        base.Update(dt);

        //after move check
        if (gameLevel.GetElementTypeAt(CursorY, CursorX) == LevelElementType.Wall || gameLevel.GetElementTypeAt(CursorY, CursorX) == LevelElementType.BreakableWall)
        {
            //reset position to before update
            xPos = previousPositionX;
            yPos = previousPositionY;
            //turn xSpeed around
            xSpeed = -xSpeed;
            ySpeed = -ySpeed;
        }
    }

        public void SetLevel(Level newLevel)
    {
        gameLevel = newLevel;
    }
}
