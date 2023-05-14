using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    // Start is called before the first frame update
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
