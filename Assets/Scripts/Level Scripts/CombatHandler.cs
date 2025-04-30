using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    
    [SerializeField] private GridDetector gridDetector;


    public void AttackChecker(Vector2Int posAttacker, Vector2Int posDefender)
    {
        GameObject attacker = gridDetector.ReturnTileData(posAttacker)[1];
        GameObject defender = gridDetector.ReturnTileData(posDefender)[1];

        Dictionary<string, int> attackStats = attacker.GetComponent<CharacterSheet>().GetStats();
        Dictionary<string, int> defendStats = defender.GetComponent<CharacterSheet>().GetStats();

        int dist = Mathf.Abs( (posAttacker.x + posAttacker.y) - (posDefender.y + posDefender.y));

        ///Setting up player combat
        ///
        /// check attackers equipped weapon damage stat
        /// use the same string to grab atk or mag stat
        /// if its atk or mag, compare against def or res
        /// defender takes damage based on difference between defensive stat and offensive stat
        /// checks to see if enemy is within range
        /// repeats process
        /// checks to see if attacker has 5 more spd than enemy
        /// repeats process
        ///


        if (dist !> defendStats["range"])
        {

        }

        

        //PlayerCharacterSheets attackerSheet = attacker.GetComponent<PlayerCharacterSheets>();
        //PlayerCharacterSheets defenderSheet = defender.GetComponent<PlayerCharacterSheets>();






        //compare the attackers attack stat to the defenders defend stat

        ///Setting up playerCombat
        ///
        /// Set up can attack situations (if attacker has higher range than defender and is attacking from outside of that range, if attacker has weapon that can prevent attacking from defender, if defender doesn't have a weapon)
        /// 
        /// what needs to come through is:
        /// who can attack
        /// how many attacks
        /// what type of damage 
        ///


    }
    public void Attacking()
    {

    }
    
}
