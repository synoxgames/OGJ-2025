using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DrawingTool : MonoBehaviour
{
    public int brushSize = 1;
    public Color paintColour;

    public void SetColour(Color selectedColour) {
        paintColour = selectedColour;
    }

    public abstract void UseTool(int xPix, int yPix);
}
