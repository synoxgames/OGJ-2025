using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFill : DrawingTool
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
        if (mouseHeld && (mouseHeldLastFrame == false))
        {
            startPosX = xPix;
            startPosY = yPix;
        }
        if (mouseHeld == true && mouseHeldLastFrame == true)
        {
            // draw an oval from startPosX, startPosY to xPix, yPix
            int xMin = Mathf.Min(startPosX, xPix);
            int yMin = Mathf.Min(startPosY, yPix);
            int xMax = Mathf.Max(startPosX, xPix);
            int yMax = Mathf.Max(startPosY, yPix);

            int xcentre = (xMin + xMax) / 2;
            int ycentre = (yMin + yMax) / 2;

            //Debug.Log(xcentre + ", " + ycentre);

            int height = ycentre - yMin;
            int width = xcentre - xMin;

            bool vertical = true;

            if (width > height)
            {
                vertical = false;
            }

            int majorAxis = Mathf.Max(height, width);
            int minorAxis = Mathf.Min(height, width);

            int deltaFoci = (int)Mathf.Sqrt(Mathf.Pow(majorAxis, 2) - Mathf.Pow(minorAxis, 2));

            int xFoci1;
            int xFoci2;
            int yFoci1;
            int yFoci2;

            if (vertical)
            {
                xFoci1 = xcentre;
                xFoci2 = xcentre;

                yFoci1 = ycentre - deltaFoci;
                yFoci2 = ycentre + deltaFoci;
            }
            else
            {
                yFoci1 = ycentre;
                yFoci2 = ycentre;

                xFoci1 = xcentre - deltaFoci;
                xFoci2 = xcentre + deltaFoci;
            }

            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    int distanceFoci1 = (int)Mathf.Sqrt(Mathf.Pow(yFoci1 - y, 2) + Mathf.Pow(xFoci1 - x, 2));
                    int distanceFoci2 = (int)Mathf.Sqrt(Mathf.Pow(yFoci2 - y, 2) + Mathf.Pow(xFoci2 - x, 2));

                    if (distanceFoci1 + distanceFoci2 < 2 * majorAxis)
                    {
                        canvas.canvasPixels[canvas.GetPixelIndexAt(x, y)] = paintColour;
                    }
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
