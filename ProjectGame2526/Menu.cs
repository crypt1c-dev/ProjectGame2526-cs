using System;

namespace ProjectGame2526;

public class Menu : Screen
{
    /*
    MAIN MENU

    Start game
    Highscores
    Exit
    */

    protected List<MenuItem> menuItems;
    protected int activeMenuItemID;
    protected int previousActiveMenuItemID;
    protected ConsoleColor activeItemForegroundColor;
    protected ConsoleColor activeItemBackgroundColor;

    public Menu(string filePath)
        : base(filePath)
    {
        menuItems = new List<MenuItem>();
        activeMenuItemID = 0;
        activeItemForegroundColor = ConsoleColor.Black;
        activeItemBackgroundColor = ConsoleColor.White;
    }

    public Menu(string filePath,
                ConsoleColor newForeGroundColor,
                ConsoleColor newBackGroundColor,
                ConsoleColor newActiveItemForegroundColor,
                ConsoleColor newActiveItemBackgroundColor)
    : base(filePath, newForeGroundColor, newBackGroundColor)
    {
        menuItems = new List<MenuItem>();
        activeMenuItemID = 0;
        activeItemForegroundColor = newActiveItemForegroundColor;
        activeItemBackgroundColor = newActiveItemBackgroundColor;
    }

    public void AddMenuItem(MenuItem newMenuItem)
    {
        menuItems.Add(newMenuItem);
    }

    public void SelectNextItem()
    {
        if (activeMenuItemID == menuItems.Count - 1)
        {
            activeMenuItemID = 0;
        }
        else
        {
            activeMenuItemID++;
        }
    }
    public void SelectPreviousItem()
    {
        if (activeMenuItemID == 0)
        {
            activeMenuItemID = menuItems.Count - 1;
        }
        else
        {
            activeMenuItemID--;
        }
    }

    public bool DidActiveMenuItemChange()
    {
        return activeMenuItemID != previousActiveMenuItemID;
    }

    public void ActivateSelectedMenuItem(Game game)
    {
        menuItems[activeMenuItemID].Activate(game);
    }

    public override void Draw()
    {
        base.Draw();

        for (int i = 0; i < menuItems.Count; i++)
        {
            Console.Write("\t");
            if (i == activeMenuItemID)
            {
                Console.ForegroundColor = activeItemForegroundColor;
                Console.BackgroundColor = activeItemBackgroundColor;
                menuItems[i].Draw();
                Console.ForegroundColor = foreGroundColor;
                Console.BackgroundColor = backGroundColor;
            }
            else
            {
                Console.ForegroundColor = foreGroundColor;
                Console.BackgroundColor = backGroundColor;
                menuItems[i].Draw();
            }
            Console.Write("\n\n");
        }

        previousActiveMenuItemID = activeMenuItemID;
    }
}
