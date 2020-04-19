using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageHandler : MonoBehaviour
{
    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<GameObject, float, DamageDirectionData> { }

    [SerializeField]
    private bool showDamageNumbers = true;

    public TakeDamageEvent OnDamageTaken;

    public class DamageDirectionData
    {
        public DamageDirectionData(Vector3 hitPoint, Vector3 damageDirection, float knockbackAmount)
        {
            this.hitPoint = hitPoint;
            this.damageDirection = damageDirection;
            this.knockbackAmount = knockbackAmount;
        }
        public Vector3 hitPoint;
        public Vector3 damageDirection;
        public float knockbackAmount;
    }

    private void Start()
    {
        if(OnDamageTaken == null)
        {
            OnDamageTaken = new TakeDamageEvent();
        }
    }

    public void ApplyDamage(GameObject damageSource, float damageAmount, DamageDirectionData damageDirectionData)
    {
        if(showDamageNumbers)
        {
            FloatingDamageTextHandler.CreateFloatingText(FloatingDamageTextHandler.DamageType.Normal, damageAmount, damageDirectionData.hitPoint + Random.onUnitSphere * 0.25f);
        }
        OnDamageTaken.Invoke(damageSource, damageAmount, damageDirectionData);
    }
}
