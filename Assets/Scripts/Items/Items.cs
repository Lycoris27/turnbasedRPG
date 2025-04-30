using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Intentory/Item")]

public class Items : ScriptableObject
{
    [SerializeField] private int itemID; public int ItemID => itemID; 

    //[Header("Sprite")]
    //[SerializeField] private Sprite icon = null;

    [Header("Descriptions")]
    [SerializeField] private string itemName = "New Item"; public string ItemName => itemName;
    [SerializeField] private string description = "A simple weapon"; public string Description => description;



    [Header("Boons")]
    [SerializeField] [Range(0, 100)] private int atkBoon; public int AtkBoon => atkBoon;
    [SerializeField] [Range(0, 100)] private int magBoon; public int MagBoon => magBoon;
    [SerializeField] [Range(0, 100)] private int spdBoon; public int SpdBoon => spdBoon;
    [SerializeField] [Range(0, 100)] private int sklBoon; public int SklBoon => sklBoon;
    [SerializeField] [Range(0, 100)] private int lckBoon; public int LckBoon => lckBoon;
    [SerializeField] [Range(0, 100)] private int defBoon; public int DefBoon => defBoon;
    [SerializeField] [Range(0, 100)] private int resBoon; public int ResBoon => resBoon;

    
    private enum DmgType { atk, mag, hybrid, none }

    [Header(" ")]

    [SerializeField] [Range(0, 100)] private int range; public int Range => range;

    [SerializeField] private DmgType damageType; public string DamageType => damageType.ToString();

    private enum ItmType { sword, lance, axe, bow, tome, staff, consumable }
    [SerializeField] private ItmType itemType; public string ItemType => itemType.ToString();


    [SerializeField] private bool isEquipped; public bool IsEquipped => isEquipped;
    //public bool isDefaultItem;
    
    
    [SerializeField] [Range(0, 50)] private int maxDurability; public int MaxDurability => maxDurability;

    [SerializeField] [Range(0, 50)]private int durability; public int Durability
    {
        get => durability;
        private set => durability = Mathf.Clamp(value, 0, maxDurability);
    }

    private void OnValidate()
    {
        durability = Mathf.Clamp(durability, 0, maxDurability); // Restrict durability within maxDurability
    }


    public void ChangeEquipped(bool equipped)
    {
        isEquipped = equipped;
    }

    public void ChangeDurability(int value)
    {
        durability = durability + value;
    }

}
