
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[System.Serializable]
public class CursorMove : MonoBehaviour
{
    //References

    [SerializeField, Header("References")] private GridDetector gridDetector;
    private CursorDetect cursorDetect;

    // Cursor-related fields
    [SerializeField, Header("Positions")] private Vector2Int cursorPos;
    //[SerializeField] private Vector2Int heldPosition;




    private void OnValidate() { SetPosition(); }

    private void Awake() { cursorDetect = GetComponent<CursorDetect>(); }
    private void Start() => SetPosition();
    private void OnEnable() { InputManager.OnMovePerformed += Move; }
    private void OnDisable() { InputManager.OnMovePerformed -= Move; }
    private void SetPosition()
    {
        transform.position = new Vector3(cursorPos.x + 0.5f, (int)0f, cursorPos.y + 0.5f);
    }

    private void Move(Vector2 value) {
        // Round the values to ensure they are -1, 0, or 1
        int moveX = Mathf.RoundToInt(-value.y);
        int moveY = Mathf.RoundToInt(value.x);

        // Calculate new cursor positions
        int newPosX = cursorPos.x + moveX;
        int newPosY = cursorPos.y + moveY;

        // Check bounds for new cursor positions
        if (newPosX >= 0 && newPosX < gridDetector.ReturnGridSize().x + 1 && newPosY >= 0 && newPosY < gridDetector.ReturnGridSize().y + 1) {
            cursorPos.x = newPosX;
            cursorPos.y = newPosY;
            cursorPos = new Vector2Int(cursorPos.x, cursorPos.y);
            SetPosition();
            cursorDetect.DetectCharacter(cursorPos);   
        }
    }
}
