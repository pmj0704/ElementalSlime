using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDistroy : MonoBehaviour
{
    private AudioSource audioSource = null;
    void Start()
    {

        if(FindObjectsOfType<DontDistroy>().Length > 1)
        {
            Destroy(FindObjectsOfType<DontDistroy>()[1].gameObject);
        }

        audioSource = GetComponent<AudioSource>();  
        if(audioSource.isPlaying) return;
        else{
            audioSource.Play();
            DontDestroyOnLoad(gameObject);
            }
    }
}
