using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSoundHandler : SoundHandlerBase
{
    [SerializeField]
    private SoundSettings bulletImpactEnvironment;

    [SerializeField]
    private SoundSettings bulletImpactEnemy;

    private void Start()
    {
        bulletImpactEnvironment.Initialize(gameObject, soundSettingsMap);
        bulletImpactEnemy.Initialize(gameObject, soundSettingsMap);
    }
}

