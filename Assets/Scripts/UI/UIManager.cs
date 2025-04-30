using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Stack<GameObject> menuStack = new Stack<GameObject>();
    public GameObject defaultUI;

    [Header("UI References")]
    [SerializeField] private List<GameObject> UIList;
    public Dictionary<string, GameObject> UIHandler { get; private set; } = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (GameObject UI in UIList)
        {
            UIHandler.Add(UI.gameObject.name, UI);
            UIHandler[UI.gameObject.name].SetActive(false);
        }
        defaultUI.SetActive(true);
    }

    public void OpenUI(GameObject newUI) // Accessed by buttons to load new UI, must add to stack.
    {
        GameObject currentMenu = GetActiveUI();

        if (currentMenu != null) { currentMenu.SetActive(false); }
        else if(currentMenu == null) { defaultUI.SetActive(false);  }

        menuStack.Push(newUI);
        newUI.SetActive(true);

    }
    public void CloseUI()
    {
        GameObject currentMenu = menuStack.Pop();

        currentMenu.SetActive(false);

        if (menuStack.Count == 0)
        {
            defaultUI.SetActive(true);
        }
        else if (menuStack.Count > 0)
        {
            GameObject nextMenu = menuStack.Peek();
            nextMenu.SetActive(true);
        }

    }
    public GameObject GetActiveUI()
    {
        if (menuStack.Count != 0)
        {
            return menuStack.Peek();
        }
        else return null;
    }
    public void SetDefaultUI(GameObject newUI)
    {
        defaultUI = newUI;
    }

    private void Checking()
    {
        print("objects in menustack: ");

        foreach (var obj in menuStack)
        {
            print(obj);
        }
    }
}
