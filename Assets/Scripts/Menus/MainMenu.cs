using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text highscoreText;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        highscoreText.text = PlayerPrefs.GetInt("highScore", 0).ToString();
        //fill dropdown with resolutions
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        foreach (var resolution in Screen.resolutions)
        {
            options.Add(resolution.width + "x" + resolution.height);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", options.Count - 1);
        SetResolution(resolutionDropdown.value);
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        SetFullscreen(fullscreenToggle.isOn);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int index)
    {
        var resolution = Screen.resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", index);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}