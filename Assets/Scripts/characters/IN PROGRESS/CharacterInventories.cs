using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventories : MonoBehaviour
{
    [SerializeField] private List<Items> personalInventory;

    public void AddItem(Items newItem)
    {
        personalInventory.Add(newItem);
    }
    public void RemoveItem(Items newItem)
    {
        personalInventory.Remove(newItem);
    }
    public void EquipItem(Items itemToEquip)
    {
        if (personalInventory.Contains(itemToEquip))
        {
            personalInventory.Remove(itemToEquip);
            foreach (Items item in personalInventory)
            {
                itemToEquip.ChangeEquipped(false);
            }
            personalInventory.Insert(0, itemToEquip);

            itemToEquip.ChangeEquipped(true);
        }
    }
    public List<Items> GetItems()
    {
        return personalInventory;
    }
    public Items GetEquippedItem()
    {
        return personalInventory[0];
    }
}
