using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : DrawingTool
{
    bool[,] visitedPixels;

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
        // find all adjacent pixels with this colour
        Color floodColour = canvas.canvasPixels[canvas.GetPixelIndexAt(canvaxPixelX, canvaxPixelY)];

        visitedPixels = new bool[canvas.canvasResolutionX, canvas.canvasResolutionY];
        Queue<Pixel> pixelsToBeFilled = new Queue<Pixel>();

        pixelsToBeFilled.Enqueue(initialPixel);

        while (pixelsToBeFilled.Count > 0)
        {
            Pixel current = pixelsToBeFilled.Dequeue();
            FloodFill(current, floodColour, pixelsToBeFilled);
        }

        // get all the connected pixels of the same colour
        FloodFill(initialPixel, floodColour, pixelsToBeFilled);

        // paint the pixels
        for (int y = 0; y < canvas.canvasResolutionY; y ++)
        {
            for (int x = 0; x < canvas.canvasResolutionX; x ++)
            {
                if (visitedPixels[x,y] == true)
                {
                    canvas.canvasPixels[canvas.GetPixelIndexAt(x, y)] = paintColour;
                }
            }
        }
    }

    // its recursion wednesday baby
    private void FloodFill(Pixel pixel, Color floodColour, Queue<Pixel> pixelsToBeFilled)
    {
        // the pixel is already to be filled
        if (visitedPixels[pixel.x, pixel.y] == true)
        {
            return;
        }

        // the pixel is a different colour
        if (canvas.canvasPixels[canvas.GetPixelIndexAt(pixel.x, pixel.y)] != floodColour)
        {
            return;
        }

        visitedPixels[pixel.x, pixel.y] = true;

        Pixel up = pixel;
        Pixel down = pixel;
        Pixel left = pixel;
        Pixel right = pixel;

        if (up.y + 1 < canvas.canvasResolutionY)
        {
            up.y += 1;
        }
        if (down.y - 1 >= 0)
        {
            down.y -= 1;
        }
        if (right.x + 1 < canvas.canvasResolutionX)
        {
            right.x += 1;
        }
        if (left.x - 1 >= 0)
        {
            left.x -= 1;
        }

        pixelsToBeFilled.Enqueue(up);
        pixelsToBeFilled.Enqueue(left);
        pixelsToBeFilled.Enqueue(right);
        pixelsToBeFilled.Enqueue(down);
    }
}
