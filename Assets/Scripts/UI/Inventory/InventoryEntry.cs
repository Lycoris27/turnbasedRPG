using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntry
{
    public Items item { get; private set; }
    public int count { get; private set; }
    public int uniqueID { get; private set; }

    public InventoryEntry(Items newItem)
    {
        item = newItem;
        count = 0;
        uniqueID = 0;
    }
    public void IncreaseCount()
    {
        count++;
    }
    public void DecreaseCount()
    {
        count--;
    }

    public void SetUniqueID(int newID)
    {
        uniqueID = newID;
    }

}
