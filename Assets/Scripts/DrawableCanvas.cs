using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableCanvas : MonoBehaviour
{
    // the resolution of the canvas
    [Header("Canvas Properties")]
    public int canvasResolutionX;
    public int canvasResolutionY;

    // the currently selected tool
    [Header("Tool Information")]
    public DrawingTool selectedTool;

    [Header("Rendering Information")]
    // that material that the canvas uses, displays the canvas texture
    public Material canvasMaterial;
    // the texture that is displayed on the canvas
    public Texture2D canvasTexture;

    // the positions of the mouse and the coreners of the canvas
    [Header("Drawing Information")]
    public Transform cursorPosition;        // Where the cursor is on the screen
    public Transform topLeftCorner;         // These are to help with knowing where the cursor is on screen
    public Transform bottomRightCorner;     // These are to help with knowing where the cursor is on screen

    [Header("Compare Information")]
    [SerializeField] ComparisonManager compaisonManager; //reference to the CompareScript component 
    
    public static DrawableCanvas instance;

    [HideInInspector]
    // this array represents the pixels in the canvas, used to update the generated drawing
    public Color[] canvasPixels;
    // the main camera in the scene, used for raycasting
    Camera mainCamera;
    // why are these global
    int cursorCanvasPixelX = 0, cursorCanvasPixelY = 0;
    // the position of the something last frame
    int cursorCanvasPixelLastFrameX = 0, cursorCanvasPixelLastFrameY = 0;
    // was the mouse held down last frame or not
    bool toolUsedLastFrame = false;
    // the ui that us used to select tools / colours
    ToolUIHandler uiHandler;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(instance);
        }
        instance = this;

        // get the main camera in the scene
        mainCamera = Camera.main;
        // get the ui manager
        uiHandler = FindObjectOfType<ToolUIHandler>();
    }

    private void Start()
    {
        CreateNewCanvas();
    }

    // called by tools when they are selected
    // sets the currently selected tool to whatever was selected
    public void ChangeTool(DrawingTool tool)
    {
        selectedTool = tool;
        uiHandler.ChangeTool(tool);
    }

    // called by the colours in the colour picker when they are selected
    public void ChangeActiveToolColour(string hex)
    {
        selectedTool.SetColour(hex);
        uiHandler.ChangeTool(selectedTool);
    }

    private void Update()
    {
        // if the mouse was clicked select whetever pixel the cursor is over
        if (Input.GetMouseButton(0))
        {
            // try and find the canvas pixel being clicked on
            SelectPixel();
        }
        else
        {
            toolUsedLastFrame = false;
        }
    }

    // creates a new canvas as well as the things needed to draw on it
    public void CreateNewCanvas()
    {
        // create the array that represents the pixels on the canvas
        canvasPixels = new Color[canvasResolutionX * canvasResolutionY];

        // create the texture that will be displayed on the canvas
        canvasTexture = new Texture2D(canvasResolutionX, canvasResolutionY, TextureFormat.RGB24, false);
        canvasTexture.filterMode = FilterMode.Point;

        // clear the canvas
        ClearCanvas();

        // set the canvas to use its texture
        canvasMaterial.SetTexture("_MainTex", canvasTexture);
    }

    // runs if the left mouse button is held down
    private void SelectPixel()
    {
        // cast a ray from the camera to wherever the cursor is
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // if the ray hits the canvas
        if (Physics.Raycast(ray, out hit, 10f))
        {
            // move the cursor to the hit point
            cursorPosition.position = hit.point;

            // find the position of the cursor relative to the top left corner in world units
            float relativeCursorPositionX = cursorPosition.position.x - topLeftCorner.position.x;
            float relativeCursorPositionY = cursorPosition.position.y - topLeftCorner.position.y;

            // find the width of the canvas in wrold units
            float canvasWidth = bottomRightCorner.position.x - topLeftCorner.position.x;
            float canvasHeight = bottomRightCorner.position.y - topLeftCorner.position.y;

            // find the number of pixels on the canvas per world unit
            float canvasPixelsPerWorldUnitX = canvasResolutionX / canvasWidth;
            float canvasPixelsPerWorldUnitY = canvasResolutionY / canvasHeight;

            // find the x and y canvas pixel coordinates of the cursor
            cursorCanvasPixelX = (int)(relativeCursorPositionX * canvasPixelsPerWorldUnitX);
            cursorCanvasPixelY = (int)(relativeCursorPositionY * canvasPixelsPerWorldUnitY);

            // use whatever tool is selected at the canvas coordinates of the hit
            UseToolAt(cursorCanvasPixelX, cursorCanvasPixelY);
        }
        else
        {
            toolUsedLastFrame = false;
        }
    }

    // called whenever a raycast hits the canvas
    // uses a tool at the position that the raycast hit
    public void UseToolAt(int canvasPixelX, int canvasPixelY)
    {
        // if the selected tool uses interpolation, use the tool along a line from the cursors position last frame to its position this frame
        if (selectedTool.useInterpolation && toolUsedLastFrame && CursorHasMoved())
        {
            // find the distance that the cursor has moved
            int changeInX = cursorCanvasPixelX - cursorCanvasPixelLastFrameX;
            int changeInY = cursorCanvasPixelY - cursorCanvasPixelLastFrameY;

            // sqrt(x^2 + y^2)
            int distance = (int)Mathf.Sqrt(Mathf.Pow(changeInX, 2) + Mathf.Pow(changeInY, 2));

            // use the tool every unit along the path from where the cursor was last frame to where it is this frame
            for (int travel = 1; travel <= distance; travel++)
            {
                int interpolatedX = (travel * cursorCanvasPixelX + (distance - travel) * cursorCanvasPixelLastFrameX) / distance;
                int interpolatedY = (travel * cursorCanvasPixelY + (distance - travel) * cursorCanvasPixelLastFrameY) / distance;

                // use the tool at every interpolated position
                selectedTool.UseTool(interpolatedX, interpolatedY);
            }
        }

        // use the selected tool at the current cursor pixel coordinates
        selectedTool.UseTool(canvasPixelX, canvasPixelY);

        // remember the position of the cursor for next frame
        // used for interpolation if the mouse is moving too fast
        cursorCanvasPixelLastFrameX = canvasPixelX;
        cursorCanvasPixelLastFrameY = canvasPixelY;

        toolUsedLastFrame = true;

        // draw the pixels onto the canvas
        PixelsToCanvas();
    }

    // returns the index of the pixel at a position in the canvas pixels array
    public int GetPixelIndexAt(int canvaxPixelX, int canvaxPixelY)
    {
        int index = canvaxPixelX * canvasResolutionY + canvaxPixelY;
        return index;
    }

    // checks if the cursor has moved since last frame
    private bool CursorHasMoved()
    {
        if (cursorCanvasPixelLastFrameX == cursorCanvasPixelX && cursorCanvasPixelLastFrameY == cursorCanvasPixelY)
        {
            return false;
        }
        return true;
    }

    // set the pixels of the canvas texture to the colours in the canvas pixels array
    public void PixelsToCanvas()
    {
        canvasTexture.SetPixels(canvasPixels);
        canvasTexture.Apply();
    }

    // clear the canvas pixels, replacing every pixel with white
    public void ClearCanvas()
    {
        for (int index = 0; index < canvasPixels.Length; index++)
        {
            canvasPixels[index] = Color.white;
        }

        // draw the cleared pixels onto the canvas
        PixelsToCanvas();
    }

    // compare the drawn image to the refernce image
    public void SumbitCanvas()
    {
        PixelsToCanvas();
        Texture2D rotatedCanvasTexture = RotateCanvas(canvasTexture);
        int badnessScore = ImageComparer.CompareImages(rotatedCanvasTexture, ArtManager.GetArtTexture(), 6, 30, 3);
        compaisonManager.SetReferenceImage(ArtManager.GetArtTexture());
        compaisonManager.SetDrawnImage(rotatedCanvasTexture);
        compaisonManager.StartAnimation(badnessScore);
    }

    // Rotate the Texture2D clockwise (Will add more comments soon)
    Texture2D RotateCanvas(Texture2D originalTexture)
    {
        Color[] originalCanvasPixels = originalTexture.GetPixels();
        Color[] rotatedCanvasPixels = new Color[originalCanvasPixels.Length];
        int width = originalTexture.width;
        int height = originalTexture.height;

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                int rotatedIndex = (x + 1) * height - y - 1;
                int originalIndex = originalCanvasPixels.Length - 1 - (y * width + x);
                rotatedCanvasPixels[rotatedIndex] = originalCanvasPixels[originalIndex];
            }
        }

        Texture2D rotatedTexture = new Texture2D(height, width, TextureFormat.RGB24, false);
        rotatedTexture.SetPixels(rotatedCanvasPixels);
        rotatedTexture.Apply();
        return rotatedTexture;
    }
}
