using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Checkpoints : MonoBehaviour
{
    public int currentCheckpoint;
    public List<int> score = new List<int>();
    public TextMeshProUGUI placingText;
    private Dictionary<int, bool> hasAddedScore = new Dictionary<int, bool>();
    public CineMachineHandler cineMachineHandler;
    public InGameUIHandler inGameUIHandler;


    private void Start()
    {
        score.Clear();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "Checkpoint":
                other.GetComponent<ThirdPersonMovement>().currentCheckpoint = this.currentCheckpoint;
                other.GetComponent<ThirdPersonMovement>().respawnPosition = this.transform.position;
                    other.GetComponent<ThirdPersonMovement>().respawnRotation = this.transform.localRotation;
               // Debug.Log(other.GetComponent<Player>().currentCheckpoint);
                break;

                case "FinishLine":
                    int playerID = other.GetComponent<ThirdPersonMovement>().playerInt;
                    if (!hasAddedScore.ContainsKey(playerID) || !hasAddedScore[playerID])
                    {
                        score.Add(playerID);
                        Debug.Log(other.name + " " + "Your placement :" + score.Count);
                        float totalSeconds = inGameUIHandler.matchTimer;
                        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
                        cineMachineHandler.playerTimeTexts[playerID].text = "player: " + other.GetComponent<ThirdPersonMovement>().playerInt + "'s placing: " + score.Count +
                             " Time: " + time.ToString("mm':'ss':'ms");
                        hasAddedScore[playerID] = true;
                    }
                break;

                case "Respawn":
                   // Debug.Log(other.GetComponent<Player>().respawnPosition);
                  
                    other.GetComponent<ThirdPersonMovement>().Respawn();
                break;
            }
        }
        
    }


}
