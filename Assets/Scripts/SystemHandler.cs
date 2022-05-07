using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 14h50 / 16h40
// 17h40 / 18h50
// 16h00 / 20h00 -> 7h

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
    public int actualLookAt = 0;
    public bool systemIsActive;


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

        //mainCamera.transform.position = startCameraPosition + planets[actualLookAt].transform.position
        //    + mainCamera.transform.forward * zoomAmount;
        //mainCamera.transform.LookAt(planets[actualLookAt].transform);

        mainCamera.transform.LookAt(planets[actualLookAt].transform);


        if (Input.GetKey(KeyCode.LeftArrow)) mainCamera.transform.RotateAround(planets[actualLookAt].transform.position, Vector3.up, 1);
        if (Input.GetKey(KeyCode.RightArrow)) mainCamera.transform.RotateAround(planets[actualLookAt].transform.position, Vector3.up, -1);

        if (Input.GetKeyDown(KeyCode.UpArrow)) ZoomIn(1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) ZoomIn(-1);

        if (Input.GetMouseButtonDown(1) && Input.mousePosition.y > 140) ChangeLookAt(-1);
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > 140) ChangeLookAt(1);

        if (Input.GetKeyDown(KeyCode.Space)) systemIsActive = !systemIsActive;

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
        if (planets != null && planets.Count > 0) return planets[actualLookAt].GetComponent<Planet>();
        else return null;
    }

    public void AdjustCurrentPlanetParameters(PlanetData data)
    {
        planets[actualLookAt].GetComponent<Planet>().AdjustParameters(data);
    }

    void ChangeLookAt(int add)
    {
        actualLookAt += add;
        if (actualLookAt < 0) actualLookAt = planets.Count - 1;
        else if (actualLookAt >= planets.Count) actualLookAt = 0;

        uiHandler.AdjustUIValues();
    }

    void ZoomIn(int add)
    {
        zoomAmount += add;
        //mainCamera.transform.position += mainCamera.transform.forward * zoomAmount;
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
}
