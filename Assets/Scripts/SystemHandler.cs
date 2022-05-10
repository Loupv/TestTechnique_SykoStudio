using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class SystemHandler : MonoBehaviour
{
    public List<GameObject> planets;
    public List<Material> planetMaterials;
    
    GameObject mainStar;
    public UIHandler uiHandler;

    public bool systemIsActive;
    public int targettedPlanetID = 0;

    // Initie l'ensemble du système constitué d'une mainStar et de plusieurs planètes
    public void InitSystem(GameData gameData)
    {
        uiHandler = FindObjectOfType<UIHandler>();

        planets = new List<GameObject>();

        // si aucune data de mainStar n'a été trouvé dans le json
        if (gameData == null)
        {
            gameData = new GameData();
            gameData.mainStar = new PlanetData(0, 4, 0, 5, 0, "MainStar", 0);
        }

        // si le nombre de planète n'est pas dans les limites autorisées
        if (gameData.planetsData != null && gameData.planetsData.Length > 11)
        {
            PlanetData[] newPlanetData = new PlanetData[10];
            Array.Copy(gameData.planetsData, newPlanetData, 10);
            gameData.planetsData = newPlanetData;
        }
        else if (gameData.planetsData == null || gameData.planetsData.Length < 1)
        {
            gameData.planetsData = new PlanetData[1];
            gameData.planetsData[0] = new PlanetData(1);
        }

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

    // Actualise le systeme si systemIsActive est vrai
    public void UpdateSystem()
    {
        if (systemIsActive) {
            ActualizePlanetsTransform();
        }

        if (Input.GetKeyDown(KeyCode.Space)) systemIsActive = !systemIsActive;
    }


    // Actualise les Transform des GameObject liés au objets Planet
    void ActualizePlanetsTransform()
    {
        foreach (GameObject planetObj in planets)
        {
            Planet planet = planetObj.GetComponent<Planet>();
            planet._angle += Time.deltaTime * planet._revolutionSpeed * Time.deltaTime;
            planet.transform.position = new Vector3(Mathf.Cos((float)planet._angle * 2 * Mathf.PI) * (float)planet._distanceFromStar, 0, Mathf.Sin((float)planet._angle * 2 * Mathf.PI) * (float)planet._distanceFromStar);
            planet.transform.Rotate(Vector3.up, (float)planet._rotationSpeed * Time.deltaTime);
        }
    }

    // Change l'ID de la Planet actuellement ciblée
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

    // Retourne l'objet Planet actuellement ciblé
    public Planet GetCurrentlyFocusedPlanet()
    {
        if (planets.Count > 0) return planets[targettedPlanetID].GetComponent<Planet>();
        else return null;
    }

}
