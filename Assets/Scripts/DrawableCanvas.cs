using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableCanvas : MonoBehaviour
{
    [Header("Canvas Properties")]
    public int canvasSizeX = 512;
    public int canvasSizeY = 512;

    [Header("Tool Information")]
    public DrawingTool activeTool;

    [Header("Rendering Information")]
    public Material canvasMaterial;
    public Texture2D generatedDrawing;

    [Header("Drawing Information")]
    public Transform cursorPoint;           // Where the cursor is on the screen
    public Transform topLeftPosition;       // These are to help with knowing where the cursor is on screen
    public Transform bottomRightPosition;   // These are to help with knowing where the cursor is on screen

    public static DrawableCanvas instance;

    [HideInInspector]
    public Color[] colourMap;

    Camera mainCamera;
    int xPixel = 0, yPixel = 0;

    private void Awake() {
        if (instance != this)
            Destroy(instance);
        instance = this;


        mainCamera = Camera.main;
    }

    private void Start() {
        CreateNewCanvas();
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            SelectPixel();
        }
    }

    public void CreateNewCanvas() {
        colourMap = new Color[canvasSizeX * canvasSizeY];
        generatedDrawing = new Texture2D(canvasSizeX, canvasSizeY, TextureFormat.RGBA32, false);
        generatedDrawing.filterMode = FilterMode.Point;

        ResetCanvasColour();
        
        canvasMaterial.SetTexture("_MainTex", generatedDrawing);
    }

    private void SelectPixel() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f)) {
            cursorPoint.position = hit.point;
            xPixel = (int)((cursorPoint.position.x - topLeftPosition.position.x) * canvasSizeX / (bottomRightPosition.position.x - topLeftPosition.position.x));
            yPixel = (int)((cursorPoint.position.y - topLeftPosition.position.y) * canvasSizeX / (bottomRightPosition.position.y - topLeftPosition.position.y));
            UseTool(xPixel, yPixel);
        }
    }

    public void UseTool(int x, int y) {
        activeTool.UseTool(x, y);
        SetTexture();
    }

    public void SetTexture() {
        generatedDrawing.SetPixels(colourMap);
        generatedDrawing.Apply();
    }

    public void ResetCanvasColour() {
        for (int i = 0; i < colourMap.Length; i++) {
            colourMap[i] = Color.white;
        }

        SetTexture();
    }
}
