using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager: MonoBehaviour
{
    [System.Serializable]
    public class CharacterVars
    {
        public string name;
        public bool engagedState;
        public GameObject prefab;
        
        [Header("UI elements")]
        [SerializeField] private GameObject characterSelectUI;
    }


    [SerializeField] private Dictionary<string, CharacterVars> characterVars = new();
    [SerializeField] private List<CharacterVars> inspectorPrefabs = new();

    private void Awake()
    {
        //LoadAllCharacters();
    }
    /*
    private void LoadAllCharacters()
    {
        int i = 0;
        foreach (GameObject characterPrefab in inspectorPrefabs)
        {
            i++;

            // engages the first 3 characters immediately, as there would be no scene which specifically engages them
            // this will need to be changed later such that this script holds names in playerprefs and can be modified at start of game.
            if (i <= 2) characterVars.Add(characterPrefab.name, new CharacterVars { prefab = characterPrefab, engagedState = true });
            else if (i > 2) characterVars.Add(characterPrefab.name, new CharacterVars { prefab = characterPrefab, engagedState = false });

            print($"new character {characterPrefab.name} added and active state set to {characterVars[characterPrefab.name].engagedState}");
        }
    }
    */
    public void DisplayEngagedCharacters()
    {

    }
    
    public void ChangeCharacterEngaged(string playerName, bool activeState)
    {
        characterVars[playerName].engagedState = activeState;
    }
    public bool CheckCharacterEngaged(string playerName)
    {
        return characterVars[playerName].engagedState;
    }
    public void SaveCharacterEngaged()
    {
        foreach(var character in characterVars)
        {
            PlayerPrefs.SetInt(character.Key, characterVars[character.Key].engagedState? 1:0);
        }
        
    }
    public void LoadCharacterEngaged()
    {
        foreach(var character in characterVars)
        {
            bool isEngaged = PlayerPrefs.GetInt(character.Key, 0) == 1;
            characterVars[character.Key].engagedState = isEngaged;
        }
    }
}
