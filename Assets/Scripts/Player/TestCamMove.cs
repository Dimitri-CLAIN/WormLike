using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Working Camera Move coded by Waldo here https://www.youtube.com/watch?v=4_HUlAFlxwU&t=298s
/// Not used, just kept as a reference to implement the PlayerCamera script
/// </summary>
public class TestCamMove : MonoBehaviour {
    private Vector3 touchStart;
    public Camera cam;
    public float groundZ = 0;

    void Update () {
        if (Input.GetMouseButtonDown(0)){
            touchStart = GetWorldPosition(groundZ);
        }
        if (Input.GetMouseButton(0)){
            Vector3 direction = touchStart - GetWorldPosition(groundZ);
            cam.transform.position += direction;
        }
    }
    private Vector3 GetWorldPosition(float z){
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0,0,z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
