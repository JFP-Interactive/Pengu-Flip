using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
            Load();
        }
        else
        {
            Load();
        }
        ChangeVolume();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = musicVolumeSlider.value;
        Save();
    }

    private void Load()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
    }
}