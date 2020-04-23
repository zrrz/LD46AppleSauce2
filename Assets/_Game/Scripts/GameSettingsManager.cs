using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager
{
    public abstract class Setting<T>
    {
        public SettingType settingType;
        protected bool cached = false;
        protected T cachedValue;
        public abstract T GetValue();
        public abstract void SetValue(T value);
    }

    public class FloatSetting : Setting<float>
    {
        public FloatSetting(SettingType settingType, float defaultValue)
        {
            this.settingType = settingType;

            if (PlayerPrefs.HasKey(settingType.ToString()))
            {
                cachedValue = PlayerPrefs.GetFloat(settingType.ToString());
            }
            else
            {
                PlayerPrefs.SetFloat(settingType.ToString(), defaultValue);
                PlayerPrefs.Save();
                cachedValue = defaultValue;
            }
            settingMap.Add(settingType, this);
        }

        public override float GetValue()
        {
            if(!cached)
            {
                cachedValue = PlayerPrefs.GetFloat(settingType.ToString());
                cached = true;
            }

            return cachedValue;
        }

        public override void SetValue(float value)
        {
            cached = false;
            PlayerPrefs.SetFloat(settingType.ToString(), value);
            PlayerPrefs.Save();
        }
    }

    public enum SettingType
    {
        SFXVolume,
        MusicVolume,
        MouseSensitivity
    }

    //Object is gross, but I'm over it
    private static Dictionary<SettingType, object> settingMap = new Dictionary<SettingType, object>();

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        FloatSetting sfxVolumeSetting = new FloatSetting(SettingType.SFXVolume, 0.7f);
        FloatSetting musicVolumeSetting = new FloatSetting(SettingType.MusicVolume, 0.7f);
        FloatSetting mouseSensitivitySetting = new FloatSetting(SettingType.MouseSensitivity, 0.5f);
    }

    public static float GetSettingFloat(SettingType settingType)
    {
        return ((FloatSetting)settingMap[settingType]).GetValue();
    }

    public static void SaveSettingFloat(SettingType settingType, float value)
    {
        ((FloatSetting)settingMap[settingType]).SetValue(value);
    }
}
