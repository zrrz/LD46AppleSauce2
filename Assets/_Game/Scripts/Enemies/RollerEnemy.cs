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
    private float forceAmount = 10f;

    [SerializeField]
    private float velocityChangeSpeed = 20f;

    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private float damageAmount = 10f;

    [SerializeField]
    private float maxHealth = 30f;
    private float currentHealth;

    private RobotSoundHandler soundHandler;

    float robotMiscSoundsTimer = 0f;

    public bool enemyActive = true;

    void Awake()
    {
        soundHandler = GetComponentInChildren<RobotSoundHandler>();
        //TODO Fix this eventually to not use FindObjectOfType
        target = FindObjectOfType<PlayerMovement>().transform;

        currentHealth = maxHealth;
        damageHandler.OnDamageTaken.AddListener(TakeDamage);

        robotMiscSoundsTimer = Random.Range(5f, 15f);
    }

    private void TakeDamage(GameObject damageSource, float damageAmount, DamageHandler.DamageDirectionData damageDirectionData)
    {
        enemyActive = true;

        currentHealth -= damageAmount;
        soundHandler.PlaySound(SoundType.RobotHurt);
        if (currentHealth < 0f)
        {
            Die();
        }
    }

    bool dying = false;
    private void Die()
    {
        if (dying == true)
        {
            return;
        }
        dying = true;
        soundHandler.PlaySound(SoundType.RobotDeath);
        //TODO particles and stuff
        this.enabled = false;
        PickupManager.SpawnPickup(PickupManager.PickupType.Energy, transform.position, transform.rotation);
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        robotMiscSoundsTimer -= Time.deltaTime;
        if(robotMiscSoundsTimer <= 0f)
        {
            robotMiscSoundsTimer = Random.Range(1f, 5f);
            soundHandler.PlaySound(SoundType.RobotMiscSounds);
        }

        if (!enemyActive)
        {
            return;
        }

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
        if(!target)
        {
            return;
        }
        if(collision.gameObject == target.gameObject)
        {
            float velocityStrength = rigidbody.velocity.sqrMagnitude;
            if (velocityStrength > 2f)
            {
                print("Vel strength: " + velocityStrength);
                DamageHandler damageHandler = target.GetComponent<DamageHandler>();
                if(damageHandler != null)
                {
                    float velocityDamage = Mathf.Clamp(damageAmount * velocityStrength/2f, damageAmount, damageAmount * 5f);
                    Vector3 direction = (target.position - transform.position).normalized;
                    var damageDirectionData = new DamageHandler.DamageDirectionData(Vector3.zero, direction, velocityStrength);
                    damageHandler.ApplyDamage(gameObject, velocityDamage, damageDirectionData);
                    soundHandler.PlaySound(SoundType.RobotAttack);
                }
            }
        }
    }
}
