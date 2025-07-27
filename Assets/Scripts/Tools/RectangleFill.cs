using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectangleFill : DrawingTool
{
    [Header("UI")]
    public Image colourSelected;

    bool mouseHeldLastFrame = false;
    bool mouseHeld = false;

    int startPosX;
    int startPosY;

    private void Start()
    {
        UpdateUI();
        canvas = DrawableCanvas.instance;
    }

    private void Update()
    {
        if (mouseHeld)
        {
            mouseHeldLastFrame = true;
        }
        else
        {
            mouseHeldLastFrame = false;
        }

        mouseHeld = false;

        if (Input.GetKey(KeyCode.Mouse0) == true)
        {
            mouseHeld = true;
        }
    }

    public void UpdateUI()
    {
        colourSelected.color = paintColour;
    }

    public override void UseTool(int xPix, int yPix)
    {
        Debug.Log(xPix + ", " + yPix);

        if (mouseHeld && (mouseHeldLastFrame == false))
        {
            Debug.Log("holding Started");
            startPosX = xPix;
            startPosY = yPix;
        }
        if (mouseHeld == true && mouseHeldLastFrame == true)
        {
            Debug.Log("holding continued");
            // draw an oval from startPosX, startPosY to xPix, yPix
            int xMin = Mathf.Min(startPosX, xPix);
            int yMin = Mathf.Min(startPosY, yPix);
            int xMax = Mathf.Max(startPosX, xPix);
            int yMax = Mathf.Max(startPosY, yPix);

            for (int x = xMin; x < xMax; x ++)
            {
                for (int y = yMin; y < yMax; y ++)
                {
                    canvas.canvasPixels[canvas.GetPixelIndexAt(x, y)] = paintColour;
                }
            }
        }
    }

    public override void SetColour(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out paintColour);
        UpdateUI();
    }
}
