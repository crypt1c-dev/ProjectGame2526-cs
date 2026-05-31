using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace ProjectGame2526;

public class RulesScreen : Screen
{
    int scrollOffset = 0;
    public RulesScreen() : base("RulesMenu.txt", ConsoleColor.Magenta, ConsoleColor.Black)
    {

    }

    public void Draw(List<string> rules)
    {
        ///// RULES LIST
        rules.Add("=-=-= LEVEL =-=-=");
        rules.Add("- Each level is different. After 20 seconds \n   the level will be completed.");
        rules.Add("=-=-= DAMAGE =-=-=");
        rules.Add("- A zombie displayed as 'Z' will remove \n   1 live");
        rules.Add("- A zombie/tank displayed as 'T' \n   will remove 2 lives");
        rules.Add("=-=-= POWERUPS =-=-=");
        rules.Add("- A powerup marked as '+' will increase \n   1 live");
        rules.Add("- A powerup marked as 'B' will destroy walls \n   marked as '='");
        rules.Add("- A powerup marked as 'I' will make you invincible \n   and able to walk trough walls and avoid enemies\n   for 10 seconds");
        rules.Add("");
        rules.Add("- A powerup marked as 'O' is a coin and gains you \n   50 points");
        base.Draw();

        Console.ForegroundColor = ConsoleColor.White;

        // INFO TEXT (vaste UI, dus NIET in de loop)
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.SetCursorPosition(44, 2);
        Console.Write("Use up/down arrow");
        Console.SetCursorPosition(44, 3);
        Console.Write("To see more");
        Console.SetCursorPosition(44, 5);
        Console.Write("Press [BACKSPACE]");
        Console.SetCursorPosition(44, 6);
        Console.Write("To return to menu");
        // RESET COLOR
        Console.ForegroundColor = ConsoleColor.White;

        int startY = 10;

        // Amount of rules visible
        int visibleRules = 5;

        //

        for (int i = 0; i < visibleRules; i++)
        {
            int ruleIndex = i + scrollOffset;

            // stop als er geen scores meer zijn
            if (ruleIndex >= rules.Count)
            {
                break;
            }

            string rule = rules[ruleIndex];

            int yPosition = startY + (i * 3);

            Console.SetCursorPosition(1, yPosition);
            Console.Write(rule);
        }
    }

    public void OnInput(ConsoleKey key, List<string> rules)
    {
        if (key == ConsoleKey.DownArrow)
        {
            if (scrollOffset < rules.Count - 1)
            {
                scrollOffset++;
            }
        }

        if (key == ConsoleKey.UpArrow)
        {
            if (scrollOffset > 0)
            {
                scrollOffset--;
            }
        }
    }
}
