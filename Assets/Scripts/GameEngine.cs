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

    // Start is called before the first frame update
    void Start()
    {
        FileInOut.PopulatePlaybackDataFileDropdown(uiHandler.scenariosDropDown);

        gameState = GameState.LoadingScreen;

        uiHandler.GetComponent<UIHandler>();
        uiHandler.SwitchUIPanel(gameState);
    }

    public void LaunchGame()
    {
        if (gameState == GameState.LoadingScreen)
        { 
            string selectedFileName = FileInOut.performanceDataFiles[uiHandler.scenariosDropDown.value];
            
            GameData gameData = FileInOut.LoadPlanetData(selectedFileName);

            systemHandler.InitSystem(gameData);

            gameState = GameState.InGame;
            uiHandler.SwitchUIPanel(gameState);
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
        }
    }
}
