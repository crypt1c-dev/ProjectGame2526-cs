using System;

namespace ProjectGame2526;

public class Sprite
{
    protected double xPos;
    protected double yPos;
    protected char symbol;
    protected int gameWidth;
    protected int gameHeight;
    public double XPos
    {
        get { return xPos; }
        set { xPos = value; }
    }
    public double YPos
    {
        get { return yPos; }
        set { yPos = value; }
    }
    public int CursorX
    {
        get { return Convert.ToInt32(xPos); }
    }
    public int CursorY
    {
        get { return Convert.ToInt32(yPos); }
    }
    public char Symbol
    {
        get { return symbol; }
        set { symbol = value; }
    }
    public Sprite()
    {
        xPos = 0;
        yPos = 0;
        symbol = '?';
    }
    public Sprite(double newX, double newY, char newSymbol)
    {
        xPos = newX;
        yPos = newY;
        symbol = newSymbol;
    }
    public Sprite(double newX, double newY, char newSymbol, int newGameWidth, int newGameHeight)
    {
        xPos = newX;
        yPos = newY;
        symbol = newSymbol;
        gameWidth = newGameWidth;
        gameHeight = newGameHeight;
    }

    public void Move(double distanceToMoveX, double distanceToMoveY, int screenWidth, int screenHeight)
    {
        xPos += distanceToMoveX;
        if (xPos < 0)
        {
            xPos = 0;
        }
        else if (xPos > screenWidth)
        {
            xPos = screenWidth - 1;
        }
        if (yPos < 0)
        {
            yPos = 0;
        }
        else if (yPos > screenHeight)
        {
            yPos = screenHeight - 1;
        }
    }
    public virtual void Draw(int xOffset, int yOffset)
    {
        Console.SetCursorPosition(CursorX + xOffset, CursorY + yOffset);
        Console.Write(symbol);
    }

    public virtual void Update(double dt)
    {

    }
}
