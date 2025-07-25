using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int minZoom = 5, maxZoom = 10;

    int zoom = 5;
    Camera mainCam;

    private void Awake() {
        mainCam = Camera.main;
    }

    private void Update() {
        zoom += (int)Input.GetAxisRaw("Mouse ScrollWheel");
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        mainCam.orthographicSize = zoom;
    }

}
