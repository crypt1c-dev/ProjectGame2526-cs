using System;

namespace ProjectGame2526;

public enum LevelElementType
{
    Empty,
    Wall,
    BreakableWall
}
public class LevelElement
{
    protected LevelElementType type;
    public LevelElementType Type
    {
        get { return type; }
        set { type = value; }
    }
    public LevelElement()
    {
        type = LevelElementType.Empty;
    }
    public LevelElement(LevelElementType newType)
    {
        type = newType;
    }
    public void Draw(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        switch (type)
        {
            case LevelElementType.Empty:
                Console.Write(" ");
                break;
            case LevelElementType.Wall:
                Console.Write("#");
                break;
            case LevelElementType.BreakableWall:
                Console.Write("=");
                break;
        }
    }
}
