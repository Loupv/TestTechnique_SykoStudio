using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe relative à la manipulation de la caméra
public class CameraHandler : MonoBehaviour
{

    GameObject mainCamera;

    public float cameraMoveSpeed, cameraRotationSpeed;

    public float optimalHeight = 2, optimalZoomSpeed = 5;
 
    public float accumulatedDistanceToPlanet;
    public float cameraAngle;
    public float minCameraToPlanetDistance, maxCameraToPlanetDistance;

    
    public void InitCameraSystem()
    {
        mainCamera = Camera.main.gameObject;
        accumulatedDistanceToPlanet = maxCameraToPlanetDistance / 2 + minCameraToPlanetDistance;
    }

    public void UpdateCamera(Transform targettedPlanetTransform)
    {
        Transform cameraTransform = mainCamera.transform;

        if (Input.GetKey(KeyCode.LeftArrow)) cameraAngle -= cameraRotationSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.RightArrow)) cameraAngle += cameraRotationSpeed * Time.deltaTime;

        if ((Input.GetKey(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0) && GetHorizontalDistance(cameraTransform.position, targettedPlanetTransform.position) > minCameraToPlanetDistance)
            accumulatedDistanceToPlanet -= cameraMoveSpeed * Time.deltaTime;
        else if ((Input.GetKey(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0) && GetHorizontalDistance(cameraTransform.position, targettedPlanetTransform.position) < maxCameraToPlanetDistance)
            accumulatedDistanceToPlanet += cameraMoveSpeed * Time.deltaTime;

        cameraTransform.position = targettedPlanetTransform.position + new Vector3(Mathf.Cos(cameraAngle * 2 * Mathf.PI/360) * accumulatedDistanceToPlanet, optimalHeight, Mathf.Sin(cameraAngle * 2 * Mathf.PI / 360) * accumulatedDistanceToPlanet);

        cameraTransform.LookAt(targettedPlanetTransform.transform);

        mainCamera.transform.position = cameraTransform.position;
        mainCamera.transform.rotation = cameraTransform.rotation;

    }

    // Récupère la distance sur le plan horizontal, ne prend pas en compte l'axe Y
    public float GetHorizontalDistance(Vector3 vector1, Vector3 vector2)
    {
        vector1.y = 0;
        vector2.y = 0;
        return Vector3.Distance(vector1, vector2);
    }

}
