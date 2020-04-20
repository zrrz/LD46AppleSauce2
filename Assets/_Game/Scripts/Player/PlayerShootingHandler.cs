using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingHandler : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private Transform shootPoint;

    [SerializeField]
    private float shootSpeed = 80f;

    [SerializeField]
    private bool autoFire = false;

    [SerializeField]
    private float roundsPerMinute = 60f;

    private float shootCooldown = 0f;

    PlayerInput playerInput;

    [SerializeField]
    LayerMask hitLayerMask;

    [SerializeField]
    private int damageAmountMin = 2;
    
    [SerializeField]
    private int damageAmountMax = 5;

    [SerializeField]
    private float bulletSpreadRadius = 1f;

    [System.Serializable]
    class RecoilSettings
    {
        public float recoilAmount = 5f;
        public float maxRecoil = 45f;
        public float recoilResetAmount = 20f;
    }

    [Header("Recoil Settings")]
    [SerializeField]
    Transform recoilHolder;

    [SerializeField]
    private RecoilSettings uziRecoilSettings = new RecoilSettings();
    private RecoilSettings currentRecoilSettings;
    [SerializeField]
    private float currentRecoilAmount = 0f;

    private GunSoundHandler gunSoundHandler;

    private void Start()
    {
        gunSoundHandler = GetComponentInChildren<GunSoundHandler>();
        currentRecoilSettings = uziRecoilSettings;
    }

    void Update()
    {
        if (playerInput == null)
        {
            playerInput = PlayerInput.Instance;
        }

        if(!playerInput.sprint.IsPressed)
        {
            HandleShooting();
        }

        HandleRecoil();
    }

    private void HandleShooting()
    {
        if (autoFire)
        {
            if (shootCooldown > 0f)
            {
                shootCooldown -= Time.deltaTime;
            }
            else
            {
                if (playerInput.shoot.IsPressed)
                {
                    shootCooldown = 60f / roundsPerMinute;
                    ShootBullet();
                }
            }
        }
        else
        {
            if (playerInput.shoot.WasPressed)
            {
                ShootBullet();
            }
        }
    }

    private void HandleRecoil()
    {
        //TODO check diff against epsilon
        Vector3 rotation = recoilHolder.localEulerAngles;
        if(currentRecoilAmount != 0f)
        {
            if(!playerInput.shoot.IsPressed)
            {
                currentRecoilAmount = Mathf.MoveTowards(currentRecoilAmount, 0f, currentRecoilSettings.recoilResetAmount * Time.deltaTime);
            }

        }

        rotation.x = currentRecoilAmount;
        recoilHolder.localRotation = Quaternion.Lerp(recoilHolder.localRotation, Quaternion.Euler(rotation), 25f * Time.deltaTime);
    }

    void AddRecoil()
    {
        currentRecoilAmount -= currentRecoilSettings.recoilAmount * (playerInput.aim.IsPressed ? 0.3f : 1f);
        //TODO max recoil
    }

    private void ShootBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Vector3 bulletVelocity = shootPoint.forward * shootSpeed 
            + shootPoint.right * Random.Range(-bulletSpreadRadius, bulletSpreadRadius) 
            + shootPoint.up * Random.Range(-bulletSpreadRadius, bulletSpreadRadius);
        bullet.Initialize(gameObject, Random.Range(damageAmountMin, damageAmountMax), hitLayerMask, bulletVelocity);

        gunSoundHandler.PlaySound(SoundType.GunShoot);

        AddRecoil();
    }
}
