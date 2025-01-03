using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Import UnityEvent

public class TurnScript : MonoBehaviour
{
    private string playerlayer = "Character";
    private string enemylayer = "Enemy";
    private bool playersTurn = true; // Track whose turn it is

    [SerializeField] private List<GameObject> Players = new List<GameObject>();
    [SerializeField] private List<GameObject> Enemies = new List<GameObject>();
    [SerializeField] private GridDetector gridDetector;

    // UnityEvents that can be customized in the Inspector or assigned via code
    public UnityEvent onPlayersTurnStart;
    public UnityEvent onEnemiesTurnStart;

    [SerializeField] private PlayerCharacterSheets enemyCharacter;
    [SerializeField] private PlayerCharacterSheets playerCharacter;

    // Start is called before the first frame update
    void Start()
    {
        FindUnits();
    }

    private void Update()
    {
        if (playersTurn)
        {
            if (AllPlayersInactive(Players))
            {
                SetAllPlayersActive(Enemies, true); // Switch to enemies when all players are inactive
                playersTurn = false; // Now it's the enemies' turn
                RunEnemyAI();
            }
        }
        else
        {
            if (AllPlayersInactive(Enemies))
            {
                SetAllPlayersActive(Players, true); // Switch back to players when all enemies are inactive
                playersTurn = true; // Now it's the players' turn
            }
        }
    }

    private void FindUnits()
    {
        // Convert layer names to layer integers
        int playerLayerInt = LayerMask.NameToLayer(playerlayer);
        int enemyLayerInt = LayerMask.NameToLayer(enemylayer);

        if (playerLayerInt == -1 || enemyLayerInt == -1)
        {
            Debug.LogError("One or both layers do not exist. Please check the layer names.");
            return;
        }

        // Get all game objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Loop through each object and check its layer, appending to respective lists
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == playerLayerInt)
            {
                Players.Add(obj);
            }
            else if (obj.layer == enemyLayerInt)
            {
                Enemies.Add(obj);
            }
        }

        // Optional: Debug the number of players and enemies found
        Debug.Log("Found " + Players.Count + " players and " + Enemies.Count + " enemies.");
    }

    // Check if all units in the list are inactive
    private bool AllPlayersInactive(List<GameObject> units)
    {
        foreach (GameObject unit in units)
        {
            PlayerCharacterSheets unitScript = unit.GetComponent<PlayerCharacterSheets>();
            if (unitScript != null && unitScript.GetIfMove()) // If any unit is still active, return false
            {
                return false;
            }
        }
        return true; // All units are inactive
    }


    // Set all players active or inactive, then trigger the event
    private void SetAllPlayersActive(List<GameObject> units, bool state)
    {
        foreach (GameObject unit in units)
        {
            PlayerCharacterSheets unitScript = unit.GetComponent<PlayerCharacterSheets>();
            if (unitScript != null)
            {
                unitScript.SetIfMove();
            }
        }

        // Trigger the event after all players have been set
        onPlayersTurnStart?.Invoke();
        Debug.Log("All players are active.");
    }

    private void RunEnemyAI()
    {
        if (!playersTurn)
        {
            foreach (GameObject enemy in Enemies)
            {
                // if they are in attack mode and not in guard or search mode
                PlayerCharacterSheets enemyCharacter = enemy.GetComponent<PlayerCharacterSheets>();
                Vector2 enemyPos = enemyCharacter.ReturnPosition();
                GameObject closestPlayer = null;
                Vector2 closestDist = Vector2.positiveInfinity; // Initial large value for comparison

                foreach (GameObject player in Players)
                {
                    PlayerCharacterSheets playerCharacter = player.GetComponent<PlayerCharacterSheets>();
                    Vector2 playerPos = playerCharacter.ReturnPosition();
                    Vector2 dist = playerPos - enemyPos;

                    // If the current distance is less than the closest, update the target
                    if (dist.magnitude < closestDist.magnitude)
                    {
                        closestDist = dist;
                        closestPlayer = player;
                    }
                }

                //BEGIN FINDING LOCATIONS TO MOVE TO

                ///
                /// Question: should I hold positions or objects
                /// I use positions to compare players and enemies, it might be good to hold positions to determine h
                ///


                // Now closestPlayer holds the closest player to the current enemy
                if (closestPlayer != null)
                {
                    List<List<GameObject>> movementGrid = new List<List<GameObject>>();
                    for (int i = 0; i < enemyCharacter.GetMovement() + 1; i++) { movementGrid.Add(new List<GameObject>()); }
                    movementGrid[0].Add(enemy);

                    int targetLayer = LayerMask.NameToLayer("MovementTile");
                    Vector3[] directions = { new Vector3(-2.5f, 0, 0), new Vector3(2.5f, 0, 0), new Vector3(0, 0, 2.5f), new Vector3(0, 0, -2.5f) };
                    HashSet<Vector3> occupiedPositions = new HashSet<Vector3> { movementGrid[0][0].transform.position };
                    //gridDetector.detectedObjects[0][0]

                    for (int i = 0; i <= enemyCharacter.GetMovement(); i++)
                    {

                        foreach (GameObject obj in movementGrid[i])
                        {
                            Vector3 basePosition = obj.transform.position;

                            foreach (Vector3 direction in directions)
                            {
                                Vector3 newPosition = basePosition + direction;

                                // Skip already occupied positions or blocked positions
                                if (occupiedPositions.Contains(newPosition) || IsPositionBlocked(newPosition, targetLayer))
                                    continue;

                                float newPosx = (newPosition.x / 2.5f) - 0.5f;
                                float newPosy = (newPosition.z / 2.5f) - 0.5f;

                                // Check bounds for newPosx and newPosy
                                if (newPosx < 0 || newPosy < 0 || (int)newPosx >= gridDetector.detectedObjects.Count || (int)newPosy >= gridDetector.detectedObjects[(int)newPosx].Count)
                                    continue;

                                GameObject tileObject = gridDetector.detectedObjects[(int)newPosx][(int)newPosy];
                                if (tileObject == null) continue;

                                MovementTileScript heldScript = tileObject.GetComponent<MovementTileScript>();
                                if (heldScript == null) continue;

                                int targetIndex = i + heldScript.moveNumber;

                                // Ensure targetIndex is within bounds
                                if (heldScript.moveNumber >= 0 && targetIndex >= 0 && targetIndex < movementGrid.Count)
                                {
                                   // GameObject newTile = GetOrCreateTile(newPosition);
                                    movementGrid[targetIndex].Add(newTile);
                                    occupiedPositions.Add(newPosition);
                                }
                            }
                        }
                    }
                }

                //BEGIN MOVING CHARACTER
            }
        }
    }
    private bool IsPositionBlocked(Vector3 position, int targetLayer)
    {
        Vector3 boxSize = new Vector3(1f, 0.5f, 1f);
        Collider[] colliders = Physics.OverlapBox(position, boxSize * 0.5f, Quaternion.identity, 1 << targetLayer);
        foreach (var collider in colliders) { if (collider.gameObject.layer == targetLayer) { return true; } }
        return false;
    }
}