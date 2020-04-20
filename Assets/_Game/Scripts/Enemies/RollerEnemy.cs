using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerEnemy : MonoBehaviour
{
    [SerializeField]
    private DamageHandler damageHandler;

    [SerializeField]
    private new Rigidbody rigidbody;

    Transform target;

    [SerializeField]
    private float forceAmount = 100f;

    [SerializeField]
    private float velocityChangeSpeed = 50f;

    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private float damageAmount = 10f;

    [SerializeField]
    private float health = 100f;

    void Start()
    {
        //TODO Fix this eventually to not use FindObjectOfType
        target = FindObjectOfType<PlayerMovement>().transform;

        damageHandler.OnDamageTaken.AddListener(TakeDamage);
    }

    private void TakeDamage(GameObject damageSource, float damageAmount, DamageHandler.DamageDirectionData damageDirectionData)
    {
        health -= damageAmount;
        if(health < 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        //TODO particles and stuff
        Destroy(gameObject);
    }

    void Update()
    {
        Vector3 direction = (target.position - transform.position);
        if(direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        direction *= forceAmount;

        velocity = Vector3.MoveTowards(velocity, direction, velocityChangeSpeed * Time.deltaTime);

        rigidbody.AddForce(velocity * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject == target.gameObject)
        {
            float velocityStrength = rigidbody.velocity.sqrMagnitude;
            if (velocityStrength > 2f)
            {
                print("Vel strength: " + velocityStrength);
                DamageHandler damageHandler = target.GetComponent<DamageHandler>();
                if(damageHandler != null)
                {
                    float velocityDamage = Mathf.Clamp(damageAmount * velocityStrength, damageAmount, damageAmount * 10f);
                    Vector3 direction = (target.position - transform.position).normalized;
                    var damageDirectionData = new DamageHandler.DamageDirectionData(Vector3.zero, direction, velocityStrength);
                    damageHandler.ApplyDamage(gameObject, velocityDamage, damageDirectionData);
                    print("Hit: " + target.name);
                }
            }
        }
    }
}
