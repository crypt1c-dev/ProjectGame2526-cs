using System;

namespace ProjectGame2526;

public class HighscoreSaveScreen : Screen
{
    string playerName;
    public HighscoreSaveScreen() : base("HighscoresSaveMenu.txt", ConsoleColor.White, ConsoleColor.Black)
    {
    }

    public void Draw()
    {
        Console.WriteLine("Enter your name to save your score:");
        playerName = Console.ReadLine();
        // Here you would add code to save the player's name and score to a file or database


    }

    public string GetPlayerName()
    {
        

        return playerName;
    }
}
