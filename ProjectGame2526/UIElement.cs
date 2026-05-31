using System;

namespace ProjectGame2526;

public class UIElement
{
    protected string name;
    protected int elementValue;
    protected int xPos, yPos;

    public UIElement(string newName, int newValue, int newXPos, int newYPos)
    {
        name = newName;
        elementValue = newValue;
        xPos = newXPos;
        yPos = newYPos;
    }
    public UIElement(string newName, int newXPos, int newYPos)
    {
        name = newName;
        xPos = newXPos;
        yPos = newYPos;
    }

    public string Name
    {
        get { return name; }
    }
    public int ElementValue
    {
        get { return elementValue; }
        set { elementValue = value; }
    }
    public void Draw()
    {
        if (name == "exit")
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(xPos, yPos);
            Console.Write("Press [ESCAPE] to end the game (NO SAVE)");
        }
        else
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(name + ": " + elementValue);
        }
    }
}
