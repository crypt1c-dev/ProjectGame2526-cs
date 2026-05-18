using System;
using System.Diagnostics.Contracts;

namespace ProjectGame2526;

public class Screen
{
    protected string text;
    protected ConsoleColor foreGroundColor;
    protected ConsoleColor backGroundColor;


    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public Screen(string filePath)
    {
        StreamReader streamReader = null;
        try
        {
            streamReader = new StreamReader(filePath);

            text = streamReader.ReadToEnd();
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine("Error Screen.cs: File not found - \n" + e);
        }
        finally
        {
            if (streamReader != null)
            {
                streamReader.Close();
            }
        }
    }

    public Screen(string filePath, ConsoleColor newForeGroundColor, ConsoleColor newBackGroundColor)
    {
        foreGroundColor = newForeGroundColor;
        backGroundColor = newBackGroundColor;

        StreamReader streamReader = null;
        try
        {
            streamReader = new StreamReader(filePath);

            text = streamReader.ReadToEnd();
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine("Error Screen.cs: File not found - \n" + e);
        }
        finally
        {
            if (streamReader != null)
            {
                streamReader.Close();
            }
        }
    }

    public virtual void Draw()
    {
        Console.SetCursorPosition(0, 0);
        ConsoleColor currentForegroundColor = foreGroundColor;
        ConsoleColor currentBackgroundColor = backGroundColor;
        Console.ForegroundColor = currentForegroundColor;
        Console.BackgroundColor = currentBackgroundColor;
        Console.WriteLine(text);
    }
}
