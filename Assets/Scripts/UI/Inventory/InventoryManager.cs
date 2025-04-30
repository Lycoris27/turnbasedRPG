using System.Collections.Generic;
using UnityEngine;
using System.Linq;



/// <summary>
/// 
/// it might be worthwhile to change the inventory from a dictionary<int, list> to a dictionary<int, dictionary<int,list>> or something of the sort 
/// 
/// dictionary<(int, int), list> wouldn't be horrible as (-,0) could be used as a determinant for the first position in the system
/// 
/// TRY AND WORK WITHOUT THIS FOR NOW, IF THE LISTS GET TOO BIG THEN FALL BACK ON AN 0(1) SOLUTION  
/// 
/// 
/// 
/// 
/// 
/// </summary>


public class InventoryManager : MonoBehaviour
{
    public ItemHolder itemHolder;

    private Dictionary<int, List<InventoryEntry>> inventory = new Dictionary<int, List<InventoryEntry>>();

    public HashSet<int> uniqueIDs = new HashSet<int>();

    private int newID;

    private void Start()
    {
        foreach (Items item in itemHolder.allItems)
        {
            inventory.Add(item.ItemID, new List<InventoryEntry>());
            inventory[item.ItemID].Add(new InventoryEntry(item));

        }
    }

    public void AddItem(Items item)
    {
        if (item.Durability == item.MaxDurability)
        {
            inventory[item.ItemID][0].IncreaseCount();
        }
        else if (item.Durability != item.MaxDurability) // if item durability is less than max
        {
            // add a unique item to the end of the list, and generate a UniqueID for it
            inventory[item.ItemID].Add(new InventoryEntry(item));

            int itemID = CreateUniqueID(item);

            inventory[item.ItemID].Last().SetUniqueID(itemID);
        }
    }
    






    public void RemoveItem(Items item, InventoryEntry inventoryEntry)
    {
        if (inventoryEntry.uniqueID == 0) // if item is not unique, remove from count, might have to move a check like this to be in the inventoryEntry
        {
            inventory[item.ItemID][0].DecreaseCount();
        }
        else if (inventoryEntry.uniqueID > 0) // if the item is lower than max durability, loop through list to find item with same uniqueID and remove it
        {
            for (int i = 1; i < inventory[item.ItemID].Count; i++)
            {
                if (inventoryEntry.uniqueID == inventory[item.ItemID][i].uniqueID)
                {
                    inventory[item.ItemID].RemoveAt(i);
                }
            }
        }
    }
    
    public void GiveItemToPlayer(Items item, InventoryEntry inventoryEntry, GameObject player)
    {        
        if (inventoryEntry.uniqueID == 0) { int itemID = RemoveItemFromStack(item); }

        else if (inventoryEntry.uniqueID > 0) { int itemID = inventoryEntry.uniqueID; }

        
        // Give item to the player
        

    }

    public void RemoveItemFromPlayer(InventoryEntry inventoryEntry)
    {
        for (int i = 1; i <= inventory[inventoryEntry.item.ItemID].Count(); i++)
        {

        }

        //remove the item from the players inventory
    }

    public int RemoveItemFromStack(Items item)
    {
        inventory[item.ItemID][0].DecreaseCount();

        inventory[item.ItemID].Add(new InventoryEntry(item));

        int itemID = CreateUniqueID(item);

        inventory[item.ItemID].Last().SetUniqueID(itemID);

        return itemID;
    }

    private int CreateUniqueID(Items item)
    {
        bool checkingForNewID = false;
        int uniqueIDMod = 0;

        //check to see if an ID exists

        while (checkingForNewID)
        {
            uniqueIDMod++;
            newID = int.Parse(uniqueIDMod.ToString() + item.ItemID.ToString());

            print("New ID is: " + newID);

            if (!uniqueIDs.Contains(newID))
            {
                uniqueIDs.Add(newID);
                checkingForNewID = true;
            }
        }
        return newID;
    }

    private void DeleteUniqueID(InventoryEntry inventoryEntry)
    {
        uniqueIDs.Remove(inventoryEntry.uniqueID);
    }
}