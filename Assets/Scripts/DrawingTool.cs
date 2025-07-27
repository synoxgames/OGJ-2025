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

    private void Awake()
    {
        costIcon.SetActive(true);
        costText.text = cost.ToString();

        if (ToolManager.IsUnlocked(toolName))
        {
            unlocked = true;
            costIcon.SetActive(false);
        }
    }

    public virtual void Buy()
    {
        if (CoinManager.GetCoinCount() >= cost)
        {
            CoinManager.ChangeCoins(-cost);
            unlocked = true;
            costIcon.SetActive(false);
            ToolManager.UnlockTool(toolName);
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
