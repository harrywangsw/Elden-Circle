using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ding : MonoBehaviour
{
    AudioSource aud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.Play();
    }

    void Update()
    {
        if(!aud.isPlaying){
            Destroy(gameObject);
        }
    }
}
