using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderProjectile : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 15f;

    private void OnCollisionEnter(Collision collision)
    {
        var damageHandler = collision.gameObject.GetComponent<DamageHandler>();
        if (damageHandler != null)
        {
            damageHandler.ApplyDamage(gameObject, damageAmount, new DamageHandler.DamageDirectionData(collision.contacts[0].point, -collision.contacts[0].normal, 0f));
            //soundHandler.PlaySound(SoundType.RobotAttack);
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }
}
