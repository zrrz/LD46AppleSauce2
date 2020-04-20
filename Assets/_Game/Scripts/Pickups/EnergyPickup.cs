using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : PickupBase
{
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 offset = new Vector3(0f, Mathf.Sin(Time.time * 2f) / 4f, 0f);
        transform.position = startPosition + offset;
    }

    public override void PickupItem(PlayerInventory playerInventory)
    {
        playerInventory.AddItem(PlayerInventory.ItemType.Energy);
        //TODO Particle? Sound?
        base.PickupItem(playerInventory);
    }

}
