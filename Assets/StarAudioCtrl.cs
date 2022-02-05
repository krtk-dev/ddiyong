using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAudioCtrl : MonoBehaviour
{
    public AudioClip first;
    public AudioClip hit;

    public void StarHit()
    {
        GetComponent<AudioSource>().clip = hit;
        GetComponent<AudioSource>().Play();
    }
    public void FirstLoad()
    {
        if(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().isFirstSceneLoad)
        {
            GetComponent<AudioSource>().clip = first;
            GetComponent<AudioSource>().Play();
        }
    }
}
