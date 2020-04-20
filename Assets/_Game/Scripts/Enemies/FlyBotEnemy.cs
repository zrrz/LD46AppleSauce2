using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBotEnemy : MonoBehaviour
{
    [SerializeField]
    private float maxSpinSpeed = 360f;

    [SerializeField]
    private float spinRecoverSpeed = 120f;

    [SerializeField] //TEMP
    private float currentSpinSpeed = 0f;

    [SerializeField]
    private float desiredHeight = 4f;

    private Vector3? recoverPosition = null;

    [SerializeField]
    private float heightRaiseSpeed = 1.3f;

    [SerializeField]
    private float maxMoveSpeed = 2f;

    //private float currentMoveSpeed;

    [SerializeField]
    private LayerMask groundMask;

    private Transform target;

    [SerializeField]
    Rigidbody rigidbody;

    [SerializeField]
    private Transform bladesVis;

    [SerializeField]
    private float velocityCollisionSpeed = 4f;

    [SerializeField]
    private float damageAmount = 12f;

    [SerializeField]
    private float maxHealth = 25f;
    private float currentHealth;

    [SerializeField]
    private DamageHandler damageHandler;

    enum State
    {
        Recover,
        Attack,
        Fall
    }

    private State state = State.Recover;

    float fallTimer = 0f;

    private RobotSoundHandler soundHandler;

    float robotMiscSoundsTimer = 0f;

    void Start()
    {
        //TODO dont use FindObjectOfType    
        target = FindObjectOfType<PlayerMovement>().transform;

        currentHealth = maxHealth;
        damageHandler.OnDamageTaken.AddListener(TakeDamage);
        robotMiscSoundsTimer = Random.Range(5f, 15f);
    }

    private void TakeDamage(GameObject damageSource, float damageAmount, DamageHandler.DamageDirectionData damageDirectionData)
    {
        currentHealth -= damageAmount;
        soundHandler.PlaySound(SoundType.RobotHurt);
        if (currentHealth < 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        soundHandler.PlaySound(SoundType.RobotDeath);
        Fall();
        //TODO particles and stuff
        this.enabled = false;
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        robotMiscSoundsTimer -= Time.deltaTime;
        if (robotMiscSoundsTimer <= 0f)
        {
            robotMiscSoundsTimer = Random.Range(5f, 15f);
            soundHandler.PlaySound(SoundType.RobotMiscSounds);
        }

        bladesVis.Rotate(0f, 0f, currentSpinSpeed * Time.deltaTime, Space.Self);

        //TEMP
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Fall();
        }

        switch (state)
        {
            case State.Fall:
                if(fallTimer > 0f)
                {
                    fallTimer -= Time.deltaTime;
                    currentSpinSpeed = Mathf.MoveTowards(currentSpinSpeed, 0f, spinRecoverSpeed * Time.deltaTime * 3f);
                }
                else
                {
                    state = State.Recover;
                    rigidbody.useGravity = false;
                    rigidbody.angularVelocity = Vector3.zero;
                    rigidbody.velocity = Vector3.zero;
                }
                break;
            case State.Recover:
                if (currentSpinSpeed < maxSpinSpeed)
                {
                    currentSpinSpeed = Mathf.MoveTowards(currentSpinSpeed, maxSpinSpeed, spinRecoverSpeed * Time.deltaTime);
                }

                bool recovered = true;
                if (currentSpinSpeed < maxSpinSpeed / 2f)
                {
                    recovered = false;
                }
                if (recoverPosition == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, Vector3.down * desiredHeight, out hit, desiredHeight, groundMask))
                    {
                        recoverPosition = hit.point + Vector3.up * desiredHeight;
                    }
                }
                else
                {
                    Vector3 recoverTargetDirection = (recoverPosition.Value - transform.position);
                    if (recoverTargetDirection.sqrMagnitude > 1f)
                    {
                        recoverTargetDirection.Normalize();
                    }

                    if (Vector3.Dot(transform.up, Vector3.up) < 0.95f)
                    {
                        recovered = false;
                    }
                    transform.up = Vector3.Lerp(transform.up, (Vector3.up + recoverTargetDirection * 0.5f).normalized, Time.deltaTime * 3f);

                    bool belowRecoverHeight = transform.position.y < recoverPosition.Value.y;
                    if (belowRecoverHeight)
                    {
                        recovered = false;
                    }
                    
                    Vector3 targetVelocity = (belowRecoverHeight ? 1f : -1f) * transform.up * -Physics.gravity.y * heightRaiseSpeed + recoverTargetDirection * 4f;

                    rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, targetVelocity, Time.deltaTime);
                    rigidbody.angularVelocity = Vector3.zero;
                }
                if(recovered)
                {
                    recoverPosition = null;
                    state = State.Attack;
                }
                break;
            case State.Attack:
                if (currentSpinSpeed < maxSpinSpeed)
                {
                    currentSpinSpeed = Mathf.MoveTowards(currentSpinSpeed, maxSpinSpeed, spinRecoverSpeed * Time.deltaTime);
                }

                Vector3 targetDirection = (target.position + Vector3.up/2f - transform.position);
                if (targetDirection.sqrMagnitude > 1f)
                {
                    targetDirection.Normalize();
                }

                Debug.DrawRay(transform.position, transform.up, Color.blue);
                Debug.DrawRay(transform.position, (Vector3.up + targetDirection * 0.5f).normalized, Color.red);

                transform.up = Vector3.Lerp(transform.up, (Vector3.up + targetDirection * 0.5f).normalized, Time.deltaTime * 3f);
                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, targetDirection * maxMoveSpeed, Time.deltaTime);
                rigidbody.angularVelocity = Vector3.zero;

                break;
            default:
                break;
        }
    }

    private void Fall()
    {
        state = State.Fall;
        rigidbody.useGravity = true;
        fallTimer = 2f;
    }

    private void OnDrawGizmosSelected()
    {
        if(recoverPosition.HasValue)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(recoverPosition.Value, 0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageHandler = collision.gameObject.GetComponent<DamageHandler>();
        if (damageHandler != null)
        {
            damageHandler.ApplyDamage(gameObject, damageAmount, new DamageHandler.DamageDirectionData(collision.contacts[0].point, -collision.contacts[0].normal, 0f));
            soundHandler.PlaySound(SoundType.RobotAttack);
        }
        else
        {
            if (rigidbody.velocity.sqrMagnitude > velocityCollisionSpeed)
            {
                Fall();
            }
        }
    }
}
