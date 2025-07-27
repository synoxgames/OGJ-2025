using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DrawingTool : MonoBehaviour
{
    [Header("Brush Information")]
    public string toolName;
    public int brushSize = 1;
    public Color paintColour;
    public bool useInterpolation = true;
    private bool unlocked = false;
    private int cost = 100;

    protected DrawableCanvas canvas = DrawableCanvas.instance;

    public virtual void Buy()
    {
        if (CoinManager.GetCoinCount() >= cost)
        {
            CoinManager.ChangeCoins(-cost);
            unlocked = true;
        }
    }

    public virtual bool IsUnlocked()
    {
        return unlocked;
    }

    public virtual void SetColour(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out paintColour);
    }

    public abstract void UseTool(int xPix, int yPix);
}
