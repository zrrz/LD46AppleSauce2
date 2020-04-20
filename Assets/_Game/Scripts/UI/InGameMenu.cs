using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    private Slider sfxVolume;

    [SerializeField]
    private Slider musicVolume;

    [SerializeField]
    private TMPro.TextMeshProUGUI qualitySettingsText;

    string sfxVolumePrefsKey = "sfxVolume";
    string musicVolumePrefsKey = "musicVolume";

    [SerializeField]
    private UnityEngine.Audio.AudioMixerGroup sfxGroup;
    [SerializeField]
    private UnityEngine.Audio.AudioMixerGroup musicGroup;

    void Start()
    {
        sfxVolume.value = GetPlayerPrefsFloat(sfxVolumePrefsKey, 0.7f);
        musicVolume.value = GetPlayerPrefsFloat(musicVolumePrefsKey, 0.7f);
    }

    float GetPlayerPrefsFloat(string key, float defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            PlayerPrefs.SetFloat(key, defaultValue);
            PlayerPrefs.Save();
            return defaultValue;
        }
    }

    public void SFXSliderChanged(float value)
    {
        sfxGroup.audioMixer.SetFloat("FXVolume", (value * 80f) - 80f);
        PlayerPrefs.SetFloat(sfxVolumePrefsKey, value);
        PlayerPrefs.Save();
    }

    public void MusicSliderChanged(float value)
    {
        musicGroup.audioMixer.SetFloat("MusicVolume", (value * 80f) - 80f);
        PlayerPrefs.SetFloat(musicVolumePrefsKey, value);
        PlayerPrefs.Save();
    }

    public void BTN_NextQualitySettings()
    {
        int currentLevel = QualitySettings.GetQualityLevel();

        if (currentLevel >= QualitySettings.names.Length - 1)
        {
            QualitySettings.SetQualityLevel(0);
        }
        else
        {
            QualitySettings.IncreaseLevel(true);
        }
        qualitySettingsText.text = "Quality Settings: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void BTN_QuitToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void BTN_ExitGame()
    {
#if UNITY_EDITOR
        if(Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
#endif
        {
            Application.Quit();
        }
    }
}
