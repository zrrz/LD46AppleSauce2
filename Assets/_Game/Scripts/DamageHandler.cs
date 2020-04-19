using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageHandler : MonoBehaviour
{
    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<GameObject, float, Vector3, Vector3> { }

    [SerializeField]
    private bool showDamageNumbers = true;

    public TakeDamageEvent OnDamageTaken;

    private void Start()
    {
        if(OnDamageTaken == null)
        {
            OnDamageTaken = new TakeDamageEvent();
        }
    }

    public void ApplyDamage(GameObject damageSource, float damageAmount, Vector3 hitPoint, Vector3 damageDirection)
    {
        if(showDamageNumbers)
        {
            FloatingDamageTextHandler.CreateFloatingText(FloatingDamageTextHandler.DamageType.Normal, damageAmount, hitPoint + Random.onUnitSphere * 0.25f);
        }
        OnDamageTaken.Invoke(damageSource, damageAmount, hitPoint, damageDirection);
    }
}
