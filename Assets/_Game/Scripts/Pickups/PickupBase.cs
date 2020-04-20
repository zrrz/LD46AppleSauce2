using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBase : MonoBehaviour
{
    public virtual void PickupItem(PlayerInventory playerInventory)
    {
        //TODO Particle? Sound?
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            PickupItem(playerInventory);
        }
    }
}
