using System;

namespace ProjectGame2526;

public class Entity : Sprite
{
    protected int lives;
    protected double xSpeed;
    protected double ySpeed;
    private int width;
    private int height;
    public int Lives
    {
        get { return lives; }
        set { lives = value; }
    }


    // public virtual void Move(int x, int y)
    // {
    //     xPos += (int)(x * speed);
    //     yPos += (int)(y * speed);
    // }

    public Entity() : base(1, 1, '?')
    {
        lives = 3;

    }
    public Entity(int newX, int newY, char newSymbol, int newLives, double newXSpeed, double newYSpeed) : base(newX, newY, newSymbol)
    {
        lives = newLives;
        xSpeed = newXSpeed;
        ySpeed = newYSpeed;

    }
    public Entity(int newX, int newY, char newSymbol, int newLives, double newXSpeed, double newYSpeed, int gameWidth, int gameHeight) : base(newX, newY, newSymbol, gameWidth, gameHeight)
    {
        xSpeed = newXSpeed;
        ySpeed = newYSpeed;
        lives = newLives;

        width = gameWidth;
        height = gameHeight;
    }

    public override void Update(double dt)
    {
        Move(xSpeed * dt, ySpeed * dt, width, height);
    }

    public virtual void Move(double distanceToMoveX, double distanceToMoveY, int screenWidth, int screenHeight)
    {
        xPos += distanceToMoveX;
        yPos += distanceToMoveY;
        if (xPos < 0)
        {
            xPos = 0;
        }
        else if (xPos > screenWidth - 1)
        {
            xPos = screenWidth - 2;
        }
        if (yPos < 0)
        {
            yPos = 0;
        }
        else if (yPos > screenHeight - 1)
        {
            yPos = screenHeight - 2;
        }
    }
}
