using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet : MonoBehaviour
{

    public int _ID;
    public double _diameter, _revolutionSpeed, _rotationSpeed, _distanceFromStar, _angle;
    public string _name;
    public int _colorID;

    // Initialisation de l'objet Planet
    public void Init(PlanetData data, Material mat)
    {
        _ID = data.ID;
        _name = data.name;
        _diameter = data.diameter;
        _revolutionSpeed = data.revolutionSpeed;
        _rotationSpeed = data.rotationSpeed;
        _distanceFromStar = data.distanceFromStar;
        _colorID = data.colorID;
        _angle = Random.Range(-Mathf.PI, Mathf.PI);

        gameObject.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        gameObject.transform.localScale = new Vector3((float)_diameter, (float)_diameter, (float)_diameter);
        gameObject.name = _name;

        gameObject.transform.parent = GameObject.FindWithTag("PlanetsParent").transform;

        gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().material = mat;
    }

   

    // Renvoie la liste des données de l'instance Planet au format PlanetData
    public PlanetData GetPlanetData()
    {
        PlanetData data = new PlanetData(_ID);
        data.diameter = _diameter;
        data.revolutionSpeed = _revolutionSpeed;
        data.rotationSpeed = _rotationSpeed;
        data.distanceFromStar = _distanceFromStar;
        data.name = _name;
        data.colorID = _colorID;
        return data;
    }

    // Applique à l'instance Planet un nouveau set de données PlanetData
    public void AdjustParameters(PlanetData data)
    {
        _diameter = data.diameter;
        _revolutionSpeed = data.revolutionSpeed;
        _rotationSpeed = data.rotationSpeed;
        _distanceFromStar = data.distanceFromStar;

        gameObject.transform.localScale = new Vector3((float)_diameter, (float)_diameter, (float)_diameter);

    }

    public void ChangePlanetMaterial(Material mat, int newColorID)
    {
        _colorID = newColorID;
        GetComponent<MeshRenderer>().material = mat;
    }


}
