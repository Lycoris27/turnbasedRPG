using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction menuAction;
    private InputAction UIAction;
    private InputAction backspace;

    public static event System.Action<Vector2> OnMovePerformed;
    public static event System.Action<float> OnMenuActivated;
    public static event System.Action<float> OnUIActivated;
    public static event System.Action<float> OnBackspace;

    public GameObject cursor;

    public float holdTime = 0.5f; // Time to start continuous input
    public float continuousInputInterval = 0.1f; // Time interval for continuous input

    private bool isButtonHeld = false;
    private float holdTimer = 0f;
    private Coroutine moveCoroutine; // Reference to the running coroutine

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        menuAction = playerInput.actions["OpenMenu"];
        UIAction = playerInput.actions["OpenUI"];
        backspace = playerInput.actions["Backspace"];
    }

    void OnEnable()
    {
        moveAction.started += OnMoveStarted;
        moveAction.canceled += OnMoveCanceled;
        menuAction.performed += HandleMenu;
        UIAction.performed += HandleUI;
        backspace.performed += Backspace;
    }

    void OnDisable()
    {
        moveAction.started -= OnMoveStarted;
        moveAction.canceled -= OnMoveCanceled;
        menuAction.performed -= HandleMenu;
        UIAction.performed -= HandleUI;
        backspace.performed -= Backspace;
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        OnMovePerformed?.Invoke(value);
        //Debug.Log("Move value: " + value);

        isButtonHeld = true;
        holdTimer = 0f; // Reset the hold timer when the button is pressed

        // Stop the previous coroutine if it's running
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        // Start a new coroutine
        moveCoroutine = StartCoroutine(HandleMoveHold(context));
    }
    private IEnumerator HandleMoveHold(InputAction.CallbackContext context)
    {
        while (isButtonHeld)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTime)
            {
                Vector2 value = context.ReadValue<Vector2>();
                OnMovePerformed?.Invoke(value);
                Debug.Log("Continuous move value: " + value);
                yield return new WaitForSeconds(continuousInputInterval);
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        isButtonHeld = false;
        holdTimer = 0f; // Reset the hold timer when the button is released

        // Stop the coroutine when the button is released
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null; // Reset the coroutine reference
        }
    }

    private void HandleMenu(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        OnMenuActivated?.Invoke(value);
        Debug.Log("Menu value: " + value);
    }

    private void HandleUI(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        OnUIActivated?.Invoke(value);
        //Debug.Log("UI value: " + value);
    }
    private void Backspace(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        OnBackspace?.Invoke(value);
        //Debug.Log("UI value: " + value);
    }
}