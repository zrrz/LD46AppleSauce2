using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderArm : MonoBehaviour
{
    [SerializeField]
    private Transform armThrowerTarget;

    enum TargetType
    {
        Reload,
        Throw
    }

    [System.Serializable]
    class TargetData
    {
        public TargetType targetType;
        public Vector3 position;
        public float time = 1f;
    }

    [SerializeField]
    private TargetData reloadTargetData;

    [SerializeField]
    private TargetData throwTargetData;

    private TargetData currentTarget;

    private Vector3 startPosition;

    private float timer = 0f;

    [SerializeField]
    private GameObject projectileInHand;

    [SerializeField]
    private GameObject projectileInBackpack;

    [SerializeField]
    private GameObject projectilePrefab;

    private float throwCooldown = 0f;

    public bool ReadyToAttack => timer >= 1f && currentTarget.targetType == TargetType.Reload;

    private RobotSoundHandler soundHandler;

    void Start()
    {
        soundHandler = GetComponentInChildren<RobotSoundHandler>();
        armThrowerTarget.position = transform.TransformPoint(reloadTargetData.position);
        currentTarget = reloadTargetData;
        startPosition = Vector3.zero;
        projectileInHand.SetActive(false);
        projectileInBackpack.SetActive(true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SetTargetThrow();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetTargetReload();
        }

        if (timer < 1f)
        {
            timer += Time.deltaTime / currentTarget.time;
            armThrowerTarget.position = Vector3.Lerp(transform.TransformPoint(startPosition), transform.TransformPoint(currentTarget.position), timer);
            if(timer >= 1f)
            {
                
                if(currentTarget.targetType == TargetType.Throw)
                {
                    throwCooldown = 0.5f;
                    projectileInHand.SetActive(false);
                    SpawnProjectile();
                    //TODO spiderLobber.Throw();
                }
                else if(currentTarget.targetType == TargetType.Reload)
                {
                    projectileInHand.SetActive(true);
                    projectileInBackpack.SetActive(false);
                }
            }
        }

        if(throwCooldown > 0f)
        {
            throwCooldown -= Time.deltaTime;
            if(throwCooldown <= 0f)
            {
                if (currentTarget.targetType == TargetType.Throw)
                {
                    SetTargetReload();
                }
            }
        }
    }

    public void SpawnProjectile()
    {
        GameObject projectile = GameObject.Instantiate(projectilePrefab, projectileInHand.transform.position, projectileInHand.transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 300f + transform.up * 100f);
        soundHandler.PlaySound(SoundType.RobotAttack);
    }

    public void SetTargetThrow()
    {
        print("SetTargetThrow");
        startPosition = transform.InverseTransformPoint(armThrowerTarget.position);
        currentTarget = throwTargetData;
        timer = 0f;
    }

    public void SetTargetReload()
    {
        print("SetTargetReload");
        startPosition = transform.InverseTransformPoint(armThrowerTarget.position);
        currentTarget = reloadTargetData;
        timer = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.TransformPoint(reloadTargetData.position), Vector3.one * 0.1f);
        Gizmos.DrawWireCube(transform.TransformPoint(throwTargetData.position), Vector3.one * 0.1f);
    }
}
