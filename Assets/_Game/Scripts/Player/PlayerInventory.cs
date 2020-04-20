using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public enum ItemType
    {
        Energy, //TODO upgrades
    }

    Dictionary<ItemType, int> items;

    void Awake()
    {
        items = new Dictionary<ItemType, int>();
        foreach(ItemType type in System.Enum.GetValues(typeof(ItemType)))
        {
            items.Add(type, 0);
        }
    }

    public void AddItem(ItemType itemType)
    {
        items[itemType]++; 
    }

    public bool HasItem(ItemType itemType)
    {
        return items[itemType] > 0;
    }

    public void RemoveItem(ItemType itemType)
    {
        if(HasItem(itemType))
        {
            items[itemType]--;
        }
    }

}
