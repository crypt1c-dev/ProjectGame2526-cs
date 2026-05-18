using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace ProjectGame2526;


public class Level
{
    int playerLives;
    public int currentLevel = 1;
    int height;
    int width;
    int newXOffset;
    int newYOffset;
    protected LevelElement[,] level;


    public int PlayerLives
    {
        get { return playerLives; }
        set { playerLives = value; }
    }
    public int LevelNumber
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }

    public Level(int newWidth, int newHeight, int currentLevelNumber, int xOffset, int yOffset)
    {
        width = newWidth;
        height = newHeight;
        currentLevel = currentLevelNumber;

        level = new LevelElement[width, height];

        int length1 = level.GetLength(0);
        int length2 = level.GetLength(1);

        for (int row = 0; row < level.GetLength(1); row++)
        {
            for (int col = 0; col < level.GetLength(0); col++)
            {
                if (row == 0 || row == height - 1 || col == 0 || col == width - 1)
                {
                    level[col, row] = new LevelElement(LevelElementType.Wall);
                }
                else if (row == 3 && col > 23 && col < 32 && currentLevelNumber == 1)
                {
                    level[col, row] = new LevelElement(LevelElementType.Wall);
                }
                // Check level 2 specific walls
                else if (currentLevelNumber == 2)
                {
                    if (row == 3 && col > 23 && col < 32 || row > 4 && row < 10 && col == 11)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                // Check level 3 specific walls
                else if (currentLevelNumber == 3)
                {
                    if (row > 4 && row < 10 && col == 11 || row == 5 && col > 11 && col < 30)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                else
                {
                    level[col, row] = new LevelElement(LevelElementType.Empty);
                }
            }
        }
    }

    public void Draw(int xOffset, int yOffset)
    {
        // newXOffset = xOffset;
        // newYOffset = yOffset;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        for (int row = 0; row < level.GetLength(1); row++)
        {
            for (int col = 0; col < level.GetLength(0); col++)
            {
                level[col, row].Draw(col + xOffset, row + yOffset);
            }
            Console.WriteLine();
        }
    }

    public LevelElementType GetElementTypeAt(int row, int col)
    {
        //Level 1
        if (row == newYOffset || row >= height + newYOffset || col == newXOffset || col >= width + newXOffset)
        {
            return LevelElementType.Wall;
        }
        return level[col, row].Type;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int newLevel)
    {
        currentLevel = newLevel;
    }
    public void SetPlayerLives(int newPlayerLives)
    {
        playerLives = newPlayerLives;
    }
    public void LoadNextLevel()
    {
        currentLevel++;
    }
}
