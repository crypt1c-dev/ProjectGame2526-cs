using System;

namespace ProjectGame2526;

public class HighscoreSaveScreen : Screen
{
    protected string playerName;
    public bool overwriteExistingScore = false;
    public bool exitEnabled = false;
    public HighscoreSaveScreen() : base("HighscoresSaveMenu.txt", ConsoleColor.White, ConsoleColor.Black)
    {
    }

    public void Draw(Game game)
    {
        base.Draw();

        // Here you would add code to save the player's name and score to a file or database
        HandleNameInput(game);
    }

    public void HandleNameInput(Game game)
    {
        bool validInput = false;
        bool isFirstInput = true;
        do
        {
            if (isFirstInput)
            {
                Console.Write("Enter your name to save your score: ");
                playerName = Console.ReadLine();

                isFirstInput = false;
            }
            else
            {
                if (playerName == null || playerName.Trim() == "")
                {
                    Console.Write("Name cannot be empty. Please enter a valid name: ");
                    playerName = Console.ReadLine();
                }
                else
                {
                    validInput = true;
                }
            }
        }
        while (!validInput);

        StreamReader reader = new StreamReader("Highscores.json");
        string highscoresAsText;
        highscoresAsText = reader.ReadToEnd();
        reader.Close();

        if (highscoresAsText.Contains(playerName))
        {
            Console.WriteLine("Name already exists. Do you want to overwrite the existing score? (Y/N)");
            string choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                overwriteExistingScore = true;
                Console.WriteLine("Your score will be overwritten. Return to main menu by pressing [SPACE].");
                exitEnabled = true;
            }
            else if (choice.ToUpper() == "N")
            {
                Console.WriteLine("Do you want to change your name? (Y/N)");
                string changeNameChoice = Console.ReadLine();
                if (changeNameChoice.ToUpper() == "Y")
                {
                    HandleNameInput(game);
                }
                else
                {
                    Console.WriteLine("Your score will be forgotten. Return to main menu by pressing [SPACE].");
                    exitEnabled = true;
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Your score will be forgotten. Return to main menu by pressing [SPACE].");
                exitEnabled = true;
                game.CurrentGameState = GameState.MainMenu;
            }
        }
        else
        {
            Console.WriteLine("Your score has been saved. Return to main menu by pressing [SPACE].");
            exitEnabled = true;
        }
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}
