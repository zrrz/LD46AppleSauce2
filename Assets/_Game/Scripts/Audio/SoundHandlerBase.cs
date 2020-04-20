using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandlerBase : MonoBehaviour
{
    protected Dictionary<SoundType, SoundSettings> soundSettingsMap = new Dictionary<SoundType, SoundSettings>();

    public void PlaySound(SoundType soundType)
    {
        if (soundSettingsMap.ContainsKey(soundType))
        {
            var soundSettings = soundSettingsMap[soundType];
            soundSettings.PlaySound();
        }
        else
        {
            Debug.LogError($"There is no {soundType} on {gameObject.name}", this);
        }
    }
}
