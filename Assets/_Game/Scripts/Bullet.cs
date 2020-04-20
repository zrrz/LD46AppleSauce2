using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    new Rigidbody rigidbody;

    private GameObject damageSource;

    private float damageAmount;

    [SerializeField]
    private TrailRenderer trail;

    //TODO use layermask


    public void Initialize(GameObject damageSource, float damageAmount, LayerMask hitLayerMask, Vector3 velocity)
    {
        rigidbody.AddForce(velocity, ForceMode.Impulse);
        
        this.damageSource = damageSource;
        this.damageAmount = damageAmount;

        //This is bad code
        gameObject.layer = damageSource.layer;

        Destroy(gameObject, 6f); //Destory bullet after 6 seconds if you shoot into space
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageHandler = collision.gameObject.GetComponent<DamageHandler>();
        if (damageHandler != null)
        {
            var damageDirectionData = new DamageHandler.DamageDirectionData(collision.contacts[0].point, rigidbody.velocity.normalized, 0f);
            damageHandler.ApplyDamage(damageSource, damageAmount, damageDirectionData);
            GetComponentInChildren<BulletSoundHandler>().PlaySound(SoundType.GunBulletImpactEnemy);
        }
        else
        {
            GetComponentInChildren<BulletSoundHandler>().PlaySound(SoundType.GunBulletImpactEnvironment);
        }
        trail.transform.parent = null;
        trail.gameObject.AddComponent<TimedDestroy>().Destroy(trail.time); //Sorry for this code
        Destroy(gameObject);
    }
}
