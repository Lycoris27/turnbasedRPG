using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        // This is just an addition becasue of the brackeys "ITEMS - Making an RPG in unity (E04)" video
       if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
    }
    #endregion
    public List<Items> itemInventory = new List<Items>();

    public void Add(Items item)
    {
        if (!item.isDefaultItem)
        {
            itemInventory.Add(item);
        }
    }
    public void Remove(Items item)
    {
        itemInventory.Remove(item);
    }
}
