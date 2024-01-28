using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text highscoreText;
    
    private void Start()
    {
        highscoreText.text = PlayerPrefs.GetInt("highScore", 0).ToString();
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