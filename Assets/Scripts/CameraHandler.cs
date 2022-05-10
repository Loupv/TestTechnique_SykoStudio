using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{

    GameObject mainCamera;
    
    public float optimalHeight = 2, optimalZoomSpeed = 5;
 
    public float accumulatedDistanceToPlanet;
    public float cameraAngle;
    public float minCameraToPlanetDistance, maxCameraToPlanetDistance;



    // Start is called before the first frame update
    public void InitCameraSystem(int pCount)
    {
        mainCamera = Camera.main.gameObject;
        accumulatedDistanceToPlanet = maxCameraToPlanetDistance / 2 + minCameraToPlanetDistance;
    }

    public void UpdateCamera(Transform targettedPlanetTransform)
    {
        Transform cameraTransform = mainCamera.transform;

        if (Input.GetKey(KeyCode.LeftArrow)) cameraAngle -= 1;
        else if (Input.GetKey(KeyCode.RightArrow)) cameraAngle += 1;

        if ((Input.GetKey(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0) && GetHorizontalDistance(cameraTransform.position, targettedPlanetTransform.position) > minCameraToPlanetDistance)
            accumulatedDistanceToPlanet -= 1;
        else if ((Input.GetKey(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0) && GetHorizontalDistance(cameraTransform.position, targettedPlanetTransform.position) < maxCameraToPlanetDistance)
            accumulatedDistanceToPlanet += 1;

        cameraTransform.position = targettedPlanetTransform.position + new Vector3(Mathf.Cos(cameraAngle * 2 * Mathf.PI/360) * accumulatedDistanceToPlanet, optimalHeight, Mathf.Sin(cameraAngle * 2 * Mathf.PI / 360) * accumulatedDistanceToPlanet);

        cameraTransform.LookAt(targettedPlanetTransform.transform);

        mainCamera.transform.position = cameraTransform.position;
        mainCamera.transform.rotation = cameraTransform.rotation;

    }


    public float GetHorizontalDistance(Vector3 vector1, Vector3 vector2)
    {
        vector1.y = 0;
        vector2.y = 0;
        return Vector3.Distance(vector1, vector2);
    }

}
