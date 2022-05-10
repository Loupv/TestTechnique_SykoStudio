using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public PlanetData mainStar;
    public PlanetData[] planetsData;
}

[System.Serializable]
public class PlanetData
{
    public string name;
    public int ID;
    public double diameter, revolutionSpeed, rotationSpeed, distanceFromStar, angle;
    public int colorID;
}

public static class FileInOut
{

    public static List<string> performanceDataFiles;
    public static string currentConfigFileName;

    // Charge un fichier json et retourne un objet Gamedata contenant un PlanetData de mainstar et une liste de PlanetData pour les autres planètes
    public static GameData LoadPlanetData(string jsonName)
    {
        jsonName = "/StreamingAssets/" + jsonName;

        string filePath = Application.dataPath + jsonName;

        Debug.Log("Loading Json at " + filePath);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GameData gameData = JsonUtility.FromJson<GameData>(dataAsJson);
            Debug.Log("GameData JSON loaded successfuly");
            currentConfigFileName = jsonName;
            return gameData;
        }
        else
        {
            Debug.Log("JSON File not found");
            return null;
        }

    }

    // load all existing config files into UI dropdown
    public static List<string> LoadScenarioList(string jsonName)
    {
        TextAsset ta = Resources.Load<TextAsset>(jsonName);
        string dataAsJson = ta.text;

        if (ta != null)
        {
            List<string> scenarios = JsonUtility.FromJson<List<string>>(dataAsJson);

            Debug.Log("ScenariosList JSON loaded successfuly");
            return scenarios;
        }
        else
        {
            Debug.Log("ScenariosList JSON File not found");
            return null;
        }

    }

    // Load le dropdown de l'UI d'accueil avec la liste des fichiers de config trouvés dans le système de fichiers
    public static void PopulatePlaybackDataFileDropdown(TMPro.TMP_Dropdown dropdown)
    {

        dropdown.ClearOptions();
        performanceDataFiles = new List<string>();

        string myPath = Application.dataPath + "/StreamingAssets/";

        DirectoryInfo dir = new DirectoryInfo(myPath);
        FileInfo[] files = dir.GetFiles("*.json*");

        foreach (FileInfo file in files)
        {
            if (!file.Name.Contains(".meta") && !file.Name.Contains(".DS_Store"))
            {
                dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(file.Name.Replace(Application.dataPath, "")));
                performanceDataFiles.Add(file.Name);
            }
        }

        dropdown.value = 0;
        dropdown.RefreshShownValue();

    }

    // Sauvegarde l'ensemble des paramètres du système actuellement affiché dans le fichier initialement chargé
    public static void SavePlanetConfigToFile(GameData gameData)
    {
        string configDataString = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(Application.dataPath + currentConfigFileName, configDataString);
    }
}
    
