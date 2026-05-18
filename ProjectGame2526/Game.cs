using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ProjectGame2526;

public enum GameState
{
    StartingScreen,
    MainMenu,
    HighscoresMenu,
    HighscoreSaveMenu,
    GameRunning,
    GamePaused,
    GameOver,
    GameWon,
    NoGameState,
}

public class Game
{
    /**
 *     __     ___    ____  
 *     \ \   / / \  |  _ \ 
 *      \ \ / / _ \ | |_) |
 *       \ V / ___ \|  _ < 
 *        \_/_/   \_\_| \_\            
 */
    protected bool GameFirstStart = true;
    protected int width, height;
    protected int playerPosX = 5, playerPosY = 5;
    protected double refreshRate;
    protected int uiXOffset = 3;
    protected int uiYOffset = 5;
    protected int score = 0;
    protected string playerName;

    protected double timeSinceHit = 10000000;
    const double hitInvincibleTime = 0.5;
    /**
 *       ___  _     _           _       
 *      / _ \| |__ (_) ___  ___| |_ ___ 
 *     | | | | '_ \| |/ _ \/ __| __/ __|
 *     | |_| | |_) | |  __/ (__| |_\__ \
 *      \___/|_.__// |\___|\___|\__|___/
 *       __| | __|__/__| | __ _ _ __    
 *      / _` |/ _ \/ __| |/ _` | '__|   
 *     | (_| |  __/ (__| | (_| | |_     
 *      \__,_|\___|\___|_|\__,_|_(_)                                         
 */
    protected GameState currentGameState;
    protected GameState previousGameState;

    protected Player player;
    protected Level level;
    protected Zombie zombieHorizontal;
    protected Zombie zombieVertical;
    protected Sprite medKit;
    protected List<Zombie> zombies = new List<Zombie>();
    protected List<Powerup> powerupSpawns = new List<Powerup>();
    protected List<Highscores> highscoresList = new List<Highscores>();

    protected UI gameUI;
    protected UIElement uiLives, uiTime, uiLevel;

    /**
 *      __  __                  
 *     |  \/  | ___ _ __  _   _ 
 *     | |\/| |/ _ \ '_ \| | | |
 *     | |  | |  __/ | | | |_| |
 *     |_|  |_|\___|_| |_|\__,_|                      
 */
    protected Screen startingScreen;
    protected Screen gameOverScreen;
    protected Menu mainMenu;
    protected StartGameMenuItem startGameMenuItem;
    protected ExitGameMenuItem ExitGameMenuItem;
    protected HighscoresMenuItem highscoresMenuItem;
    protected HighscoresScreen highscoresScreen;
    protected HighscoreSaveScreen highscoreSaveScreen;

    /* ---------------------------------------------------------*/

    protected Stopwatch stopwatch;

    public GameState CurrentGameState
    {
        get { return currentGameState; }
        set { currentGameState = value; }
    }

