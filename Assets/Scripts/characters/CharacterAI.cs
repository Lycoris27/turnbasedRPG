using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// will have to sort out attacking and damage, the player character sheet still has to come through, might be good to have it done in turnscript or in the levelmanager as it is an interaction between 2 objects
/// </summary>

public class CharacterAI : MonoBehaviour
{
    private CharacterSheet eSheet;
    private CharacterPathfinding epathfind;
    private MovementDisplay eDisplay;
    private CharacterMovement eMovement;

    private bool tilesEngaged = false;

    private void Awake()
    {
        eSheet = GetComponent<CharacterSheet>();
        epathfind = GetComponent<CharacterPathfinding>();
        eDisplay = GetComponent<MovementDisplay>();
        eMovement = GetComponent<CharacterMovement>();
    }
    public bool CanCharacterMove() { return eMovement.GetIfMove(); }
    public void ToggleCharacterMove() { eMovement.ToggleMove(); }
    public void ReturnMovement() { eSheet.GetMovement(); }

    public Dictionary<string, int> ReturnStats() { return eSheet.GetStats(); }

    public void SetPosition(Vector2Int pos) { eMovement.SetPosition(pos); }
    public Vector2Int ReturnPosition() { return eMovement.ReturnPosition(); }

    public void ToggleDisplayGrid()
    {
        
        if (!tilesEngaged)
        {
            
            epathfind.BeginPredictPathfinding();
            tilesEngaged = true;
        }
        else if(tilesEngaged)
        {
            eDisplay.RemoveDisplayedTiles();
            tilesEngaged = false;
        }
    }




    public void ActivateMovement() // used to move character with autopathing, usually for enemies
    {
        // Finds the path the enemy needs to follow, creates a dictionary that is the most direct path to the player
        Dictionary<Vector2Int, int> fScore = epathfind.BeginPathfinding();

        // if fscore is sorted, moves the enemy along based on fscore
        if (fScore != null)
        {
            eMovement.AutoMoveCharacter(fScore);
        }
    }
}
