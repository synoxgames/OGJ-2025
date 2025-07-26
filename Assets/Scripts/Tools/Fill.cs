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
        Color refColour = canvas.colourMap[GetIndex(xPix, yPix)];
        if (refColour == paintColour) return;

        Queue<Point> nodes = new Queue<Point>();
        nodes.Enqueue(initalNode);

        while (nodes.Count > 0) {
            Point current = nodes.Dequeue();

            // Going right of the pixel
            for (int i = current.x; i < canvas.canvasSizeX; i++) {
                Color C = canvas.colourMap[GetIndex(i, current.y)];
                if (C != refColour) break;
                canvas.colourMap[GetIndex(i, current.y)] = paintColour;

                // Check pixel above
                if (current.y + 1 < canvas.canvasSizeY) {
                    C = canvas.colourMap[GetIndex(i, current.y + 1)];
                    if (C == refColour) nodes.Enqueue(new Point(i, current.y + 1));
                }

                // Check pixel below
                if (current.y - 1 >= 0) {
                    C = canvas.colourMap[GetIndex(i, current.y - 1)];
                    if (C == refColour) nodes.Enqueue(new Point(i, current.y - 1));
                }
            }

            // Going left of the pixel
            for (int i = current.x - 1; i >= 0; i--) {
                Color C = canvas.colourMap[GetIndex(i, current.y)];
                if (C != refColour) break;
                canvas.colourMap[GetIndex(i, current.y)] = paintColour;

                // Check pixel above
                if (current.y + 1 < canvas.canvasSizeY) {
                    C = canvas.colourMap[GetIndex(i, current.y + 1)];
                    if (C == refColour) nodes.Enqueue(new Point(i, current.y + 1));
                }

                // Check pixel below
                if (current.y - 1 >= 0) {
                    C = canvas.colourMap[GetIndex(i, current.y - 1)];
                    if (C == refColour) nodes.Enqueue(new Point(i, current.y - 1));
                }
            }
        }
    }

    int GetIndex(int x, int y) {
        int rotatedX = y;
        int rotatedY = canvas.canvasSizeX - 1 - x;
        return rotatedX + rotatedY * canvas.canvasSizeX;
    }
}
