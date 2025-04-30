using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet : CharacterScript
{ 

    public int GetMovement()
    {
        return Mvmnt.GetValue();
    }
    
    public int GetRange()
    {
        return Range.GetValue();
    }
    
    public Dictionary<string, int> GetStats()
    {
        Dictionary<string, int> statDictionary = new Dictionary<string, int>();

        statDictionary.Add("HP", HPCurrent);
        statDictionary.Add("atk", Atk.GetValue() + Atk.GetModifier());
        statDictionary.Add("mag", Mag.GetValue() + Mag.GetModifier());
        statDictionary.Add("spd", Spd.GetValue() + Spd.GetModifier());
        statDictionary.Add("skl", Skl.GetValue() + Skl.GetModifier());
        statDictionary.Add("lck", Lck.GetValue() + Lck.GetModifier());
        statDictionary.Add("def", Def.GetValue() + Def.GetModifier());
        statDictionary.Add("res", Res.GetValue() + Res.GetModifier());
        statDictionary.Add("range", Range.GetValue() + Range.GetModifier());

        return statDictionary;
    }

}