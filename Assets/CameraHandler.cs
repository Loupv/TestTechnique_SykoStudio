using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{

    GameObject mainCamera;
    
    Vector3 startCameraPosition;
    public float optimalHeight = 2, optimalZoomSpeed = 5;

    int zoomAmount, planetCount;

    public float minCameraToPlanetDistance, maxCameraToPlanetDistance;



    // Start is called before the first frame update
    public void InitCameraSystem(int pCount)
    {
        mainCamera = Camera.main.gameObject;
        startCameraPosition = mainCamera.transform.position;

        planetCount = pCount;
    }

    public void UpdateCamera(Transform targettedPlanetTransform)
    {

        //mainCamera.transform.position = startCameraPosition + planets[actualLookAt].transform.position
        //    + mainCamera.transform.forward * zoomAmount;
        //mainCamera.transform.LookAt(planets[actualLookAt].transform);

        Transform cameraTransform = mainCamera.transform;

        if (Input.GetKey(KeyCode.LeftArrow))
            cameraTransform.RotateAround(targettedPlanetTransform.position, Vector3.up, 1);

        if (Input.GetKey(KeyCode.RightArrow))
            cameraTransform.RotateAround(targettedPlanetTransform.position, Vector3.up, -1);

        cameraTransform.position = new Vector3(cameraTransform.position.x, optimalHeight, cameraTransform.transform.position.z);
        cameraTransform.LookAt(targettedPlanetTransform.transform);


        if ((Input.GetKey(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0) && GetHorizontalDistance(cameraTransform.position, targettedPlanetTransform.position) > minCameraToPlanetDistance)
            cameraTransform = ZoomIn(cameraTransform, targettedPlanetTransform, optimalZoomSpeed / 10f);
        else if ((Input.GetKey(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0) && GetHorizontalDistance(cameraTransform.position, targettedPlanetTransform.position) < maxCameraToPlanetDistance)
            cameraTransform = ZoomIn(cameraTransform, targettedPlanetTransform, -optimalZoomSpeed / 10f);
        else cameraTransform = ZoomIn(cameraTransform, targettedPlanetTransform, 0);

        mainCamera.transform.position = cameraTransform.position;
        mainCamera.transform.rotation = cameraTransform.rotation;

    }


    Transform ZoomIn(Transform originalTransform, Transform targettedPlanet, float zoomQuantity)
    {
        //zoomAmount += add;
        originalTransform.position = Vector3.MoveTowards(originalTransform.position, targettedPlanet.position, zoomQuantity);
        return originalTransform;
    }


    float GetHorizontalDistance(Vector3 vector1, Vector3 vector2)
    {
        vector1.y = 0;
        vector2.y = 0;
        return Vector3.Distance(vector1, vector2);
    }




}
