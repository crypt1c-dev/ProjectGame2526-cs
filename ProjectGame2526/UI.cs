using System;

namespace ProjectGame2526;

public class UI
{
    protected List<UIElement> uiElements;

    public UI()
    {
        uiElements = new List<UIElement>();
    }

    public void AddUIElement(UIElement element)
    {
        uiElements.Add(element);
    }

    public void Draw()
    {
        foreach (UIElement element in uiElements)
        {
            element.Draw();
        }
    }

    public void UpdateUIElementValue(string elementName, int newValue)
    {
        bool updated = false;
        for(int i = 0; i < uiElements.Count && !updated; i++)
        {
            if(uiElements[i].Name == elementName)
            {
                uiElements[i].ElementValue = newValue;
                updated = true;
            }
        }
    }


}
