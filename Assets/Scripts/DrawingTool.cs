using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class DrawingTool : MonoBehaviour
{
    [Header("Brush Information")]
    public string toolName;
    public int brushSize = 1;
    public Color paintColour;
    public bool useInterpolation = true;
    public bool unlocked = false;
    public int cost = 100;
    public Text costText;
    public GameObject costIcon;

    protected DrawableCanvas canvas = DrawableCanvas.instance;

    // on load, check if this tool is unlocked
    private void Awake()
    {
        costIcon.SetActive(true);
        costText.text = $"${cost.ToString()}";

        if (ToolColourManager.IsUnlocked(toolName))
        {
            Unlock();
        }
    }

    // unlock this tool
    public virtual void Unlock()
    {
        unlocked = true;
        costIcon.SetActive(false);
        ToolColourManager.UnlockTool(toolName);
    }

    // buy this tool
    public virtual void Buy()
    {
        if (CoinManager.GetCoinCount() >= cost)
        {
            CoinManager.ChangeCoins(-cost);
            Unlock();
        }
    }

    // check if this tool is unlocked or not
    public virtual bool IsUnlocked()
    {
        return unlocked;
    }

    // set teh colour of this tool
    public virtual void SetColour(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out paintColour);
    }

    public abstract void UseTool(int xPix, int yPix);
}
