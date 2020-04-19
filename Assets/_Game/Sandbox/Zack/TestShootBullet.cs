using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShootBullet : MonoBehaviour
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

    void Update()
    {
        if (playerInput == null)
        {
            playerInput = PlayerInput.Instance;
        }

        if (autoFire)
        {
            if(shootCooldown > 0f)
            {
                shootCooldown -= Time.deltaTime;
            }
            else
            {
                if (Input.GetButton("Fire1"))
                {
                    shootCooldown = 60f / roundsPerMinute;
                    ShootBullet();
                }
            }
        }
        else
        {
            if(Input.GetButtonDown("Fire1"))
            {
                ShootBullet();
            }
        }
    }

    void ShootBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.Initialize(gameObject, Random.Range(damageAmountMin, damageAmountMax), hitLayerMask, Vector3.forward * shootSpeed);
    }
}
