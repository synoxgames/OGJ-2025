using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DrawingTool : MonoBehaviour
{
    [Header("Brush Information")]
    public int brushSize = 1;
    public Color paintColour;
    public bool useInterpolation = true;

    protected DrawableCanvas canvas = DrawableCanvas.instance;


    public virtual void SetColour(string hex) {
        ColorUtility.TryParseHtmlString(hex, out paintColour);
    }

    public abstract void UseTool(int xPix, int yPix);
}