    public Game(int newWidth, int newHeight)
    {
        // set the size
        width = newWidth;
        height = newHeight;

        // set the window
        Console.WindowWidth = width + 1 + uiXOffset;
        Console.WindowHeight = height + 3 / 2 + uiYOffset;
        Console.SetBufferSize(width + 1 + uiXOffset, height + 2 + uiYOffset);

        currentGameState = GameState.StartingScreen;
        previousGameState = GameState.NoGameState;

        Thread.Sleep(50);

        /**
         *       ___  _     _           _       
         *      / _ \| |__ (_) ___  ___| |_ ___ 
         *     | | | | '_ \| |/ _ \/ __| __/ __|
         *     | |_| | |_) | |  __/ (__| |_\__ \
         *      \___/|_.__// |\___|\___|\__|___/
         *               |__/                   
         */
        //---- level ----
        level = new Level(width, height, 1, uiXOffset, uiYOffset);

        //---- entities ----
        player = new Player(30, 10, '@', 3, 1, false, false, width, height, level);
        zombieHorizontal = new Zombie(5, 6, 'Z', 0, 1, 28, 0, width, height, level);
        zombieVertical = new Zombie(3, 10, 'Z', 0, 1, 0, 28, width, height, level);

        if (level.currentLevel >= 1 && level.currentLevel < 3)
        {
            zombies.Add(zombieHorizontal);
            zombies.Add(zombieVertical);
        }
        // Power-ups
        if (level.currentLevel >= 1 && level.currentLevel < 3)
        {
            Powerup medKit = new Powerup(PowerupType.MedKit, 5, 7, '+');
            powerupSpawns.Add(medKit);
        }
        //---- UI ----
        gameUI = new UI();
        uiLives = new UIElement("Lives", player.Lives, 2, 1);
        gameUI.AddUIElement(uiLives);
        uiTime = new UIElement("Time", 0, 15, 1);
        gameUI.AddUIElement(uiTime);

        uiLevel = new UIElement("Level", level.LevelNumber, 30, 1);
        gameUI.AddUIElement(uiLevel);
        //---- SCREENS ----
        startingScreen = new Screen("StartingScreen.txt", ConsoleColor.Blue, ConsoleColor.Black);
        gameOverScreen = new Screen("GameOverScreen.txt", ConsoleColor.Red, ConsoleColor.Black);
        mainMenu = new Menu("MainMenu.txt", ConsoleColor.Cyan, ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.White);
        highscoresScreen = new HighscoresScreen();
        startGameMenuItem = new StartGameMenuItem();
        highscoresMenuItem = new HighscoresMenuItem();
        ExitGameMenuItem = new ExitGameMenuItem();
        // ADDING TO LIST
        mainMenu.AddMenuItem(startGameMenuItem);
        mainMenu.AddMenuItem(highscoresMenuItem);
        mainMenu.AddMenuItem(ExitGameMenuItem);
        // STOPWATCH
        stopwatch = new Stopwatch();
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
    public int GetXOffset()
    {
        return uiXOffset;
    }
    public int GetYOffset()
    {
        return uiYOffset;
    }

    public void Draw(double newRefreshRate)
    {
        refreshRate = newRefreshRate;
        switch (currentGameState)
        {
            case GameState.StartingScreen:
                if (currentGameState != previousGameState)
                {
                    Reset();
                    startingScreen.Draw();
                }
                break;
            case GameState.MainMenu:
                if (currentGameState != previousGameState || mainMenu.DidActiveMenuItemChange())
                {
                    mainMenu.Draw();
                }
                break;
            case GameState.HighscoresMenu:
                if (currentGameState != previousGameState)
                {
                    highscoresList = highscoresScreen.LoadHighScores();
                }
                highscoresScreen.Draw(highscoresList);
                break;
            case GameState.GameRunning:
                level.Draw(uiXOffset, uiYOffset);
                player.Draw(uiXOffset, uiYOffset);
                // Zombies
                foreach (Zombie zombie in zombies)
                {
                    zombie.Draw(uiXOffset, uiYOffset);
                }
                // Power-ups
                foreach (Powerup powerup in powerupSpawns)
                {
                    powerup.Draw(uiXOffset, uiYOffset);
                }

                gameUI.Draw();
                break;
            case GameState.GameOver:
                stopwatch.Stop();
                stopwatch.Reset();
                if (currentGameState != previousGameState)
                {
                    FullScreenReset();
                    gameOverScreen.Draw();
                    UpdateHighscore();
                    player.Lives = player.DefaultPlayerLives;
                }
                break;
        }
        previousGameState = currentGameState;
    }
    public void Reset()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Black;
        int gameUiYOffset = GetYOffset();
        int gameUiXOffset = GetXOffset();
        Console.SetCursorPosition(gameUiXOffset, gameUiYOffset);
        for (int y = 0; y < GetHeight(); ++y)
        {
            for (int x = 0; x < GetWidth(); ++x)
            {
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        Console.SetCursorPosition(0, 0);
    }
    public void FullScreenReset()
    {
        Console.SetCursorPosition(0, 0);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Black;
        for (int i = 0; i < Console.WindowWidth; i++)
        {
            for (int j = 0; j < Console.WindowHeight; j++)
            {
                Console.Write(" ");
            }
        }
        Console.SetCursorPosition(0, 0);
    }


    public void OnInput(ConsoleKey key)
    {
        switch (currentGameState)
        {
            case GameState.StartingScreen:
                if (key == ConsoleKey.Spacebar)
                {
                    FullScreenReset();
                    currentGameState = GameState.MainMenu;
                }
                break;
            case GameState.MainMenu:
                if (key == ConsoleKey.Spacebar)
                {
                    FullScreenReset();
                    mainMenu.ActivateSelectedMenuItem(this); // This is de game class
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    mainMenu.SelectPreviousItem();
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    mainMenu.SelectNextItem();
                }
                break;
            case GameState.HighscoresMenu:
                if (key == ConsoleKey.Backspace)
                {
                    FullScreenReset();
                    currentGameState = GameState.MainMenu;
                }
                highscoresScreen.OnInput(key, highscoresList);
                if(key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
                {
                    FullScreenReset();
                }
                break;
            case GameState.GameRunning:
                if (key == ConsoleKey.LeftArrow)
                {
                    player.Move(-1, 0, width, height);
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    player.Move(1, 0, width, height);
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    player.Move(0, 1, width, height);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    player.Move(0, -1, width, height);
                }
                break;
            case GameState.GameOver:
                if (key == ConsoleKey.Spacebar)
                {
                    FullScreenReset();
                    currentGameState = GameState.MainMenu;
                }
                break;
        }
    }

    public void Update(double dt)
    {
        switch (currentGameState)
        {
            case GameState.StartingScreen:
                break;
            case GameState.GameRunning:
                // Update zombies
                foreach (Zombie zombie in zombies)
                {
                    zombie.Update(dt);
                    HandleCollision(dt);
                }

                stopwatch.Start();
                gameUI.UpdateUIElementValue("Time", (int)stopwatch.ElapsedMilliseconds / 1000);
                gameUI.UpdateUIElementValue("Lives", player.Lives);

                checkStopwatch();
                break;
        }
    }

    public void HandleCollision(double dt)
    {
        timeSinceHit += dt;
        // check if colliding with an enemy
        foreach (Zombie zombie in zombies)
        {
            if (zombie.CursorX == player.CursorX && zombie.CursorY == player.CursorY && timeSinceHit > hitInvincibleTime)
            {
                player.DoDamage(zombie.AttackDamage, this);
                timeSinceHit = 0;
            }
        }
        foreach (Powerup powerup in powerupSpawns)
        {
            if (powerup.CursorX == player.CursorX && powerup.CursorY == player.CursorY)
            {
                PowerupType type = powerup.GetPowerupType();
                PowerupActivation(type);
                powerupSpawns.Remove(powerup);
                break;
            }
        }
    }

    // ----- POWERUP HANDLER -----

    public void PowerupActivation(PowerupType type)
    {
        if (type == PowerupType.Invincibility)
        {
            player.IsInvincible = true;
            player.GoInvincible();
        }
        else if (type == PowerupType.JumpBoost)
        {
            // Implement jump boost effect
        }
        else if (type == PowerupType.MedKit)
        {
            player.Lives++;
        }
        else if (type == PowerupType.Bomb)
        {
            // Implement bomb effect
        }
    }

    // --------------------------------
    // CHECK STOPWATCH
    public void checkStopwatch()
    {
        if (stopwatch.ElapsedMilliseconds >= 10000)
        {
            level.currentLevel++;
            stopwatch.Stop();
            gameUI.UpdateUIElementValue("Time", 0);
            gameUI.UpdateUIElementValue("Level", level.currentLevel);
            stopwatch.Reset();
            level = new Level(width, height, level.currentLevel, uiXOffset, uiYOffset);
            level.Draw(uiXOffset, uiYOffset);
            player.ResetPosition();
            Draw(refreshRate);
            // Add new zombies or power-ups for the next level
            if (level.currentLevel == 2)
            {
                zombies.Add(new Zombie(10, 5, 'Z', 0, 1, 28, 0, width, height, level));
                powerupSpawns.Add(new Powerup(PowerupType.Invincibility, 15, 10, 'I'));
            }
        }
    }

    public void UpdateHighscore()
    {
        List<Highscores> highscoresList =
            highscoresScreen.LoadHighScores();

        playerName = highscoreSaveScreen.GetPlayerName();

        highscoresList.Add(
            new Highscores(score, playerName)
        );

        highscoresScreen.SaveHighScores(highscoresList);
    }
}