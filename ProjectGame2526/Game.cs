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
    Rules,
    HighscoresMenu,
    HighscoreSaveMenu,
    GameRunning,
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
    protected bool gameFirstStart = true;
    protected int width, height;
    protected double refreshRate;
    protected int uiXOffset = 3;
    protected int uiYOffset = 5;
    protected int score = 0;
    protected string playerName;

    protected double timeSinceHit = 10000000;
    protected const double hitInvincibleTime = 0.5;
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
    protected List<string> rulesList = new List<string>();

    protected UI gameUI;
    protected UIElement uiLives, uiTime, uiScore, uiLevel, uiExit;

    /**
 *      __  __                  
 *     |  \/  | ___ _ __  _   _ 
 *     | |\/| |/ _ \ '_ \| | | |
 *     | |  | |  __/ | | | |_| |
 *     |_|  |_|\___|_| |_|\__,_|                      
 */
    protected Screen startingScreen;
    protected Screen gameWonScreen;
    protected Screen gameOverScreen;
    protected Menu mainMenu;
    protected StartGameMenuItem startGameMenuItem;
    protected ExitGameMenuItem exitGameMenuItem;
    protected HighscoresMenuItem highscoresMenuItem;
    protected RulesMenuItem rulesMenuItem;
    protected RulesScreen rulesScreen;
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
        if (level.currentLevel == 1)
        {
            Powerup medKit = new Powerup(PowerupType.MedKit, 5, 7, '+');
            powerupSpawns.Add(medKit);
        }
        levelPropHandler();
        //---- UI ----
        gameUI = new UI();
        uiLives = new UIElement("Lives", player.Lives, 2, 1);
        gameUI.AddUIElement(uiLives);
        uiTime = new UIElement("Time", 0, 15, 1);
        gameUI.AddUIElement(uiTime);
        uiScore = new UIElement("Score", score, 25, 1);
        gameUI.AddUIElement(uiScore);

        uiLevel = new UIElement("Level", level.currentLevel, 40, 1);
        gameUI.AddUIElement(uiLevel);

        uiExit = new UIElement("exit", 2, 2);
        gameUI.AddUIElement(uiExit);
        //---- SCREENS ----
        startingScreen = new Screen("StartingScreen.txt", ConsoleColor.Blue, ConsoleColor.Black);
        gameOverScreen = new Screen("GameOverScreen.txt", ConsoleColor.Red, ConsoleColor.Black);
        gameWonScreen = new Screen("GameWonScreen.txt", ConsoleColor.Green, ConsoleColor.Black);
        mainMenu = new Menu("MainMenu.txt", ConsoleColor.Cyan, ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.White);
        highscoresScreen = new HighscoresScreen();
        rulesScreen = new RulesScreen();
        highscoreSaveScreen = new HighscoreSaveScreen();
        startGameMenuItem = new StartGameMenuItem();
        highscoresMenuItem = new HighscoresMenuItem();
        rulesMenuItem = new RulesMenuItem();
        exitGameMenuItem = new ExitGameMenuItem();
        // ADDING TO LIST
        mainMenu.AddMenuItem(startGameMenuItem);
        mainMenu.AddMenuItem(highscoresMenuItem);
        mainMenu.AddMenuItem(rulesMenuItem);
        mainMenu.AddMenuItem(exitGameMenuItem);
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
            case GameState.Rules:
                rulesScreen.Draw(rulesList);
                break;
            case GameState.HighscoresMenu:
                if (currentGameState != previousGameState)
                {
                    highscoresList = highscoresScreen.LoadHighScores();
                }
                highscoresScreen.Draw(highscoresList);
                break;
            case GameState.HighscoreSaveMenu:
                if (currentGameState != previousGameState)
                {
                    FullScreenReset();
                    highscoreSaveScreen.Draw(this);
                    if (!highscoreSaveScreen.wantsToExit)
                    {
                        highscoreSaveScreen.GetPlayerName();
                        UpdateHighscore();
                    }
                    else
                    {
                        FullScreenReset();
                        ResetGameStats();
                        currentGameState = GameState.MainMenu;
                        return;
                    }
                }
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
            case GameState.GameWon:
                stopwatch.Stop();
                stopwatch.Reset();
                if (currentGameState != previousGameState)
                {
                    FullScreenReset();
                    gameWonScreen.Draw();
                    player.Lives = player.DefaultPlayerLives;
                }
                break;
            case GameState.GameOver:
                stopwatch.Stop();
                stopwatch.Reset();
                if (currentGameState != previousGameState)
                {
                    FullScreenReset();
                    gameOverScreen.Draw();
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
                if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
                {
                    FullScreenReset();
                }
                break;
            case GameState.HighscoreSaveMenu:
                if (key == ConsoleKey.Spacebar && highscoreSaveScreen.exitEnabled)
                {
                    FullScreenReset();
                    currentGameState = GameState.MainMenu;
                    ResetGameStats();
                }
                else if (highscoreSaveScreen.wantsToExit)
                {
                    FullScreenReset();
                    currentGameState = GameState.MainMenu;
                    ResetGameStats();
                }
                break;
            case GameState.Rules:
                if (key == ConsoleKey.Backspace)
                {
                    FullScreenReset();
                    currentGameState = GameState.MainMenu;
                }
                rulesScreen.OnInput(key, rulesList);
                if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
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
                // check if the +=~ key is pressed
                if (key == ConsoleKey.OemPlus)
                {
                    NextLevel();
                }
                if (key == ConsoleKey.End)
                {
                    player.Lives = 0;
                    player.CheckDeath(this);
                }
                if(key == ConsoleKey.Home)
                {
                    score += 10000;
                    level.SetCurrentLevel(10);
                    NextLevel();
                }
                if (key == ConsoleKey.Escape)
                {
                    FullScreenReset();
                    currentGameState = GameState.MainMenu;
                    ResetGameStats();
                }
                break;
            case GameState.GameWon:
                if (key == ConsoleKey.Spacebar)
                {
                    FullScreenReset();
                    currentGameState = GameState.HighscoreSaveMenu;
                }
                break;
            case GameState.GameOver:
                if (key == ConsoleKey.Spacebar)
                {
                    FullScreenReset();
                    currentGameState = GameState.HighscoreSaveMenu;
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
                gameUI.UpdateUIElementValue("Score", score);

                checkStopwatch();

                player.UpdateInvincible();
                break;
        }
    }

    public void HandleCollision(double dt)
    {
        timeSinceHit += dt;
        // check if colliding with an enemy
        foreach (Zombie zombie in zombies)
        {
            if (zombie.CursorX == player.CursorX && zombie.CursorY == player.CursorY && timeSinceHit > hitInvincibleTime && !player.IsInvincible)
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
            score += 20;
        }
        else if (type == PowerupType.Coin)
        {
            Console.Beep(1000, 80);
            Console.Beep(1333, 20);
            score += 50;
        }
        else if (type == PowerupType.MedKit)
        {
            player.Lives++;
            score += 10;
        }
        else if (type == PowerupType.Bomb)
        {
            level.DestroyBreakableWalls((int)player.XPos, (int)player.YPos, 4);
            score += 80;
        }
    }

    // --------------------------------
    // CHECK STOPWATCH
    public void checkStopwatch()
    {
        if (stopwatch.ElapsedMilliseconds >= 20000)
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        level.currentLevel++;

        score += 100;

        stopwatch.Stop();
        stopwatch.Reset();

        FullScreenReset();
        Console.ForegroundColor = ConsoleColor.White;

        if (level.currentLevel == 11)
        {
            currentGameState = GameState.GameWon;
            return;
        }

        Console.WriteLine("Level Complete!");
        Console.WriteLine("Loading level: " + level.currentLevel + "...");
        Thread.Sleep(500);
        FullScreenReset();
        gameUI.Draw();
        gameUI.UpdateUIElementValue("Time", 0);
        gameUI.UpdateUIElementValue("Level", level.currentLevel);

        level = new Level(width, height, level.currentLevel, uiXOffset, uiYOffset);

        player.SetLevel(level);

        level.Draw(uiXOffset, uiYOffset);

        player.ResetPosition();
        if (level.currentLevel == 9 || level.currentLevel == 10)
        {
            player.XPos = 21;
            player.YPos = 11;
        }
        levelPropHandler();
        Draw(refreshRate);
    }

    public void levelPropHandler()
    {
        Zombie z1 = new Zombie(10, 5, 'Z', 0, 1, 28, 0, width, height, level);
        Zombie z2 = new Zombie(34, 5, 'Z', 0, 1, 14, 28, width, height, level);
        Zombie tankMiddle = new Zombie(40, 9, 'T', 1, 2, 4, 0, width, height, level);
        Zombie tankBottom = new Zombie(30, 17, 'T', 1, 2, 28, 0, width, height, level);
        if (level.currentLevel == 1)
        {
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 11, 11, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 39, 14, 'O'));
        }

        if (level.currentLevel == 2)
        {
            zombies.Add(z1);
            powerupSpawns.Add(new Powerup(PowerupType.Invincibility, 15, 10, 'I'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 29, 16, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 40, 18, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 48, 3, 'O'));
        }
        else if (level.currentLevel == 3)
        {
            zombies.Add(z2);
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 10, 4, 'O'));
        }
        else if (level.currentLevel == 4)
        {
            zombies.Add(tankMiddle);
            powerupSpawns.Add(new Powerup(PowerupType.Bomb, 41, 11, 'B'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 13, 2, 'O'));
        }
        else if (level.currentLevel == 5)
        {
            zombies.Add(tankBottom);
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 24, 11, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 13, 8, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 44, 2, 'O'));
        }
        else if (level.currentLevel == 6)
        {
            zombies.Remove(tankMiddle);
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 31, 17, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 7, 2, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 24, 3, 'O'));

        }
        else if (level.currentLevel == 8)
        {
            zombies.Remove(z1);
            powerupSpawns.Add(new Powerup(PowerupType.Bomb, 32, 18, 'B'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 34, 16, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 13, 9, 'O'));
        }
        else if (level.currentLevel == 9)
        {
            zombies.Remove(z1);
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 13, 2, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 48, 16, 'O'));
        }
        else if (level.currentLevel == 10)
        {
            powerupSpawns.Add(new Powerup(PowerupType.Bomb, 24, 11, 'B'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 11, 13, 'O'));
            powerupSpawns.Add(new Powerup(PowerupType.Coin, 48, 7, 'O'));
        }
        // Updates de level object for every zombie object to make sure the zombies 
        // follow the same rules in every different level
        foreach (Zombie zombie in zombies)
        {
            zombie.SetLevel(level);
        }

        level.CheckErrorPositions(powerupSpawns, zombies);
    }

    public void UpdateHighscore()
    {
        Highscores existingHighscore = null;
        List<Highscores> highscoresList =
            highscoresScreen.LoadHighScores();

        playerName = highscoreSaveScreen.GetPlayerName();

        // remove whitespace from the beginning and end of the name
        playerName = playerName.Trim();

        // zoek of de naam al bestaat
        foreach (Highscores highscore in highscoresList)
        {
            if (highscore.PlayerName.Trim() == playerName)
            {
                existingHighscore = highscore;
            }
        }
        if (level.currentLevel == 11)
        {
            level.currentLevel = 10;
        }

        // if the name doesn't exist, add a new highscore with the current name, score and level
        if (existingHighscore == null)
        {
            highscoresList.Add(
                new Highscores(score, playerName, level.currentLevel)
            );
        }
        else
        {
            if (highscoreSaveScreen.overwriteExistingScore)
            {
                existingHighscore.Score = score;
                existingHighscore.LevelNum = level.currentLevel;
            }
        }

        highscoresScreen.SaveHighScores(highscoresList);
    }

    public void ResetGameStats()
    {
        bool isResettingObjects = true;

        player.Lives = player.DefaultPlayerLives;
        level.SetCurrentLevel(1);
        score = 0;

        stopwatch.Stop();
        stopwatch.Reset();

        level = new Level(width, height, 1, uiXOffset, uiYOffset);
        player.ResetPosition();

        gameUI = new UI();
        uiLives = new UIElement("Lives", player.Lives, 2, 1);
        gameUI.AddUIElement(uiLives);
        uiTime = new UIElement("Time", 0, 15, 1);
        gameUI.AddUIElement(uiTime);
        uiScore = new UIElement("Score", score, 25, 1);
        gameUI.AddUIElement(uiScore);
        uiLevel = new UIElement("Level", level.currentLevel, 40, 1);
        gameUI.AddUIElement(uiLevel);

        uiExit = new UIElement("exit", 2, 2);
        gameUI.AddUIElement(uiExit);

        while (isResettingObjects)
        {
            if (level.currentLevel >= 1)
            {
                if (zombies.Count > 2)
                {
                    zombies.RemoveAt(2);
                }
                else
                {
                    isResettingObjects = false;
                }
            }
        }

        if (level.currentLevel == 1)
        {
            powerupSpawns.Clear();
            Powerup medKit = new Powerup(PowerupType.MedKit, 5, 7, '+');
            powerupSpawns.Add(medKit);
        }
    }
}