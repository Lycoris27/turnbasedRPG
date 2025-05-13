using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDetect : MonoBehaviour
{
    private GameObject heldCharacter;
    [SerializeField] private CharacterAI characterAI;

    private Vector2Int cursorPos;
    [SerializeField] private Vector2Int heldPosition = new Vector2Int(-1, -1);

    // Movement state
    [SerializeField, Header("Movement State")] private bool playerMoving = false; // Determines if you are in phase 1 of ProgressPlayerMovement or not
    [SerializeField] private bool onMoveTile; // finds if you are on a movement tile
    [SerializeField] private bool foundPlayer = false; // finds player or enemy
    [SerializeField] private bool foundEnemy = false; // determines if enemy is found
    [SerializeField] private bool finalCheck; // check for phase 3


    private TurnScript tScript;
    private GridEngager gDetect;

    private void Awake()
    {
        tScript = GetComponent<TurnScript>();
        gDetect = GetComponent<GridEngager>();
    }


    private void OnEnable()
    {
        InputManager.OnUIActivated += ProgressPlayerMovement;
        InputManager.OnBackspace += RegressPlayerMovement;
    }
    private void OnDisable()
    {
        InputManager.OnUIActivated -= ProgressPlayerMovement;
        InputManager.OnBackspace -= RegressPlayerMovement;
    }

    public void DetectCharacter(Vector2Int pos)
    {
        cursorPos = pos;
        List<GameObject> tileData = gDetect.ReturnTileData(cursorPos);
        GameObject characterTile = tileData[1];

        if (characterTile == null)
        {
            foundEnemy = false;
            foundPlayer = false;
            if (!playerMoving)
            {
                heldCharacter = null;
                characterAI = null;
            }
        }
        else if(characterTile != null)
        {
            heldCharacter = characterTile;
            characterAI = heldCharacter.GetComponent<CharacterAI>();
            if (characterTile.CompareTag("Player")) { foundPlayer = true; }
            else if (characterTile.CompareTag("Enemy")) { foundEnemy = true; }
        }

    }

    private void ProgressPlayerMovement(float value)
    {
        if(finalCheck)
        {
            playerMoving = false;
            finalCheck = false;
            onMoveTile = false;
            foundPlayer = true;
            heldPosition = new Vector2Int(0, 0);
            //characterAI.ToggleDisplayGrid();
            //characterAI.ToggleCharacterMove();
            tScript.CheckIfTurnChange();
            return;
        }
        else if (playerMoving && onMoveTile)
        {
            //characterAI.SetPosition(cursorPos);
            gDetect.MoveCharacter(cursorPos, heldPosition, heldCharacter);

            finalCheck = true;
            return;
        }
        else if (foundPlayer && !playerMoving && characterAI.CanCharacterMove())
        {
            heldPosition = cursorPos;
            playerMoving = true;
            onMoveTile = true;
            //characterAI.ToggleDisplayGrid();
            return;
        }
        else if (foundEnemy && !playerMoving)
        {
            //characterAI.ToggleDisplayGrid();
        }
    }
    private void RegressPlayerMovement(float value)
    {
        if (finalCheck)
        {
            finalCheck = false;
            //characterAI.SetPosition(heldPosition);

            gDetect.SetTileData(heldPosition, 1, heldCharacter);
            gDetect.SetTileData(cursorPos, 1, null);
            return;
        }
        else if (playerMoving)
        {
            playerMoving = false;
            onMoveTile = false;
            //characterAI.ToggleDisplayGrid();
            heldPosition = new Vector2Int(-1, -1);

        }
    }
}