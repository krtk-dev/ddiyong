using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnAudioCtrl : MonoBehaviour
{
    AudioSource AS;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }
    public void Clicked()
    {
        AS.Play();
    }
}
