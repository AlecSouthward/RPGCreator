using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<Item> inventory;

    private void Awake()
    {
        // creates a single instance of InventoryManager
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Destroying " + gameObject.name + "'s InventoryManager script");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // adds an item to the inventory
    public void AddItem(Item newItem)
    {
        inventory.Add(newItem);
    }

    [System.Serializable]
    public struct Item
    {
        public string name;
    }
}
