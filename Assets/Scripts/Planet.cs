using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet : MonoBehaviour
{

    public int _ID;
    public float _diameter, _revolutionSpeed, _rotationSpeed, _distanceFromStar, _angle;
    public string _name;
    public int _colorID;

    //public Material currentMaterial;

    public void Init(PlanetData data, Material mat)
    {
        _ID = data.ID;
        _name = data.name;
        _diameter = data.diameter;
        _revolutionSpeed = data.revolutionSpeed;
        _rotationSpeed = data.rotationSpeed;
        _distanceFromStar = data.distanceFromStar;
        _colorID = data.colorID;
        _angle = 0;
        //obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        gameObject.transform.localScale = new Vector3(_diameter, _diameter, _diameter);
        gameObject.name = _name;

        gameObject.transform.parent = GameObject.FindWithTag("PlanetsParent").transform;

        //currentMaterial = mat;

        gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().material = mat;
    }


    public PlanetData GetPlanetData()
    {
        PlanetData data = new PlanetData();
        data.ID = _ID;
        data.diameter = _diameter;
        data.revolutionSpeed = _revolutionSpeed;
        data.rotationSpeed = _rotationSpeed;
        data.distanceFromStar = _distanceFromStar;
        data.name = _name;
        data.colorID = _colorID;
        return data;
    }

    public void AdjustParameters(PlanetData data)
    {
        _diameter = data.diameter;
        _revolutionSpeed = data.revolutionSpeed;
        _rotationSpeed = data.rotationSpeed;
        _distanceFromStar = data.distanceFromStar;

        gameObject.transform.localScale = new Vector3(_diameter, _diameter, _diameter);

    }

    public void ChangePlanetMaterial(Material mat, int newColorID)
    {
        _colorID = newColorID;
        GetComponent<MeshRenderer>().material = mat;
    }


}
