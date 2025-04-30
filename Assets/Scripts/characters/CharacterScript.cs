using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript: MonoBehaviour
{
    [SerializeField] private new string name; public string Name => name;

    [Header("Character Prefabs")]
    [SerializeField] private int HPCur; public int  HPCurrent => HPCur;
    [Range(0, 100)] public int HPMax;

    public Stats Atk;
    public Stats Mag;
    public Stats Spd;
    public Stats Skl;
    public Stats Lck;
    public Stats Def;
    public Stats Res;
    public Stats Range;
    public Stats Mvmnt;

    private void Awake()
    {
        HPCur = HPMax;  
    }
    /*
    public void TakeDamage (int damage)
    {
        damage -= Def.GetValue();
        if (damage < 1)
        {
            damage = 1;
        }
        HPCur -= damage;
        Debug.Log("take " + damage + " damage.");
    }
    */
}
