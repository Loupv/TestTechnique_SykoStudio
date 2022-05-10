using UnityEngine;
using UnityEngine.UI;

// Gère l'ensemble des affichages de l'UI
public class UIHandler : MonoBehaviour
{
    public SystemHandler systemHandler;
    public GameObject loadingScreenPanel, inGamePanel;

    public Vector2 rotationSpeedExtrema, sizeExtrema, revolutionSpeedExtrema, distanceExtrema;

    public TMPro.TMP_Dropdown scenariosDropDown;

    public Slider rotationSpeedSlider, sizeSlider, revolutionSpeedSlider, distanceSlider;
    public Button switchMaterialButton;
    public TMPro.TMP_Text planetNameTextField;

    // Initie les valeurs min et max des Sliders
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

    // Applique les changements d'UI à la Planet ciblée
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

    // Alterne entre les deux panels d'UI
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

    // Ajuste les sliders de l'UI lorsque l'objet Planet ciblé change
    public void AdjustUIValues()
    {
        Planet currentPlanet = systemHandler.GetCurrentlyFocusedPlanet();

        if (currentPlanet != null)
        {
            PlanetData newData = currentPlanet.GetPlanetData();
            rotationSpeedSlider.value = (float)newData.rotationSpeed;
            sizeSlider.value = (float)newData.diameter;
            revolutionSpeedSlider.value = (float)newData.revolutionSpeed;
            distanceSlider.value = (float)newData.distanceFromStar;
            planetNameTextField.text = newData.name;
        }

        // si on est face à mainstar, on enlève les contrôles de distance et de révolution
        if(currentPlanet._ID == 0)
        {
            revolutionSpeedSlider.gameObject.SetActive(false);
            distanceSlider.gameObject.SetActive(false);
        }
        else
        {
            revolutionSpeedSlider.gameObject.SetActive(true);
            distanceSlider.gameObject.SetActive(true);
        }
    }

    // Methode liée au bouton SaveConfig, enregistre les paramètres actuels dans le fichier de configuration courant
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

        FileInOut.SavePlanetConfigToFile(newGameData);
    }



}
