using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        
    }

    // Update is called once per frame

    private void OnEnable()
    {
        InputManager.OnMovePerformed += Bonk;
    }
    private void OnDisable()
    {
        InputManager.OnMovePerformed -= Bonk;
    }




    private void UpdateUI()
    {
        Debug.Log("Updating UI");
    }
    private void Bonk(Vector2 value)
    {
        print("oink boink, this is " + this.name);
    }

}
