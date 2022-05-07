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
}
