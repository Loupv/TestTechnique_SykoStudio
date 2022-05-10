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
    public List<Material> planetMaterials;
    
    GameObject mainStar;
    public UIHandler uiHandler;

    public bool systemIsActive;

    public int targettedPlanetID = 0;

    public void InitSystem(GameData gameData)
    {
        uiHandler = FindObjectOfType<UIHandler>();

        
        planets = new List<GameObject>();

        // Init Star
        mainStar = new GameObject();
        mainStar.gameObject.AddComponent<Planet>();
        Material mat = PickRightMaterial(gameData.mainStar.colorID);
        mainStar.GetComponent<Planet>().Init(gameData.mainStar, mat);
        planets.Add(mainStar);

        // Init Planets
        for (int i = 0; i < gameData.planetsData.Length; i++)
        {
            GameObject newPlanet = new GameObject();
            newPlanet.AddComponent<Planet>();

            mat = PickRightMaterial(gameData.planetsData[i].colorID);
            newPlanet.GetComponent<Planet>().Init(gameData.planetsData[i], mat);

            planets.Add(newPlanet);
        }

        systemIsActive = true;
    }


    public void UpdateSystem()
    {

        if (systemIsActive) {
            ActualizePlanetsTransform();
        }


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


    public void ChangeLookAt(int add)
    {
        targettedPlanetID += add;
        if (targettedPlanetID < 0) targettedPlanetID = planets.Count - 1;
        else if (targettedPlanetID >= planets.Count) targettedPlanetID = 0;
    }


    Material PickRightMaterial(int colID)
    {
        return planetMaterials[colID];
    }

    public void AdjustCurrentPlanetParameters(PlanetData data)
    {
        planets[targettedPlanetID].GetComponent<Planet>().AdjustParameters(data);
    }

    public void PickNextMaterialForCurrentPlanet()
    {
        int colorID = planets[targettedPlanetID].GetComponent<Planet>()._colorID + 1;

        if (colorID >= planetMaterials.Count) colorID = 0;
        planets[targettedPlanetID].GetComponent<Planet>().ChangePlanetMaterial(PickRightMaterial(colorID), colorID);
        planets[targettedPlanetID].GetComponent<Planet>()._colorID = colorID;
    }

    public Planet GetCurrentlyFocusedPlanet()
    {
        if (planets.Count > 0) return planets[targettedPlanetID].GetComponent<Planet>();
        else return null;
    }

}
