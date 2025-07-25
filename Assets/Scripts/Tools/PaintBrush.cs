using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintBrush : DrawingTool
{
    [Header("UI")]
    public Text pixelText;
    public Image colourSelected;

    private void Start() {
        UpdateUI();
    }

    // Probably a temporary way to change the brushes size
    private void Update() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.Equals)) {
                brushSize++;
            } else if (Input.GetKeyDown(KeyCode.Minus)) {
                brushSize--;
            }

            brushSize = Mathf.Clamp(brushSize, 1, 100);
            UpdateUI();
        }
    }

    public void UpdateUI() {
        pixelText.text = $"{brushSize}px";
        colourSelected.color = paintColour;
    }


    public override void UseTool(int xPix, int yPix) {
        int i = xPix - brushSize + 1;
        int j = yPix - brushSize + 1;

        int iMax = xPix + brushSize - 1;
        int jMax = yPix + brushSize - 1;

        if (i < 0) i = 0;
        if (j < 0) j = 0;

        if (iMax >= DrawableCanvas.instance.canvasSizeX) iMax = DrawableCanvas.instance.canvasSizeX - 1;
        if (jMax >= DrawableCanvas.instance.canvasSizeY) jMax = DrawableCanvas.instance.canvasSizeY - 1;

        for (int x = i; x <= iMax; x++) {
            for (int y = j; y <= jMax; y++) {

                if ((x - xPix)  * (x - xPix) + (y - yPix) * (y - yPix) <= brushSize * brushSize) {
                    DrawableCanvas.instance.colourMap[x * DrawableCanvas.instance.canvasSizeY + y] = paintColour;
                }

            }
        }
    }

    public override void SetColour(string hex) {
        ColorUtility.TryParseHtmlString(hex, out paintColour);
        UpdateUI();
    }
}
