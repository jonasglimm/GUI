﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public Camera mapCamera;

    public float speed = 5f;

    public float maxZoom;
    public Vector3 boundries1;
    public Vector3 boundries2;
    public Vector3 boundries3;
    public Vector3 boundries4;
    public bool isTrackpadEnabled;
    private float mapCameraX;
    private float mapCameraY;
    private float mapCameraZ;
    private Vector3 lastMouseCoordinate = Vector3.zero;
    void Update()
    {
        mapCameraX = mapCamera.transform.position.x;
        mapCameraY = mapCamera.transform.position.y;
        mapCameraZ = mapCamera.transform.position.z;

        //keeping the camera in boundries so prevent scrolling apart from the map
        mapCamera.transform.position = new Vector3(mapCameraX, mapCameraY, Mathf.Clamp(mapCameraZ, boundries4.z, maxZoom));
        if (mapCameraZ < maxZoom && mapCameraZ >= boundries1.z)
        {
            mapCamera.transform.position = new Vector3(Mathf.Clamp(mapCameraX, -boundries1.x, boundries1.x),
            Mathf.Clamp(mapCameraY, -boundries1.y, boundries1.y),
            mapCameraZ);
        }

        else if (mapCameraZ < boundries1.z && mapCameraZ >= boundries2.z)
        {
            mapCamera.transform.position = new Vector3(Mathf.Clamp(mapCameraX, -boundries2.x, boundries2.x),
            Mathf.Clamp(mapCameraY, -boundries2.y, boundries2.y),
            mapCameraZ);
        }

        else if (mapCameraZ < boundries2.z && mapCameraZ >= boundries3.z)
        {
            mapCamera.transform.position = new Vector3(Mathf.Clamp(mapCameraX, -boundries3.x, boundries3.x),
            Mathf.Clamp(mapCameraY, -boundries3.y, boundries3.y),
            mapCameraZ);
        }

        else if (mapCameraZ < boundries3.z && mapCameraZ >= boundries4.z)
        {
            mapCamera.transform.position = new Vector3(Mathf.Clamp(mapCameraX, -boundries4.x, boundries4.x),
            Mathf.Clamp(mapCameraY, -boundries4.y, boundries4.y),
            mapCameraZ);
        }

        if(isTrackpadEnabled == true){
            handleTrackpadInput();
        } else{
            //movement of Camera using the Unity Axis
            if (Input.GetAxis("Horizontal") != 0)
            {
                mapCamera.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0));
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                mapCamera.transform.Translate(new Vector3(0, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0));
            }

            if (Input.GetAxis("Lateral") != 0) //new axis for Zoom generated in "Edit" -> "Project Settings" -> "Input"
            {
                mapCamera.transform.Translate(new Vector3(0, 0, Input.GetAxis("Lateral") * speed/1.5f * Time.deltaTime));
            }
        }
         
    }

    private void handleTrackpadInput(){
        if(Input.mouseScrollDelta.y != 0){
             mapCamera.transform.Translate(new Vector3(0, 0, Input.mouseScrollDelta.y * speed/1.5f * Time.deltaTime));
        }

        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;

        if(mouseDelta.x !=  0 ){ // if difference less than zero, moved to left
            lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
            mapCamera.transform.Translate(new Vector3(mouseDelta.x  * speed * Time.deltaTime, 0, 0));
        } 

        if(mouseDelta.y !=  0 ){ // if difference less than zero, moved to left
            lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
            mapCamera.transform.Translate(new Vector3(0, mouseDelta.y  * speed * Time.deltaTime, 0));
        } 
    }
}
