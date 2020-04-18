using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShootBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

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
        GameObject bullet = GameObject.Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.AddRelativeForce(Vector3.forward * shootSpeed, ForceMode.Impulse);
    }
}
