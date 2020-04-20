using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public enum PickupType
    {
        Health, 
        Energy
    }

    [System.Serializable]
    private class PickupData
    {
        public PickupType pickupType;
        public PickupBase pickupPrefab;
    }

    [SerializeField]
    private PickupData[] pickupData;

    Dictionary<PickupType, PickupData> pickupDataMap;

    List<PickupBase> currentPickups = new List<PickupBase>();

    static PickupManager instance;

    void Awake()
    {
        instance = this;

        pickupDataMap = new Dictionary<PickupType, PickupData>();
        foreach(var pickup in pickupData)
        {
            pickupDataMap.Add(pickup.pickupType, pickup);
        }
    }

    public static void SpawnPickup(PickupType pickupType, Vector3 position, Quaternion rotation)
    {
        if (instance == null)
        {
            Debug.LogError($"No instance of {nameof(FloatingDamageTextHandler)} in scene");
            return;

        }
        var pickup = PickupBase.Instantiate(instance.pickupDataMap[pickupType].pickupPrefab, position, rotation);
        instance.currentPickups.Add(pickup);
    }

    public static void ClearPickups()
    {
        if (instance == null)
        {
            Debug.LogError($"No instance of {nameof(FloatingDamageTextHandler)} in scene");
            return;

        }
        foreach (PickupBase pickup in instance.currentPickups)
        {
            Destroy(pickup.gameObject);
        }
        instance.currentPickups.Clear();
    }

#if UNITY_EDITOR
    public GameObject GetPickupObject(PickupType pickupType)
    {
        foreach(var pickup in pickupData)
        {
            if(pickup.pickupType == pickupType)
            {
                return pickup.pickupPrefab.gameObject;
            }
        }
        return null;
    }
#endif
}
