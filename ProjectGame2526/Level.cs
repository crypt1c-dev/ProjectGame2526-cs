using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace ProjectGame2526;


public class Level
{
    protected int playerLives;
    public int currentLevel = 1;
    protected int height;
    protected int width;
    protected int newXOffset;
    protected int newYOffset;
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
                else if (currentLevelNumber == 1)
                {
                    // small stroke of walls at row 3
                    if (row == 3 && col > 23 && col < 32
                   || row == 17 && col > 43 && col < 55
                   || col == 54 && row > 10 && row <= 17
                   || row == 14 && col == 12
                   || row == 13 && col == 13
                   || row == 12 && col == 14
                   || row == 11 && col == 15
                   || row == 10 && col == 16)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                // Check level 2 specific walls
                else if (currentLevelNumber == 2)
                {
                    if (row == 3 && col > 23 && col < 32
                    || row > 4 && row < 10 && col == 11
                    || row == 17 && col > 43 && col < 55
                   || col == 54 && row > 10 && row <= 17
                   || row == 14 && col == 12
                   || row == 13 && col == 13
                   || row == 12 && col == 14
                   || row == 11 && col == 15
                   || row == 10 && col == 16)
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
                    if (row > 4 && row < 10 && col == 11
                    || row == 5 && col > 11 && col < 30
                    || row == 11 && col > 43 && col < 55
                    || col == 54 && row < 17 && row > 10)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                else if (currentLevelNumber == 4)
                {
                    if (row > 4 && row < 10 && col == 11
                    || row == 5 && col > 11 && col < 30)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else if (col > 34 && col < 48 && row == 10)
                    {
                        level[col, row] = new LevelElement(LevelElementType.BreakableWall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                else if (currentLevelNumber == 5)
                {
                    if (row > 4 && row < 10 && col == 11
                    || row == 5 && col > 11 && col < 30
                    || row == 14 && col == 12
                    || row == 15 && col == 13
                    || row == 16 && col == 14
                    || row == 17 && col == 15
                    || row == 18 && col == 16
                    || row == 14 && col == 34
                    || row == 13 && col == 35
                    || row == 12 && col == 36
                    || row == 11 && col == 37
                    || row == 10 && col == 38)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else if (col > 34 && col < 48 && row == 10)
                    {
                        level[col, row] = new LevelElement(LevelElementType.BreakableWall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                else if (currentLevelNumber == 6)
                {
                    if (col == 29 && row > 3 && row < 15
                    || row == 9 && col > 15 && col < 43)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                else if (currentLevelNumber == 7)
                {
                    if (row > 2 && row < 7 && col > 2 && col < 10
                    || row > 2 && row < 7 && col > 48 && col < 56
                    || row > 11 && row < 16 && col > 2 && col < 10
                    || row > 11 && row < 16 && col > 48 && col < 56
                    || col == 29 && row > 3 && row < 15
                    || row == 9 && col > 15 && col < 43)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                else if (currentLevelNumber == 8)
                {
                    if (
        row == 4 && col > 18 && col < 39
        || row == 5 && col > 19 && col < 38
        || row == 6 && col > 21 && col < 36
        || row == 7 && col > 23 && col < 34
        || row == 8 && col > 25 && col < 32
        || col > 32 && col < 37 && row >= 1 && row < 17)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else if (col > 32 && col < 37 && row >= 17 && row < 19)
                    {
                        level[col, row] = new LevelElement(LevelElementType.BreakableWall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                else if (currentLevelNumber == 9)
                {
                    if (col == 10 && row > 2 && row < 15
                    || col == 20 && row > 4 && row < 17
                    || col == 30 && row > 2 && row < 13
                    || col == 40 && row > 5 && row < 16
                    || col == 50 && row > 2 && row < 15)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else
                    {
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
                else if (currentLevelNumber == 10)
                {
                    if (row == 5 && col > 10 && col < 48
                    || row == 12 && col > 10 && col < 48)
                    {
                        level[col, row] = new LevelElement(LevelElementType.Wall);
                    }
                    else if (col == 20 && row > 5 && row < 12
                        || col == 38 && row > 5 && row < 12)
                    {
                        level[col, row] = new LevelElement(LevelElementType.BreakableWall);
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

    public void DestroyBreakableWalls(int centerX, int centerY, int radius)
    {
        // from 'radius' amount of places up to 'radius' amount of spaces down
        for (int row = centerY - radius; row <= centerY + radius; row++)
        {
            // from 'radius' amount of places left to 'radius' amount of spaces right
            for (int col = centerX - radius; col <= centerX + radius; col++)
            {
                // checks if the position is in the playfield
                if (col >= 0 && col < width && row >= 0 && row < height)
                {
                    // check if its a breakable wall
                    if (level[col, row].Type == LevelElementType.BreakableWall)
                    {
                        // clear
                        level[col, row] = new LevelElement(LevelElementType.Empty);
                    }
                }
            }
        }
    }

    public void CheckErrorPositions(List<Powerup> powerups, List<Zombie> zombies)
    {
        foreach (Powerup powerup in powerups)
        {
            while (GetElementTypeAt(powerup.CursorY, powerup.CursorX) == LevelElementType.Wall
            || GetElementTypeAt(powerup.CursorY, powerup.CursorX) == LevelElementType.BreakableWall)
            {
                if(powerup.XPos + 1 < width - 1)
                {
                powerup.XPos++;
                }
                else if(powerup.YPos + 1 < height - 1)
                {
                powerup.YPos++;
                }
                else
                {
                    if(powerup.XPos - 1 > 0)
                    {
                        powerup.XPos--;
                    }
                    else if(powerup.YPos - 1 > 0)
                    {
                        powerup.YPos--;
                    }
                }
            }
        }

        foreach (Zombie zombie in zombies)
        {
            while (GetElementTypeAt(zombie.CursorY, zombie.CursorX) == LevelElementType.Wall
            || GetElementTypeAt(zombie.CursorY, zombie.CursorX) == LevelElementType.BreakableWall)
            {
                zombie.XPos++;
                zombie.YPos++;
            }
        }
    }
}
