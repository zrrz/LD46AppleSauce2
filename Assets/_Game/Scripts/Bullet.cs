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
        rigidbody.AddRelativeForce(velocity, ForceMode.Impulse);
        
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
            damageHandler.ApplyDamage(damageSource, damageAmount, collision.contacts[0].point, rigidbody.velocity.normalized);
            trail.transform.parent = null;
            //Destroy(trail, trail.time);
            Destroy(gameObject);

            //TODO destory trail even when bullet script destroys
        }
    }
}
