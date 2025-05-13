using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioInput
    {
        public string name;
        public AudioSource audioSource;
    }

    public static AudioManager Instance { get; private set; }
    [SerializeField] private List<AudioInput> SFXInput;
    [SerializeField] private List<AudioInput> musicInput;
    [SerializeField] private List<AudioInput> UIInput;
    [SerializeField] private List<AudioInput> dialogueInput;
    [SerializeField] private List<AudioInput> environmentInput;

    private Dictionary<string, AudioSource> SFXLog;
    private Dictionary<string, AudioSource> musicLog;
    private Dictionary<string, AudioSource> UILog;
    private Dictionary<string, AudioSource> dialogueLog;
    private Dictionary<string, AudioSource> environmentLog;



    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ListToDict(SFXInput, SFXLog);
        ListToDict(musicInput, musicLog);
        ListToDict(UIInput, UILog);
        ListToDict(dialogueInput, dialogueLog);
        ListToDict(environmentInput, environmentLog);
        
    }

    private void ListToDict(List<AudioInput> entryList, Dictionary<string,AudioSource> entryDict)
    {
        foreach (var input in entryList)
        {
            entryDict.Add(input.name, input.audioSource);
        }
    }

    public void PlaySFX(string title)
    {
        if (SFXLog.ContainsKey(title)) // Check if the audio source exists in the dictionary
        {
            SFXLog[title].Play();
        }
        else
        {
            Debug.LogWarning("SFX not found: " + title); // Handle the case where the SFX is not found
        }
    }
    public void PlayMusic(string title)
    {
        if (musicLog.ContainsKey(title)) // Check if the audio source exists in the dictionary
        {
            musicLog[title].Play();
        }
        else
        {
            Debug.LogWarning("Music not found: " + title); // Handle the case where the music is not found
        }
    }

    // Play the UI sound by title
    public void PlayUI(string title)
    {
        if (UILog.ContainsKey(title)) // Check if the audio source exists in the dictionary
        {
            UILog[title].Play();
        }
        else
        {
            Debug.LogWarning("UI sound not found: " + title); // Handle the case where the UI sound is not found
        }
    }

    // Play the dialogue sound by title
    public void PlayDialogue(string title)
    {
        if (dialogueLog.ContainsKey(title)) // Check if the audio source exists in the dictionary
        {
            dialogueLog[title].Play();
        }
        else
        {
            Debug.LogWarning("Dialogue not found: " + title); // Handle the case where the dialogue sound is not found
        }
    }

    // Play the environment sound by title
    public void PlayEnvironment(string title)
    {
        if (environmentLog.ContainsKey(title)) // Check if the audio source exists in the dictionary
        {
            environmentLog[title].Play();
        }
        else
        {
            Debug.LogWarning("Environment sound not found: " + title); // Handle the case where the environment sound is not found
        }
    }
}
