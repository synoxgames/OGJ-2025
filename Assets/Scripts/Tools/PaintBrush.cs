using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : DrawingTool
{
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
}
