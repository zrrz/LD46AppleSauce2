using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private Slider mouseSensitivitySlider;

    [SerializeField]
    private TMPro.TextMeshProUGUI qualitySettingsText;

    [SerializeField]
    private UnityEngine.Audio.AudioMixerGroup sfxGroup;
    [SerializeField]
    private UnityEngine.Audio.AudioMixerGroup musicGroup;

    void Start()
    {
        float sfxVolume = GameSettingsManager.GetSettingFloat(GameSettingsManager.SettingType.SFXVolume);
        sfxVolumeSlider.value = sfxVolume;
        SFXSliderChanged(sfxVolume);
        

        float musicVolume = GameSettingsManager.GetSettingFloat(GameSettingsManager.SettingType.MusicVolume);
        musicVolumeSlider.value = musicVolume;
        MusicSliderChanged(musicVolume);

        float mouseSensitivity = GameSettingsManager.GetSettingFloat(GameSettingsManager.SettingType.MouseSensitivity);
        mouseSensitivitySlider.value = mouseSensitivity;
        MouseSensitivitySliderChanged(mouseSensitivity);
    }

    public void SFXSliderChanged(float value)
    {
        GameSettingsManager.SaveSettingFloat(GameSettingsManager.SettingType.SFXVolume, value);

        float sfxVolumeScaled = Mathf.Lerp(-80f, 0f, value);
        sfxGroup.audioMixer.SetFloat("FXVolume", sfxVolumeScaled);
    }

    public void MusicSliderChanged(float value)
    {
        GameSettingsManager.SaveSettingFloat(GameSettingsManager.SettingType.MusicVolume, value);

        float musicVolumeScaled = Mathf.Lerp(-80f, 0f, value);
        musicGroup.audioMixer.SetFloat("MusicVolume", musicVolumeScaled);
    }

    public void MouseSensitivitySliderChanged(float value)
    {
        GameSettingsManager.SaveSettingFloat(GameSettingsManager.SettingType.MouseSensitivity, value);
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

    public void BTN_RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        GameManager.Instance.SetMenuState(false);
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
