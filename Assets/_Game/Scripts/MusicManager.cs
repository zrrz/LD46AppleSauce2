using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] songs;

    [SerializeField]
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        //PlayRandomSong();
    }

    void Update()
    {
        if(audioSource.isPlaying == false)
        {
            PlayRandomSong();
        }
    }

    public void PlayRandomSong()
    {
        AudioClip song = songs[Random.Range(0, songs.Length)];
        audioSource.clip = song;
        audioSource.Play();
    }
}
