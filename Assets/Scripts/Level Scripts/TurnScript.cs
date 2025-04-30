using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Import UnityEvent
using System.Linq;

public class TurnScript : MonoBehaviour
{
    
    private bool playersTurn = true; // Track whose turn it is


    [SerializeField] private GridDetector gridDetector;

    public GameObject heldTile;
    public GameObject moveTilesRef;

    [SerializeField] private GameObject[] Players;
    [SerializeField] private GameObject[] Enemies;

    // UnityEvents that can be customized in the Inspector or assigned via code
    public UnityEvent onPlayersTurnStart;
    public UnityEvent onEnemiesTurnStart;

    // Start is called before the first frame update
    private void Start()
    {
        FindUnits();
    }

    private void FindUnits()
    {
        //print("Trying to find units");
        Players = GameObject.FindGameObjectsWithTag("Player");
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");

    }
    
    public void CheckIfTurnChange()
    {
        if (playersTurn)
        {
            if (CheckPlayersInactive(Players))
            {
                SetAllPlayersActive(Enemies, true); // Switch to enemies when all players are inactive
                playersTurn = false; // Now it's the enemies' turn
                RunEnemyAI();
            }
        }
        else
        {
            SetAllPlayersActive(Players, true); // Switch back to players when all enemies are inactive
            playersTurn = true; // Now it's the players' turn

        }
    }
    

    
    // Check if all units in the list are inactive
    private bool CheckPlayersInactive(GameObject[] units)
    {
        foreach (GameObject unit in units)
        {
            CharacterAI unitScript = unit.GetComponent<CharacterAI>();
            if (unitScript != null && unitScript.CanCharacterMove()) // If any unit is still active, return false
            {
                return false;
            }
        }
        return true; // All units are inactive
    }

    // Set all players active or inactive, then trigger the event
    private void SetAllPlayersActive(GameObject[] units, bool state)
    {
        foreach (GameObject unit in units)
        {
            CharacterAI unitScript = unit.GetComponent<CharacterAI>();
            if (unitScript != null && unitScript.CanCharacterMove() != state) // if the script isn't null and the state and characters capacity isn't the same then continue
            {
                unitScript.ToggleCharacterMove();
            }
        }
        // Trigger the event after all players have been set
        onPlayersTurnStart?.Invoke();
    }
    
    private void RunEnemyAI() {
        if (!playersTurn) {
            EngageEnemyTurn();
        }
    }
    
    private void EngageEnemyTurn()
    {
        //print("Engaging enemies turn");
        foreach (GameObject enemy in Enemies)
        {
            enemy.GetComponent<CharacterAI>().ActivateMovement();
        }
        CheckIfTurnChange();
    }
    
      
}