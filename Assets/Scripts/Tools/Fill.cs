using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : DrawingTool {

    struct Point {
        public int x, y;
        
        public Point(int x, int y) { this.x = x; this.y = y; }
    }

    private void Start() {
        canvas = DrawableCanvas.instance;
    }


    public override void UseTool(int xPix, int yPix) {
        Point initalNode = new Point(xPix, yPix);
        Color refColour = canvas.colourMap[xPix + yPix * canvas.canvasSizeX];
        Queue<Point> nodes = new Queue<Point>();

        nodes.Enqueue(initalNode);

        while (nodes.Count > 0) {
            Point current = nodes.Dequeue();
            
            // Going right of the pixel
            for (int i = current.x; i < canvas.canvasSizeX; i++) {
                Color C = canvas.colourMap[i + current.y * canvas.canvasSizeX];

                if (C != refColour || C == paintColour) break;
                canvas.colourMap[i + current.y * canvas.canvasSizeX] = paintColour;

                // Checking pixel above
                if (current.y + 1 < canvas.canvasSizeY) {
                    C = canvas.colourMap[i + current.y * canvas.canvasSizeX + canvas.canvasSizeX];
                    if (C == refColour && C != paintColour) nodes.Enqueue(new Point(i, current.y + 1));
                }

                // Checking pixel below
                if (current.y - 1 >= 0) {
                    C = canvas.colourMap[i + current.y * canvas.canvasSizeX - canvas.canvasSizeX];
                    if (C == refColour && C != paintColour) nodes.Enqueue(new Point(i, current.y - 1));
                }
            }

            // Going left of the pixel
            for (int i = current.x - 1; i >= 0; i--) {
                Color C = canvas.colourMap[i + current.y * canvas.canvasSizeX];

                if (C != refColour || C == paintColour) break;
                canvas.colourMap[i + current.y * canvas.canvasSizeX] = paintColour;

                // Checking pixel above
                if (current.y + 1 < canvas.canvasSizeY) {
                    C = canvas.colourMap[i + current.y * canvas.canvasSizeX + canvas.canvasSizeX];
                    if (C == refColour && C != paintColour) nodes.Enqueue(new Point(i, current.y + 1));
                }

                // Checking pixel below
                if (current.y - 1 >= 0) {
                    C = canvas.colourMap[i + current.y * canvas.canvasSizeX - canvas.canvasSizeX];
                    if (C == refColour && C != paintColour) nodes.Enqueue(new Point(i, current.y - 1));
                }

            }
        }
    }
}
