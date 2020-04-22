using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLobberEnemy : MonoBehaviour
{

    [System.Serializable]
    class LegTargetData
    {
        public Transform legTarget;
        public Vector3 defaultPosition;
        [System.NonSerialized]
        public Vector3 lastPosition;
        [System.NonSerialized]
        public bool moving;
        [System.NonSerialized]
        public float legMoveTimer = 0f;
    }

    [SerializeField]
    private LegTargetData backLeftLeg;
    [SerializeField]
    private LegTargetData middleLeftLeg;
    [SerializeField]
    private LegTargetData frontLeftLeg;
    [SerializeField]
    private LegTargetData backRightLeg;
    [SerializeField]
    private LegTargetData middleRightLeg;
    [SerializeField]
    private LegTargetData frontRightLeg;

    [SerializeField]
    private float maxLegDistance = 1f;

    private LegTargetData[] legs;

    [SerializeField]
    public float legMoveTime = 0.5f;

    private Transform target;

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private float attackDistance = 8f;

    [SerializeField]
    private SpiderArm spiderArm;

    [SerializeField]
    private DamageHandler damageHandler;

    [SerializeField]
    private float maxHealth = 50f;
    private float currentHealth;

    private RobotSoundHandler soundHandler;

    float robotMiscSoundsTimer = 0f;

    public bool enemyActive = true;

    void Start()
    {
        soundHandler = GetComponentInChildren<RobotSoundHandler>();
        robotMiscSoundsTimer = Random.Range(5f, 15f);

        //TODO Fix this eventually to not use FindObjectOfType
        target = FindObjectOfType<PlayerMovement>().transform;

        legs = new LegTargetData[] {
            backLeftLeg,
            middleLeftLeg,
            frontLeftLeg,
            backRightLeg,
            middleRightLeg,
            frontRightLeg
        };

        foreach (LegTargetData leg in legs)
        {
            leg.legTarget.position = transform.TransformPoint(leg.defaultPosition);
        }

        legs[0].legTarget.parent.parent = null;

        currentHealth = maxHealth;
        damageHandler.OnDamageTaken.AddListener(TakeDamage);

        characterController.SimpleMove(Vector3.zero);
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

        soundHandler.transform.parent = null;
        soundHandler.PlaySound(SoundType.RobotDeath);
        soundHandler.gameObject.AddComponent<TimedDestroy>().Destroy(2f);
        //TODO particles and stuff
        this.enabled = false;
        PickupManager.SpawnPickup(PickupManager.PickupType.Energy, transform.position, transform.rotation);
        Destroy(gameObject, 0f);
    }

    void Update()
    {
        robotMiscSoundsTimer -= Time.deltaTime;
        if (robotMiscSoundsTimer <= 0f)
        {
            robotMiscSoundsTimer = Random.Range(1f, 5f);
            soundHandler.PlaySound(SoundType.RobotMiscSounds);
        }

        if (!enemyActive)
        {
            return;
        }

        Vector3 direction = (target.position - transform.position);
        direction.y = 0f;
        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 1f);
     
        if(Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            characterController.SimpleMove(transform.forward * moveSpeed);
        }
        else
        {
            if(spiderArm.ReadyToAttack)
            {
                spiderArm.SetTargetThrow();
            }
        }

        foreach (LegTargetData leg in legs)
        {
            if(leg.moving)
            {
                leg.legMoveTimer += Time.deltaTime / legMoveTime;
                Vector3 midPoint = (transform.TransformPoint(leg.defaultPosition) + leg.legTarget.position) / 2f + Vector3.one * 0.25f;
                if (leg.legMoveTimer < 0.5f)
                {
                    leg.legTarget.position = Vector3.Lerp(leg.lastPosition, midPoint, leg.legMoveTimer * 2f);
                }
                else
                {
                    leg.legTarget.position = Vector3.Lerp(midPoint, transform.TransformPoint(leg.defaultPosition), (leg.legMoveTimer - 0.5f) * 2f);
                }
                //leg.legTarget.position = Vector3.Lerp(leg.lastPosition, transform.TransformPoint(leg.defaultPosition), leg.legMoveTimer);
                if (leg.legMoveTimer >= 1f)
                {
                    leg.lastPosition = leg.legTarget.position;
                    leg.moving = false;
                }
            }
            else
            {
                if (Vector3.Distance(transform.TransformPoint(leg.defaultPosition), leg.lastPosition) > maxLegDistance)
                {
                    leg.moving = true;
                    leg.legMoveTimer = 0f;
                    leg.lastPosition = leg.legTarget.position;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.TransformPoint(backLeftLeg.defaultPosition), Vector3.one * 0.15f);
        Gizmos.DrawWireCube(transform.TransformPoint(middleLeftLeg.defaultPosition), Vector3.one * 0.15f);
        Gizmos.DrawWireCube(transform.TransformPoint(frontLeftLeg.defaultPosition), Vector3.one * 0.15f);
        Gizmos.DrawWireCube(transform.TransformPoint(backRightLeg.defaultPosition), Vector3.one * 0.15f);
        Gizmos.DrawWireCube(transform.TransformPoint(middleRightLeg.defaultPosition), Vector3.one * 0.15f);
        Gizmos.DrawWireCube(transform.TransformPoint(frontRightLeg.defaultPosition), Vector3.one * 0.15f);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(backLeftLeg.legTarget.position, Vector3.one * 0.15f);
        Gizmos.DrawWireCube(middleLeftLeg.legTarget.position, Vector3.one * 0.15f);
        Gizmos.DrawWireCube(frontLeftLeg.legTarget.position, Vector3.one * 0.15f);
        Gizmos.DrawWireCube(backRightLeg.legTarget.position, Vector3.one * 0.15f);
        Gizmos.DrawWireCube(middleRightLeg.legTarget.position, Vector3.one * 0.15f);
        Gizmos.DrawWireCube(frontRightLeg.legTarget.position, Vector3.one * 0.15f);
    }
}
