using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// will have to sort out attacking and damage, the player character sheet still has to come through, might be good to have it done in turnscript or in the levelmanager as it is an interaction between 2 objects
/// </summary>

public class CharacterAI : MonoBehaviour
{
    private CharacterSheet eSheet;
    private bool canMove;

    private void Awake()
    {
        eSheet = GetComponent<CharacterSheet>();

    }
    public bool CanCharacterMove() { return canMove; }
    public void ToggleCharacterMove() {
        if (canMove)
        {
            canMove = false;
        }
        if (!canMove)
        {
            canMove = true;
        }
             
    }
    public void ReturnMovement() { eSheet.GetMovement(); }

    public Dictionary<string, int> ReturnStats() { return eSheet.GetStats(); }
}
