using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyCharacterSheet : CharacterScript
{
    [Tooltip("x = starting vertical position / depth \n" +
        "y = starting horizontal position / width \n\n" +
        "note: should only take in full numbers")]
    
    [SerializeField] private Vector2 position;
    private GameObject heldObject;
    public GridDetector gridDetector;
    [SerializeField] private bool canPlayerMove = true;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            SetPosition();
        }
        ///InputManager.OnUIActivated += Movement;
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            SetPosition();
        }
        ///InputManager.OnUIActivated -= Movement;
    }

    private void SetPosition()
    {
        if (gridDetector != null && gridDetector.detectedObjects != null &&
            (int)position.x < gridDetector.detectedObjects.Count &&
            (int)position.y < gridDetector.detectedObjects[(int)position.x].Count)
        {
            heldObject = gridDetector.detectedObjects[(int)position.x][(int)position.y];
            
            if (heldObject != null) {
                transform.position = new Vector3(heldObject.transform.position.x, 0, heldObject.transform.position.z);
            }
        }
    }
    public void MoveToPos(Vector2 value)
    {
        heldObject = gridDetector.detectedObjects[(int)value.x][(int)value.y];
        transform.position = new Vector3(heldObject.transform.position.x, heldObject.transform.position.y, heldObject.transform.position.z);

    }

    private void Update()
    {
        // Empty if no update logic is needed, consider removing the Update method.
    }

    private void DealDamage()
    {
        // Implement damage logic here.
    }
    public Vector2 CurrentPosition()
    {
        return position;
    }
    public int GetMovement()
    {
        return Mvmnt.GetValue();
    }

    public bool GetIfMove()
    {
        return canPlayerMove;
    }

    public void SetIfMove()
    {
        if (canPlayerMove)
        {
            canPlayerMove = false;
        }
        else
        {
            canPlayerMove = true;
        }
    }

}