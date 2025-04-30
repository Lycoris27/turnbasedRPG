using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemDisplayer : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    private void OnBecameVisible()
    {
        
    }

    /*
    public void MakeAllVisible()
    {
        Dictionary<string, List<InventoryEntry>> inventory = inventoryManager.GetAllItems();

        // Foreach string, forech item in list, make visible in inventory


    }
    public void MakeCategoryVisible(string input)
    {
        List<InventoryEntry> inventory = inventoryManager.GetItemsByCategory(input);
    }

*/
}
