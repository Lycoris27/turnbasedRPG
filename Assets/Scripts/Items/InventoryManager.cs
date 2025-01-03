using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

// Class to represent each item in the inventory with its quantity
[System.Serializable]
public class InventoryItem
{
    public Items item;   // Reference to the Items ScriptableObject
    public int quantity; // Quantity of the item
}

// Class to represent a category of items
[System.Serializable]
public class InventoryCategory
{
    public string categoryName;                // Name of the category
    public List<InventoryItem> items = new List<InventoryItem>(); // List of items in this category
}

// Main Inventory class
public class InventoryManager : MonoBehaviour
{
    // List for categorized items with quantities
    [SerializeField] private List<InventoryCategory> categorizedItems = new List<InventoryCategory>();

    private void Awake()
    {
        // Initialize categories
        InitializeCategories();
        // Load items from folders into categories
        LoadItemsFromFolders();
    }


    private void InitializeCategories()
    {
        categorizedItems.Add(new InventoryCategory { categoryName = "Swords" });
        categorizedItems.Add(new InventoryCategory { categoryName = "Spears" });
        categorizedItems.Add(new InventoryCategory { categoryName = "Axes" });
        categorizedItems.Add(new InventoryCategory { categoryName = "Bows" });
        categorizedItems.Add(new InventoryCategory { categoryName = "Tomes" });
        categorizedItems.Add(new InventoryCategory { categoryName = "Staffs" });
        categorizedItems.Add(new InventoryCategory { categoryName = "Consumables" });
    }

    private void LoadItemsFromFolders()
    {
        // Base path for items
        string basePath = "Assets/Objects/Items/";

        foreach (var category in categorizedItems)
        {
            string folderPath = Path.Combine(basePath, category.categoryName);

            // Check if the folder exists
            if (Directory.Exists(folderPath))
            {
                // Load all ScriptableObjects in the folder
                string[] itemFiles = Directory.GetFiles(folderPath, "*.asset");

                foreach (string file in itemFiles)
                {
                    // Load the ScriptableObject
                    Items item = AssetDatabase.LoadAssetAtPath<Items>(file);

                    if (item != null)
                    {
                        // Add the item to the category with an initial quantity of 1
                        category.items.Add(new InventoryItem { item = item, quantity = 1 });
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Folder '{folderPath}' does not exist.");
            }
        }
    }

    // Method to add an item to the inventory
    public void AddItem(Items newItem, string category, int quantity)
    {
        InventoryCategory categoryObj = categorizedItems.Find(c => c.categoryName == category);

        if (categoryObj == null)
        {
            Debug.LogError($"Category '{category}' does not exist.");
            return;
        }

        InventoryItem existingItem = categoryObj.items.Find(i => i.item == newItem);

        if (existingItem != null)
        {
            existingItem.quantity += quantity; // Update quantity
        }
        else
        {
            categoryObj.items.Add(new InventoryItem { item = newItem, quantity = quantity }); // Add new item
        }
    }

    // Method to remove an item from the inventory
    public void RemoveItem(Items itemToRemove, string category, int quantity)
    {
        InventoryCategory categoryObj = categorizedItems.Find(c => c.categoryName == category);

        if (categoryObj == null)
        {
            Debug.LogError($"Category '{category}' does not exist.");
            return;
        }

        InventoryItem existingItem = categoryObj.items.Find(i => i.item == itemToRemove);

        if (existingItem != null)
        {
            existingItem.quantity -= quantity; // Decrease quantity
            if (existingItem.quantity <= 0)
            {
                categoryObj.items.Remove(existingItem); // Remove item if quantity is zero
            }
        }
        else
        {
            Debug.LogError($"Item '{itemToRemove.name}' not found in category '{category}'.");
        }
    }

    // Method to get the quantity of an item
    public int GetItemQuantity(Items item, string category)
    {
        InventoryCategory categoryObj = categorizedItems.Find(c => c.categoryName == category);

        if (categoryObj != null)
        {
            InventoryItem existingItem = categoryObj.items.Find(i => i.item == item);
            if (existingItem != null)
            {
                return existingItem.quantity;
            }
        }
        return 0; // Item not found
    }
}