using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CharacterMovement : MonoBehaviour
{
    [Header("Held Scripts")]
    [SerializeField] private GridEngager gridDetector;

    [Header("Positions")]
    [SerializeField] private Vector2Int position;


    private bool canCharacterMove = true;

    private CharacterMovement eMovement;
    private MovementDisplay eDisplay;
    private CharacterSheet eSheet;

    private void Awake()
    {
        eMovement = GetComponent<CharacterMovement>();
        eDisplay = GetComponent<MovementDisplay>();
        eSheet = GetComponent<CharacterSheet>();
    }


    public Vector2Int ReturnPosition() { return position; }

    private void OnEnable() {
        if (!Application.isPlaying) {
            SetPosition(position);
        }
    }

    private void OnValidate() {
        if (!Application.isPlaying) {
            SetPosition(position);
        }
    }

    public void SetPosition(Vector2Int pos)
    {
        position = pos;
        transform.position = new Vector3(pos.x + 0.5f, 0, pos.y + 0.5f);
    }
    
    public bool GetIfMove()
    {
        return canCharacterMove;
    }
    public void ToggleMove()
    {
        if (canCharacterMove)
        {
            canCharacterMove = false;
        }
        else if (!canCharacterMove)
        {
            canCharacterMove = true;
        }
    }
    
    public void AutoMoveCharacter(Dictionary<Vector2Int, int> moveLocations)
    {
        // Retrieve movement range from the player's character
        int movement = eSheet.GetMovement();
        int range = eSheet.GetRange();

        var sortedByValue = moveLocations.OrderBy(pair => pair.Value).ToList();

        eDisplay.DisplayPredictionTiles(moveLocations);

        Vector2Int? heldPos = null;

        print(sortedByValue.Count);

        foreach (var entry in sortedByValue)
        {
            //print($"Entry Value: {entry.Value}");
            if(entry.Value > movement) { break; }

            heldPos = entry.Key;
            //print($"new heldPos is {heldPos} or meant to be {entry.Key}");

        }
        SetPosition((Vector2Int)heldPos);
        //print($"Moveing to position {heldPos}");
    }
}