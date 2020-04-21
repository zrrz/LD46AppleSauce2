using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSettings
{
    public SoundType soundType;
    public MixerGroupType mixerGroupType;
    public AudioClip[] audioClips;
    public float volume = 1f;
    public float radius = 50f;
    [Range(0f, 1f)]
    [Tooltip("0.0 for 2D. 1.0 for 3D")]
    public float spatialBlend = 1f;
    public AudioRolloffMode rolloffMode;

    //TODO Randomness

    [System.NonSerialized]
    public AudioSource audioSource;

    public void Initialize(GameObject ownerObject, Dictionary<SoundType, SoundSettings> soundSettingsMap)
    {
        if (this.audioSource == null)
        {
            var audioSource = ownerObject.AddComponent<AudioSource>();
            audioSource.volume = this.volume;
            audioSource.spatialBlend = this.spatialBlend;
            audioSource.outputAudioMixerGroup = (this.mixerGroupType == MixerGroupType.SFX ? GameManager.SfxMixerGroup : GameManager.MusicMixerGroup);
            audioSource.maxDistance = this.radius;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.rolloffMode = this.rolloffMode;
            this.audioSource = audioSource;
            if(!soundSettingsMap.ContainsKey(this.soundType))
            {
                soundSettingsMap.Add(this.soundType, this);
            }
            else
            {
                Debug.LogError($"{ownerObject.name} already has a SoundSetting with {this.soundType} set");
            }
        }
    }

    public void PlaySound()
    {
        if (audioClips.Length > 0)
        {
            var audioClip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No sound clips in: " + soundType);
        }
    }
}

public enum MixerGroupType
{
    Music, SFX
}

public enum SoundType
{
    PlayerFootstep, 
    PlayerHurt, 
    PlayerGrabPickup, 
    PlayerDeath,
    GunShoot,
    GunBulletImpactEnvironment,
    BabyFeedEnergy,
    RobotMiscSounds,
    RobotAttack,
    RobotHurt,
    RobotDeath,
    GameFlowNewGame,
    GunBulletImpactEnemy,
}