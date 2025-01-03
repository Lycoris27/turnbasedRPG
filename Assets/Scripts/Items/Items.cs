using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Intentory/Item")]
public class Items: ScriptableObject
{
    [SerializeField]
    public Sprite icon = null;
    new public string name = "New Item";
    [Tooltip("0 = physical, 1 = magical, 2 = both")] [Range(0, 3)] public int damageType = 0;
    public int range;
    public int damageAmount = 0;
    public string description = "A simple weapon";

    public bool isDefaultItem = false;
}
