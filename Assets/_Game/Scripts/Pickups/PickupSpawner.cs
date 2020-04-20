using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    private PickupManager.PickupType pickupType;

    void Start()
    {
        PickupManager.SpawnPickup(pickupType, transform.position, transform.rotation);
        Destroy(gameObject); //Fix this if I want levels to reset without just reloading
    }

#if UNITY_EDITOR

    private PickupManager pickupManager;

    private void OnDrawGizmos()
    {
        if(pickupManager == null)
        {
            pickupManager = FindObjectOfType<PickupManager>();
        }
        else
        {
            GameObject pickupObj = pickupManager.GetPickupObject(pickupType)?.GetComponentInChildren<MeshFilter>().gameObject;
            Mesh mesh = pickupObj?.GetComponentInChildren<MeshFilter>().sharedMesh;
            Gizmos.color = Color.green;
            if(pickupObj && mesh)
            {
                Gizmos.DrawMesh(mesh, 0, transform.position, transform.rotation * pickupObj.transform.localRotation, pickupObj.transform.localScale);
            }
        }
    }
#endif
}
