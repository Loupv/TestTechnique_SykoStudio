using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 14h50 / 16h40
// 17h40 / 18h50
// 16h00 / 20h00 -> 7h
// 20h30 / 22h00 -> 9h30
// 21h10 / 

public class SystemHandler : MonoBehaviour
{
    //public Vector2 distanceFromStarInitialRange, speedInitialRange, diameterInitialRange;
    public List<GameObject> planets;
    public Material starMaterial, redMaterial, greenMaterial, blueMaterial, yellowMaterial;
    
    GameObject mainCamera;
    GameObject mainStar;
    public UIHandler uiHandler;

    Vector3 startCameraPosition;
    int zoomAmount;
    public int targettedPlanetID = 0;
    public bool systemIsActive;

    public float minCameraToPlanetDistance, maxCameraToPlanetDistance;
    public float optimalHeight, optimalZoomSpeed;

    public void InitSystem(GameData gameData)
    {
        uiHandler = FindObjectOfType<UIHandler>();

        mainCamera = Camera.main.gameObject;
        startCameraPosition = mainCamera.transform.position;

        planets = new List<GameObject>();

        // Init Star
        mainStar = new GameObject();
        mainStar.gameObject.AddComponent<Planet>();
        Material mat = PickRightMaterial(gameData.mainStar.color);
        mainStar.GetComponent<Planet>().Init(gameData.mainStar, mat);
        planets.Add(mainStar);

        // Init Planets
        for (int i = 0; i < gameData.planetsData.Length; i++)
        {
            GameObject newPlanet = new GameObject();
            newPlanet.AddComponent<Planet>();

            mat = PickRightMaterial(gameData.planetsData[i].color);
            newPlanet.GetComponent<Planet>().Init(gameData.planetsData[i], mat);

            planets.Add(newPlanet);
        }
        ChangeLookAt(0);

        systemIsActive = true;
    }


    public void UpdateSystem()
    {

        if (systemIsActive) {
            ActualizePlanetsTransform();
        }
        if (Input.GetKeyDown(KeyCode.Space)) systemIsActive = !systemIsActive;
    }

    public void UpdateCamera()
    {

        //mainCamera.transform.position = startCameraPosition + planets[actualLookAt].transform.position
        //    + mainCamera.transform.forward * zoomAmount;
        //mainCamera.transform.LookAt(planets[actualLookAt].transform);

        Transform cameraTransform = mainCamera.transform;

        if (Input.GetKey(KeyCode.LeftArrow))
            cameraTransform.RotateAround(planets[targettedPlanetID].transform.position, Vector3.up, 1);

        if (Input.GetKey(KeyCode.RightArrow))
            cameraTransform.RotateAround(planets[targettedPlanetID].transform.position, Vector3.up, -1);

        cameraTransform.position = new Vector3(cameraTransform.position.x, optimalHeight, cameraTransform.transform.position.z);
        cameraTransform.LookAt(planets[targettedPlanetID].transform);


        if ((Input.GetKey(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0) && GetHorizontalDistance(cameraTransform.position, planets[targettedPlanetID].transform.position) > minCameraToPlanetDistance)
            cameraTransform = ZoomIn(cameraTransform, optimalZoomSpeed / 10f);
        if ((Input.GetKey(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0) && GetHorizontalDistance(cameraTransform.position, planets[targettedPlanetID].transform.position) < maxCameraToPlanetDistance)
            cameraTransform = ZoomIn(cameraTransform, -optimalZoomSpeed / 10f);

        if (Input.GetMouseButtonDown(1) && Input.mousePosition.y > 140) ChangeLookAt(-1);
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > 140) ChangeLookAt(1);

        mainCamera.transform.position = cameraTransform.position;
        mainCamera.transform.rotation = cameraTransform.rotation;

    }

    void ActualizePlanetsTransform()
    {
        foreach (GameObject planetObj in planets)
        {
            Planet planet = planetObj.GetComponent<Planet>();
            planet._angle += Time.deltaTime * planet._revolutionSpeed;
            planet.transform.position = new Vector3(Mathf.Cos(planet._angle * 2 * Mathf.PI) * planet._distanceFromStar, 0, Mathf.Sin(planet._angle * 2 * Mathf.PI) * planet._distanceFromStar);
            planet.transform.Rotate(Vector3.up, planet._rotationSpeed);
        }
    }

    public Planet GetCurrentlyFocusedPlanet()
    {
        if (planets != null && planets.Count > 0) return planets[targettedPlanetID].GetComponent<Planet>();
        else return null;
    }

    public void AdjustCurrentPlanetParameters(PlanetData data)
    {
        planets[targettedPlanetID].GetComponent<Planet>().AdjustParameters(data);
    }

    void ChangeLookAt(int add)
    {
        targettedPlanetID += add;
        if (targettedPlanetID < 0) targettedPlanetID = planets.Count - 1;
        else if (targettedPlanetID >= planets.Count) targettedPlanetID = 0;

        uiHandler.AdjustUIValues();
    }

    Transform ZoomIn(Transform originalTransform, float zoomQuantity)
    {
        //zoomAmount += add;
        originalTransform.position = Vector3.MoveTowards(originalTransform.position, planets[targettedPlanetID].transform.position, zoomQuantity);
        return originalTransform;
    }

    Material PickRightMaterial(string colName)
    {
        switch (colName)
        {
            case "star": return starMaterial;
            case "red": return redMaterial;
            case "green": return greenMaterial;
            case "blue": return blueMaterial;
            case "yellow": return yellowMaterial;
        }
        return null;
    }

    float GetHorizontalDistance(Vector3 vector1, Vector3 vector2)
    {
        vector1.y = 0;
        vector2.y = 0;
        return Vector3.Distance(vector1, vector2);
    }

}
