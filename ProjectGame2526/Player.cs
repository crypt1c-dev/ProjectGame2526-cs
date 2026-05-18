using System;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

namespace ProjectGame2526;

public class Player : Entity
{
    protected bool isInvincible = false;
    protected bool canJump = false;
    public bool dead = false;
    protected ConsoleColor color = ConsoleColor.Red;
    protected Level gameLevel;
    protected int defaultPlayerLives = 3;
    int x, y;
    private int width;
    private int height;
    public bool IsInvincible
    {
        get { return isInvincible; }
        set { isInvincible = value; }
    }
    public bool CanJump
    {
        get { return canJump; }
        set { canJump = value; }
    }
    public int DefaultPlayerLives
    {
        get { return defaultPlayerLives; }
        set { defaultPlayerLives = value; }
    }
    public Player() : base(1, 1, '@', 3, 1, 1)
    {
        x = 1;
        y = 1;
        lives = 3;
        isInvincible = false;
        canJump = false;
    }
    public Player(int newX, int newY, char newSymbol, int newLives, int newSpeed, bool newIsInvincible, bool newCanJump, int gameWidth, int gameHeight, Level level) : base(newX, newY, newSymbol, newLives, 1, 1, gameWidth, gameHeight)
    {
        x = newX;
        y = newY;

        gameLevel = level;

        lives = newLives;
        isInvincible = newIsInvincible;
        canJump = newCanJump;

        width = gameWidth;
        height = gameHeight;

    }

    public override void Draw(int xOffset, int yOffset)
    {
        ConsoleColor originalColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        base.Draw(xOffset, yOffset);
        Console.ForegroundColor = originalColor;
    }

    public void DoDamage(int damage, Game game)
    {
        lives -= damage;
        if (lives <= 0)
        {
            lives = 0;
            game.CurrentGameState = GameState.GameOver;
        }
    }

    public override void Move(double distanceToMoveX, double distanceToMoveY, int screenWidth, int screenHeight)
    {
        xPos += distanceToMoveX;
        yPos += distanceToMoveY;
        if (xPos <= 0)
        {
            xPos = 1;
        }
        else if (xPos > screenWidth - 2)
        {
            xPos = screenWidth - 2;
        }
        if (yPos <= 0)
        {
            yPos = 1;
        }
        else if (yPos > screenHeight - 2)
        {
            yPos = screenHeight - 2;
        }
        else if (gameLevel.GetElementTypeAt(Convert.ToInt32(yPos), Convert.ToInt32(xPos)) == LevelElementType.Wall)
        {
            xPos -= distanceToMoveX;
            yPos -= distanceToMoveY;
        }
    }

    public void GoInvincible()
    {
        if (isInvincible)
        {
            color = ConsoleColor.Blue;
        }
        else
        {
            color = ConsoleColor.Red;
        }
    }

    public void ResetPosition()
    {
        xPos = 20;
        yPos = 10;
    }
}
