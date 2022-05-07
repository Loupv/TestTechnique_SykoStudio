using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public PlanetData mainStar;
    public PlanetData[] planets;
}

[System.Serializable]
public class PlanetData
{
    public int ID;
    public float diameter, revolutionSpeed, rotationSpeed, distanceFromStar, angle;
    public string name, color;
}

public static class FileInOut
{

    public static List<string> performanceDataFiles;


    public static GameData LoadPlanetData(string jsonName)
    {
        if (Application.platform == RuntimePlatform.OSXPlayer) jsonName = "/Resources/Data/" + jsonName;
        string filePath = Application.dataPath + "/StreamingAssets/" + jsonName;

        Debug.Log("Loading Json at " + filePath);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GameData gameData = JsonUtility.FromJson<GameData>(dataAsJson);
            Debug.Log("GameData JSON loaded successfuly");
            return gameData;
        }
        else
        {
            Debug.Log("JSON File not found");
            return null;
        }

    }


    public static List<string> LoadScenarioList(string jsonName)
    {
        TextAsset ta = Resources.Load<TextAsset>(jsonName);
        string dataAsJson = ta.text;

        if (ta != null)
        {
            List<string> scenarios = JsonUtility.FromJson<List<string>>(dataAsJson);

            //string dataAsJson = File.ReadAllText(filePath);
            Debug.Log("ScenariosList JSON loaded successfuly");
            return scenarios;
        }
        else
        {
            Debug.Log("ScenariosList JSON File not found");
            return null;
        }

    }

    public static void PopulatePlaybackDataFileDropdown(TMPro.TMP_Dropdown dropdown)
    {

        dropdown.ClearOptions();
        performanceDataFiles = new List<string>();
        string myPath = "Assets/StreamingAssets";

        DirectoryInfo dir = new DirectoryInfo(myPath);
        FileInfo[] files = dir.GetFiles("*.json*");

        //string[] files = System.IO.levelDirectoryPath.GetFiles("*.csv", SearchOption.AllDirectories);

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
}
    
