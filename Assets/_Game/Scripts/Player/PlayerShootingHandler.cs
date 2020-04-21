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
        public AnimationCurve recoilCurve;
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
    private float currentRecoilYAmount = 0f;
    [SerializeField]
    private float currentRecoilXAmount = 0f;

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
        float resetAmount = (playerInput.shoot.IsPressed) ? currentRecoilSettings.recoilAmount * 0.5f : currentRecoilSettings.recoilResetAmount;
        //if (!playerInput.shoot.IsPressed)
        //{
            if (currentRecoilYAmount != 0f)
            {
            
                currentRecoilYAmount = Mathf.MoveTowardsAngle(currentRecoilYAmount, 0f, resetAmount * Time.deltaTime);
            }
            if (currentRecoilXAmount != 0f)
            {
                currentRecoilXAmount = Mathf.MoveTowardsAngle(currentRecoilXAmount, 0f, resetAmount * Time.deltaTime);
            }
        //}

        rotation.x = currentRecoilYAmount;
        rotation.y = currentRecoilXAmount;
        recoilHolder.localRotation = Quaternion.Lerp(recoilHolder.localRotation, Quaternion.Euler(rotation), 25f * Time.deltaTime);
    }

    void AddRecoil()
    {
        float maxRecoilPercentage = -currentRecoilYAmount / currentRecoilSettings.maxRecoil;

        float recoil = currentRecoilSettings.recoilAmount * currentRecoilSettings.recoilCurve.Evaluate(maxRecoilPercentage);

        currentRecoilYAmount -= recoil * (playerInput.aim.IsPressed ? 0.5f : 1f);
        currentRecoilYAmount = Mathf.Clamp(currentRecoilYAmount, -currentRecoilSettings.maxRecoil, 0f);

        currentRecoilXAmount += Random.Range(0, 2) == 0 ? -1f : 1f * recoil/2f * (playerInput.aim.IsPressed ? 0.5f : 1f);
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
