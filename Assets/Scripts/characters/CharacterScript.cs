using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript: MonoBehaviour
{
    
    public int HPCur { get; private set; }
    [Range(0, 100)] public int HPMax;

    public Stats Atk;
    public Stats Mag;
    public Stats Spd;
    public Stats Skl;
    public Stats Lck;
    public Stats Def;
    public Stats Res;
    public Stats Mvmnt;

    private void Awake()
    {
        HPCur = HPMax;  
    }
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
}
