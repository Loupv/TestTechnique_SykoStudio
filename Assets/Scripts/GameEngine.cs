using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    LoadingScreen, InGame
}

public class GameEngine : MonoBehaviour
{

    public GameState gameState;
    public UIHandler uiHandler;
    public SystemHandler systemHandler;
    public CameraHandler cameraHandler;

    public int refreshRateTarget = 30;

    // Start is called before the first frame update
    void Start()
    {
        FileInOut.PopulatePlaybackDataFileDropdown(uiHandler.scenariosDropDown);

        gameState = GameState.LoadingScreen;

        uiHandler.GetComponent<UIHandler>();
        uiHandler.SwitchUIPanel(gameState);

        Application.targetFrameRate = refreshRateTarget;
    }

    public void LaunchGame()
    {
        if (gameState == GameState.LoadingScreen)
        { 
            string selectedFileName = FileInOut.performanceDataFiles[uiHandler.scenariosDropDown.value];
            
            GameData gameData = FileInOut.LoadPlanetData(selectedFileName);

            systemHandler.InitSystem(gameData);

            cameraHandler.InitCameraSystem(systemHandler.planets.Count);

            gameState = GameState.InGame;
            uiHandler.SwitchUIPanel(gameState);

            ChangeLookAt(0);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(gameState == GameState.LoadingScreen)
        {
        }
        else if(gameState == GameState.InGame)
        {
            systemHandler.UpdateSystem();
            cameraHandler.UpdateCamera(systemHandler.planets[systemHandler.targettedPlanetID].transform);

            if (Input.GetMouseButtonDown(1) && Input.mousePosition.y > 140) ChangeLookAt(-1);
            if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > 140) ChangeLookAt(1);

        }
    }


    void ChangeLookAt(int add)
    {
        systemHandler.ChangeLookAt(add);
        uiHandler.AdjustUIValues();
    }
}
