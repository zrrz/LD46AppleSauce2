using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSoundHandler : SoundHandlerBase
{
    [SerializeField]
    private SoundSettings shootBullet;

    private void Start()
    {
        shootBullet.Initialize(gameObject, soundSettingsMap);
    }
}
