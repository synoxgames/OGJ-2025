using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintBrush : DrawingTool
{
    [Header("UI")]
    public Text pixelText;
    public Image colourSelected;

    private void Start()
    {
        UpdateUI();
        canvas = DrawableCanvas.instance;
        Buy();
    }

    public void ChangeBrushSize(int dir)
    {
        brushSize += dir;
        brushSize = Mathf.Clamp(brushSize, 1, 100);
        UpdateUI();
    }

    // Probably a temporary way to change the brushes size
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.Equals)) {
                ChangeBrushSize(1);
            } else if (Input.GetKeyDown(KeyCode.Minus)) {
                ChangeBrushSize(-1);
            }
        }
    }

    public void UpdateUI()
    {
        pixelText.text = $"{brushSize}px";
        colourSelected.color = paintColour;
    }


    public override void UseTool(int xPix, int yPix)
    {
        int i = xPix - brushSize + 1;
        int j = yPix - brushSize + 1;

        int iMax = xPix + brushSize - 1;
        int jMax = yPix + brushSize - 1;

        if (i < 0) i = 0;
        if (j < 0) j = 0;

        if (iMax >= canvas.canvasResolutionX) iMax = canvas.canvasResolutionX - 1;
        if (jMax >= canvas.canvasResolutionY) jMax = canvas.canvasResolutionY - 1;

        for (int x = i; x <= iMax; x++) {
            for (int y = j; y <= jMax; y++) {

                if ((x - xPix)  * (x - xPix) + (y - yPix) * (y - yPix) <= brushSize * brushSize) {

                    int pixelIndex = canvas.GetPixelIndexAt(x, y);
                    canvas.canvasPixels[pixelIndex] = paintColour;
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
