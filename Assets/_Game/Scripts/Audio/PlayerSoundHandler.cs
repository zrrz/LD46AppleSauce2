using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : SoundHandlerBase
{
    [SerializeField]
    private SoundSettings playerFootsteps;

    [SerializeField]
    private SoundSettings playerHurt;

    [SerializeField]
    private SoundSettings playerPickupGrab;

    [SerializeField]
    private SoundSettings playerDeath;

    private void Start()
    {
        playerFootsteps.Initialize(gameObject, soundSettingsMap);
        playerHurt.Initialize(gameObject, soundSettingsMap);
        playerPickupGrab.Initialize(gameObject, soundSettingsMap);
        playerDeath.Initialize(gameObject, soundSettingsMap);
    }
}
