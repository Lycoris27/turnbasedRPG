using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField]
    [Range(0, 100)] private int baseValue; //[Range(0, 100)]

    private List<int> modifiers = new List<int>();
    public int GetValue() 
    {
        return baseValue;
    }

    /*
    public void AddModifier(int modifier){
        if (modifier != 0)
            modifiers.Add(modifier);}
    public void RemoveModifier(int modifier){
        if (modifier != 0)
            modifiers.Remove(modifier);}
    */
}
