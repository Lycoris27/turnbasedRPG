using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public static ItemHolder Instance { get; private set; }

    public List<Items> allItems = new List<Items>();

    private void Awake()
    {
        LoadAllItems();
    }

    private void LoadAllItems()
    {
        allItems.AddRange(Resources.LoadAll<Items>("Items"));
    }
}