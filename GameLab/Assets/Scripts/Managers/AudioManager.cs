using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Manager
{
    public float musicVolume { get; private set; }
    public float sfxVolume { get; private set; }
    public float masterVolume { get; private set; }

    [SerializeField]
    private AudioSource[] musicSources;
    private int musicIndex;
    private AudioListener audioListeners;
    private int activeMusicSourceIndex;


    public List<AudioSource> SFXSources = new List<AudioSource>();
    private int sfxIndex;

    public override void Awake()
    {
        //Sets the default volume of all the Sound Types
        SetDefaultVolume();

        //All the music sources.
        musicSources = new AudioSource[2];


        //The amount of Musicsources in the array will be created within the scene, and it will not destroy when the game loads a different scene. 
        for (int i = 0;
            i < musicSources.Length; i++)
        {
            GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            Object.DontDestroyOnLoad(newMusicSource);
        }
        //Saves the Volumes in the player prefs
        musicVolume = PlayerPrefs.GetFloat("music Volume", 1);
        sfxVolume = PlayerPrefs.GetFloat("sfx Volume", 1);
        masterVolume = PlayerPrefs.GetFloat("master Volume", 1);
        PlayerPrefs.Save();
    }
    //Sets Default Volume and saves it
    public void SetDefaultVolume(float musicVolume = 1f, float sfxVolume = 1f, float masterVolume = 1f)
    {
        PlayerPrefs.SetFloat("music Volume", musicVolume);
        PlayerPrefs.SetFloat("sfx Volume", sfxVolume);
        PlayerPrefs.SetFloat("master Volume", masterVolume);
        PlayerPrefs.Save();
    }
    //Sets the Volume when called for the Audio type
    public void SetVolume(float volumePercent, AudioType type)
    {
        switch (type)
        {
            case AudioType.Music:
                musicVolume = volumePercent;
                break;
            case AudioType.sfx:
                sfxVolume = volumePercent;
                break;
            case AudioType.Master:
                masterVolume = volumePercent;
                break;
            default:
                break;
        }
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].volume = musicVolume;
        }
        for (int i = 0; i < SFXSources.Count; i++)
        {
            //SFXSources[i].volume = sfxVolume;
        }
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].volume = masterVolume;
            for (int j = 0; j < SFXSources.Count; j++)
            {
                SFXSources[j].volume = masterVolume;
            }
        }
        SetDefaultVolume(musicVolume, sfxVolume, masterVolume);

    }
    //Mutes the Audio when called.
    public void Mute(bool mute)
    {

        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].enabled = mute;
        }
    }
    //gets the audio from the AudioLibrary with the given name and Plays the music.
    public void PlayMusic(string clipName, float fadeLength = 1f)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = GameManager.instance.GetComponent<AudioLibrary>().GetAudio(clipName);
        musicSources[activeMusicSourceIndex].Play();
    }
    //Creates the Sound that must be played with the given name that it gets from the audiolibrary and destroys it right after it has been played.
    public void PlaySound(string soundName)
    {
        GameObject sourceObj = new GameObject();
        AudioSource source = sourceObj.AddComponent<AudioSource>();
        source.name = "AudioClip";
        source.clip = GameManager.instance.GetComponent<AudioLibrary>().GetAudio(soundName);
        source.Play();
        SFXSources.Add(source);
        GameObject.Destroy(sourceObj, source.clip.length);
    }

    public void StopPlaying()
    {
        musicSources[activeMusicSourceIndex].Stop();
    }

    public void CancelSound()
    {
        SFXSources.Clear();
    }

    public override void Pause(bool pause)
    {
        base.Pause(pause);
    }
}

//All the Audio Types
public enum AudioType
{
    Music,
    sfx,
    Master
}



