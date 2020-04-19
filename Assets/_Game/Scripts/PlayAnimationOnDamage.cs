using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnDamage : MonoBehaviour
{
    [SerializeField]
    private DamageHandler damageHandler;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string animationTriggerName = "Hit";

    void Start()
    {
        if(damageHandler != null)
        {
            damageHandler.OnDamageTaken.AddListener(PlayAnimation);
        }
        else
        {
            Debug.LogError("No DamageHandler on this GameObject!", this);
        }
    }

    void PlayAnimation(GameObject damageSource, float damageAmount, DamageHandler.DamageDirectionData damageDirectionData)
    {
        animator.SetTrigger(animationTriggerName);
    }
}
