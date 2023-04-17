using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public int currentCheckpoint;
    public List<int> score = new List<int>();
    public UnityEngine.UI.Text placingText;
    private Dictionary<int, bool> hasAddedScore = new Dictionary<int, bool>();


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

                other.GetComponent<Player>().currentCheckpoint = this.currentCheckpoint;
                other.GetComponent<Player>().respawnPosition = this.transform.position;
                Debug.Log(other.GetComponent<Player>().currentCheckpoint);
                break;

                case "FinishLine":
                    int playerID = other.GetComponent<Player>().playerInt;
                    if (!hasAddedScore.ContainsKey(playerID) || !hasAddedScore[playerID])
                    {
                        score.Add(playerID);
                        Debug.Log("Your placement :" + score.Count);
                        print("swag");
                        placingText.text = "Your placing: " + score.Count;
                        hasAddedScore[playerID] = true;
                    }
                break;

                case "Respawn":
                    Debug.Log(other.GetComponent<Player>().respawnPosition);
                    print("deez");
                    other.GetComponent<Player>().Respawn();
                break;
            }
        }
        
    }


}
