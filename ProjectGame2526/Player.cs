using System;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

namespace ProjectGame2526;

public class Player : Entity
{
    protected bool isInvincible = false;
    protected ConsoleColor color = ConsoleColor.Red;
    protected Level gameLevel;
    protected int defaultPlayerLives = 3;
    protected Stopwatch time;
    public bool IsInvincible
    {
        get { return isInvincible; }
        set { isInvincible = value; }
    }
    public int DefaultPlayerLives
    {
        get { return defaultPlayerLives; }
        set { defaultPlayerLives = value; }
    }
    public Player() : base(1, 1, '@', 3, 1, 1)
    {
        lives = 3;
        isInvincible = false;
    }
    public Player(int newX, int newY, char newSymbol, int newLives, int newSpeed, bool newIsInvincible, bool newCanJump, int gameWidth, int gameHeight, Level level) : base(newX, newY, newSymbol, newLives, 1, 1, gameWidth, gameHeight)
    {
        gameLevel = level;

        lives = newLives;
        isInvincible = newIsInvincible;

        time = new Stopwatch();

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
        CheckDeath(game);

    }
    public void CheckDeath(Game game)
    {
        if (lives <= 0)
        {
            lives = 0;
            game.CurrentGameState = GameState.GameOver;
        }
    }

    public override void Move(double distanceToMoveX, double distanceToMoveY,
    int screenWidth, int screenHeight)
    {
        xPos += distanceToMoveX;
        yPos += distanceToMoveY;

        // screen borders
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

        // wall collision
        // debug
        Console.SetCursorPosition(0, 25);

        Console.Write(
            gameLevel.GetElementTypeAt((int)yPos, (int)xPos)
        );

        // collision
        if (gameLevel.GetElementTypeAt((int)yPos, (int)xPos)
            == LevelElementType.Wall && !isInvincible || gameLevel.GetElementTypeAt((int)yPos, (int)xPos)
            == LevelElementType.BreakableWall && !isInvincible)
        {
            xPos -= distanceToMoveX;
            yPos -= distanceToMoveY;
        }
    }

    public void GoInvincible()
    {
        if (isInvincible)
        {
            UpdateInvincible();
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
    public void SetLevel(Level newLevel)
    {
        gameLevel = newLevel;
    }

    public void UpdateInvincible()
    {
        if (isInvincible)
        {
            time.Start();
            if (time.ElapsedMilliseconds >= 10000)
            {
                isInvincible = false;
                GoInvincible();
            }
            else if (time.ElapsedMilliseconds < 10000)
            {
                color = ConsoleColor.Blue;
            }
        }
    }
}
