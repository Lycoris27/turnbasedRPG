using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePersister : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToPersist = new List<GameObject>();
    private void Awake()
    {
        foreach (var obj in objectsToPersist)
        {
            DontDestroyOnLoad(obj);
        }
    }
}
