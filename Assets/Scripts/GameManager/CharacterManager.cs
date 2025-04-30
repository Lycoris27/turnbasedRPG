using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager: MonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> characterPrefabs = new Dictionary<string, GameObject>();

    [SerializeField] private Dictionary<string, bool> isCharacterActive = new Dictionary<string, bool>();

    [SerializeField] private List<GameObject> allCharacterPrefabs = new List<GameObject>();


    private void Awake()
    {
        //LoadAllCharacters();
    }
    /*
    private void LoadAllCharacters()
    {
        foreach (GameObject characterPrefab in allCharacterPrefabs)
        {
            characterPrefabs.Add(characterPrefab.name, characterPrefab);

            isCharacterActive.Add(characterPrefab.name, false);
        }
    }
    */
    public void ChangeCharacterActive(string playerName, bool activeState)
    {
        isCharacterActive[playerName] = activeState;
    }

    public List<GameObject> GenerateCharacters(GameObject characters)
    {
        List<GameObject> characterList = new List<GameObject>();

        foreach (var entry in isCharacterActive)
        {
            if(entry.Value)
            {
                if (characterPrefabs.TryGetValue(entry.Key, out GameObject prefab))
                {
                    characterList.Add(prefab);
                }
                else
                {
                    Debug.LogWarning($" no prefab found for character: {entry.Key} ");
                }
            }
        }
        return characterList;
    }
}
