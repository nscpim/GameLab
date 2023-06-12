using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    /// <summary>
    /// At the start of the game sets the spawnpoints to add them to the gamemanager
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.instance.spawnPoints.Add(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
