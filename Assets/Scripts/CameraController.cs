using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public int xMin = -5, xMax = 5, yMin = -5, yMax = 5;
    public int minZoom = 5, maxZoom = 10;
    int zoom = 5;
    Camera mainCam;
    public float dragSpeed = 2;
    private Vector3 dragOrigin;



    private void Awake() {
        mainCam = Camera.main;
    }

    private void Update() {
        // Doing zooming
        zoom += (int)Input.GetAxisRaw("Mouse ScrollWheel");
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        mainCam.orthographicSize = zoom;

        if (Input.GetMouseButtonDown(2)) {
            dragOrigin = mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2)) {
            Vector3 difference = dragOrigin - mainCam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + difference.x, xMin, xMax),
                Mathf.Clamp(transform.position.y + difference.y, yMin, yMax),
                -10
            );
        }
    }

}
