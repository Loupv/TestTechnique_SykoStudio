using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public SystemHandler systemHandler;
    public GameObject loadingScreenPanel, inGamePanel;

    public Vector2 rotationSpeedExtrema, sizeExtrema, revolutionSpeedExtrema, distanceExtrema;


    //public Dropdown scenariosDropDown;
    public TMPro.TMP_Dropdown scenariosDropDown;

    public Slider rotationSpeedSlider, sizeSlider, revolutionSpeedSlider, distanceSlider;
    public TMPro.TMP_Text planetNameTextField;

    private void Start()
    {
        rotationSpeedSlider.minValue = rotationSpeedExtrema.x;
        rotationSpeedSlider.maxValue = rotationSpeedExtrema.y;
        sizeSlider.minValue = sizeExtrema.x;
        sizeSlider.maxValue = sizeExtrema.y;
        revolutionSpeedSlider.minValue = revolutionSpeedExtrema.x;
        revolutionSpeedSlider.maxValue = revolutionSpeedExtrema.y;
        distanceSlider.minValue = distanceExtrema.x;
        distanceSlider.maxValue = distanceExtrema.y;
    }

    public void OnSliderValueChanged(Slider currentSlider)
    {
        Planet currentPlanet = systemHandler.GetCurrentlyFocusedPlanet();

        if(currentPlanet != null)
        {
            PlanetData newData = currentPlanet.GetPlanetData();
            if (currentSlider.name.Contains("RotationSpeed")) newData.rotationSpeed = currentSlider.value;
            else if (currentSlider.name.Contains("RevolutionSpeed")) newData.revolutionSpeed = currentSlider.value;
            else if (currentSlider.name.Contains("Size")) newData.diameter = currentSlider.value;
            else if (currentSlider.name.Contains("DistanceFromStar")) newData.distanceFromStar = currentSlider.value;
            systemHandler.AdjustCurrentPlanetParameters(newData);
        }

    }


    public void SwitchUIPanel(GameState gameState)
    {
        if(gameState == GameState.LoadingScreen)
        {
            loadingScreenPanel.SetActive(true);
            inGamePanel.SetActive(false);
        }
        else if(gameState == GameState.InGame)
        {
            loadingScreenPanel.SetActive(false);
            inGamePanel.SetActive(true);
        }
    }


    public void AdjustUIValues()
    {
        Planet currentPlanet = systemHandler.GetCurrentlyFocusedPlanet();

        if (currentPlanet != null)
        {
            PlanetData newData = currentPlanet.GetPlanetData();
            rotationSpeedSlider.value = newData.rotationSpeed;
            sizeSlider.value = newData.diameter;
            revolutionSpeedSlider.value = newData.revolutionSpeed;
            distanceSlider.value = newData.distanceFromStar;
            planetNameTextField.text = newData.name;
        }
    }


    public void SaveConfigDataToFile()
    {
        GameData newGameData = new GameData();
        newGameData.mainStar = systemHandler.planets[0].GetComponent<Planet>().GetPlanetData();
        Debug.Log("Saving planet : " + newGameData.mainStar.name);

        newGameData.planetsData = new PlanetData[systemHandler.planets.Count-1];

        for(int i = 1; i < newGameData.planetsData.Length + 1; i++)
        {
            newGameData.planetsData[i-1] = systemHandler.planets[i].GetComponent<Planet>().GetPlanetData();
            Debug.Log("Saving planet : " + newGameData.planetsData[i-1].name);
        }

        FileInOut.SavePlanetConfigToFile(newGameData, "jsonTest.json");
    }



}
