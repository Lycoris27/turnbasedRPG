using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField]
    [Range(0, 100)] private int baseValue; //[Range(0, 100)]
    [SerializeField]
    [Range(0, 100)] private int posModVal; //[Range(0, 100)]
    [SerializeField]
    [Range(0, 100)] private int negModVal; //[Range(0, 100)]


    //private List<int> modifiers = new List<int>();
    public int GetValue() 
    {
        return baseValue;
    }
    public int GetPosMod()
    {
        return posModVal;
    }
    public int GetNegMod()
    {
        return negModVal;
    }
    public int GetModifier()
    {
        return posModVal - negModVal;
    }
}
