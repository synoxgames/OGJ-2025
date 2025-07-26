using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : DrawingTool {

    struct Pixel
    {
        public int x, y;
        
        public Pixel(int x, int y)
        {
            this.x = x; this.y = y;
        }
    }

    private void Start()
    {
        canvas = DrawableCanvas.instance;
    }

    // fill every connected pixel of the same colour with the fill colour
    public override void UseTool(int canvaxPixelX, int canvaxPixelY)
    {
        // the pixel to start filling from
        Pixel initialPixel = new Pixel(canvaxPixelX, canvaxPixelY);

        Debug.Log(canvaxPixelX + ", " + canvaxPixelY);

        // find the colour of the pixel to start filling from
        Color colourToFill = canvas.canvasPixels[canvas.GetPixelIndexAt(canvaxPixelX, canvaxPixelY)];

        // a queue of pixels that are to be filled
        Queue<Pixel> pixelsToFill = new Queue<Pixel>();
        pixelsToFill.Enqueue(initialPixel);

        while (pixelsToFill.Count > 0)
        {
            Pixel current = pixelsToFill.Dequeue();
            
            // Going right of the pixel
            for (int i = current.x; i < canvas.canvasResolutionX; i++)
            {
                Color fillingColour = canvas.canvasPixels[canvas.GetPixelIndexAt(current.x + 1, current.y)];

                if (fillingColour != colourToFill || fillingColour == paintColour) break;
                canvas.canvasPixels[canvas.GetPixelIndexAt(current.x + 1, current.y)] = paintColour;

                // Checking pixel above
                if (current.y + 1 < canvas.canvasResolutionY)
                {
                    fillingColour = canvas.canvasPixels[i + current.y * canvas.canvasResolutionX + canvas.canvasResolutionX];
                    if (fillingColour == colourToFill && fillingColour != paintColour) pixelsToFill.Enqueue(new Pixel(i, current.y + 1));
                }

                // Checking pixel below
                if (current.y - 1 >= 0)
                {
                    fillingColour = canvas.canvasPixels[i + current.y * canvas.canvasResolutionX - canvas.canvasResolutionX];
                    if (fillingColour == colourToFill && fillingColour != paintColour) pixelsToFill.Enqueue(new Pixel(i, current.y - 1));
                }
            }

            // Going left of the pixel
            for (int i = current.x - 1; i >= 0; i--)
            {
                Color fillingColour = canvas.canvasPixels[i + current.y * canvas.canvasResolutionX];

                if (fillingColour != colourToFill || fillingColour == paintColour) break;
                canvas.canvasPixels[i + current.y * canvas.canvasResolutionX] = paintColour;

                // Checking pixel above
                if (current.y + 1 < canvas.canvasResolutionY)
                {
                    fillingColour = canvas.canvasPixels[i + current.y * canvas.canvasResolutionX + canvas.canvasResolutionX];
                    if (fillingColour == colourToFill && fillingColour != paintColour) pixelsToFill.Enqueue(new Pixel(i, current.y + 1));
                }

                // Checking pixel below
                if (current.y - 1 >= 0)
                {
                    fillingColour = canvas.canvasPixels[i + current.y * canvas.canvasResolutionX - canvas.canvasResolutionX];
                    if (fillingColour == colourToFill && fillingColour != paintColour) pixelsToFill.Enqueue(new Pixel(i, current.y - 1));
                }

            }
        }
    }
}
