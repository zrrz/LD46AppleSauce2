using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSoundHandler : SoundHandlerBase
{
    [SerializeField]
    private SoundSettings robotAttack;

    [SerializeField]
    private SoundSettings robotHurt;

    [SerializeField]
    private SoundSettings robotDeath;

    [SerializeField]
    private SoundSettings robotMiscSounds;

    private void Start()
    {
        robotAttack.Initialize(gameObject, soundSettingsMap);
        robotHurt.Initialize(gameObject, soundSettingsMap);
        robotDeath.Initialize(gameObject, soundSettingsMap);
        robotMiscSounds.Initialize(gameObject, soundSettingsMap);
    }
}
