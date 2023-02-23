using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    //All the stored Audio
    public AudioGroup[] audioGroups;

    //Gets the Audio that needs to be played, or when there are more Clips it will pick a random one.
    public AudioClip GetAudio(string name)
    {
        if (audioGroups != null)
        {
            for (int i = 0; i < audioGroups.Length; i++)
            {
                if (audioGroups[i].name == name)
                {
                    AudioGroup group = audioGroups[i];
                    return group.clips[Random.Range(0, group.clips.Length)];
                }
            }
        }
        return null;
    }
}

[System.Serializable]
public struct AudioGroup
{
    //Name of the clip.
    public string name;
    //All the clips from that group.
    public AudioClip[] clips;
}
