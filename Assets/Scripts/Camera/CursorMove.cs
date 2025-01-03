// Main script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CursorMove : MonoBehaviour
{
    //References
    [SerializeField, Header("References")] private GameObject heldObject;
    [SerializeField] private GameObject heldCharacter;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject movementTileMap;
    [SerializeField] private GridDetector gridDetector;
    [SerializeField] private PlayerCharacterSheets characterScript;

    // Cursor-related fields
    [SerializeField, Header("Cursor Settings")] private GameObject cursor;
    [SerializeField] private Vector2 cursorPosition;

    // Movement state
    [SerializeField, Header("Movement State")] private bool canPressEnter;
    [SerializeField] private bool isOnMoveTile;
    [SerializeField] private bool movementEngaged;
    [SerializeField] private bool finalCheck;

    public List<List<GameObject>> movementArray = new List<List<GameObject>>();

    private void Start() => SetPosition();

    private void OnEnable() {
        InputManager.OnMovePerformed += Move;
        InputManager.OnUIActivated += ProgressPlayerMovement;
        InputManager.OnBackspace += RegressPlayerMovement; }

    private void OnDisable() {
        InputManager.OnMovePerformed -= Move;
        InputManager.OnUIActivated -= ProgressPlayerMovement;
        InputManager.OnBackspace -= RegressPlayerMovement; }

    private void SetPosition()  {
        heldObject = gridDetector.detectedObjects[(int)cursorPosition.x][(int)cursorPosition.y];
        transform.position = new Vector3(heldObject.transform.position.x, 0, heldObject.transform.position.z);  }

    private void Move(Vector2 value)
    {
        // Round the values to ensure they are -1, 0, or 1
        int moveX = Mathf.RoundToInt(-value.y);
        int moveY = Mathf.RoundToInt(value.x);

        // Calculate new cursor positions
        int newCursorX = (int)cursorPosition.x + moveX;
        int newCursorY = (int)cursorPosition.y + moveY;

        // Check bounds for new cursor positions
        if (newCursorX >= 0 && newCursorX < gridDetector.detectedObjects.Count &&
            newCursorY >= 0 && newCursorY < gridDetector.detectedObjects[newCursorX].Count)
        {
            cursorPosition.x = newCursorX;
            cursorPosition.y = newCursorY;

            heldObject = gridDetector.detectedObjects[newCursorX][newCursorY];
            transform.position = new Vector3(heldObject.transform.position.x, 0, heldObject.transform.position.z);

            DetectObjectAtPosition();
        }
    }

    private void DetectObjectAtPosition()
    {
        LayerMask layerMask = (1 << LayerMask.NameToLayer("Character")) |
                              (1 << LayerMask.NameToLayer("MovementTile")) |
                              (1 << LayerMask.NameToLayer("Enemy"));

        Vector3 rayStartPos = transform.position + Vector3.up * 100;
        RaycastHit[] hits = Physics.RaycastAll(rayStartPos, Vector3.down, Mathf.Infinity, layerMask);

        if (!movementEngaged) { heldCharacter = null; characterScript = null; isOnMoveTile = false; canPressEnter = false; }
        else { canPressEnter = false; }

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                int hitLayer = hit.collider.gameObject.layer;
                if (hitLayer == LayerMask.NameToLayer("Enemy")) { canPressEnter = false; isOnMoveTile = false; break; }
                else if (hitLayer == LayerMask.NameToLayer("Character") && !movementEngaged)
                {
                    heldCharacter = hit.collider.gameObject;
                    if (heldCharacter != null) { characterScript = heldCharacter.GetComponent<PlayerCharacterSheets>(); if (characterScript != null) { canPressEnter = true; } }
                }
                else if (hitLayer == LayerMask.NameToLayer("MovementTile")) { isOnMoveTile = true; }
            }
        }
        if (heldCharacter == null && !isOnMoveTile) { canPressEnter = false; isOnMoveTile = false; }
    }

    private void ProgressPlayerMovement(float value)
    {
        print(canPressEnter);
        print(characterScript.IsOnPlayer());
        if (finalCheck)
        {
            characterScript.SetIfMove();
            movementEngaged = false;
            isOnMoveTile = false;
            finalCheck = false;
            foreach (Transform child in movementTileMap.transform) { GameObject.Destroy(child.gameObject); }
        }
        else if (canPressEnter && !movementEngaged && characterScript.GetIfMove())
        {
            Vector2 heldTransform = new Vector2(heldCharacter.transform.position.x, heldCharacter.transform.position.z);
            int movement = characterScript.GetMovement();
            List<List<GameObject>> movementGrid = new List<List<GameObject>>();

            for (int i = 0; i < movement + 1; i++) { movementGrid.Add(new List<GameObject>()); }

            Vector3[] directions = { new Vector3(-2.5f, 0, 0), new Vector3(2.5f, 0, 0), new Vector3(0, 0, 2.5f), new Vector3(0, 0, -2.5f) };
            GameObject firstTile = GetOrCreateTile(new Vector3(heldTransform.x, 0.6f, heldTransform.y));
            movementGrid[0].Add(firstTile);
            characterScript.MoveToPos(cursorPosition);
            StartCoroutine(ProcessMovementGrid(movement, movementGrid, directions));
            movementEngaged = true;
            isOnMoveTile = true;
        }
        
        else if (movementEngaged && (!canPressEnter || characterScript.IsOnPlayer()) && isOnMoveTile)
        {
            characterScript.MoveToPos(cursorPosition);
            finalCheck = true;
        }

    }

    private void RegressPlayerMovement(float value)
    {
        if (finalCheck)
        {
            characterScript.BackToPos();
            finalCheck = false;
        }
        else if (movementEngaged)
        {
            foreach (Transform child in movementTileMap.transform) { GameObject.Destroy(child.gameObject); }
            movementEngaged = false;
        }
    }

    private IEnumerator ProcessMovementGrid(int movement, List<List<GameObject>> movementGrid, Vector3[] directions)
    {
        int targetLayer = LayerMask.NameToLayer("MovementTile");
        HashSet<Vector3> occupiedPositions = new HashSet<Vector3> { movementGrid[0][0].transform.position };

        for (int i = 0; i <= movement; i++)
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
                        GameObject newTile = GetOrCreateTile(newPosition);
                        movementGrid[targetIndex].Add(newTile);
                        occupiedPositions.Add(newPosition);
                    }
                }
            }
            yield return null;
        }
    }

    private bool IsPositionBlocked(Vector3 position, int targetLayer)
    {
        Vector3 boxSize = new Vector3(1f, 0.5f, 1f);
        Collider[] colliders = Physics.OverlapBox(position, boxSize * 0.5f, Quaternion.identity, 1 << targetLayer);
        foreach (var collider in colliders) { if (collider.gameObject.layer == targetLayer) { return true; } }
        return false;
    }

    private GameObject GetOrCreateTile(Vector3 position)
    {
        GameObject tile = Instantiate(prefab);
        tile.transform.SetParent(movementTileMap.transform);
        tile.transform.position = position;
        return tile;
    }
}