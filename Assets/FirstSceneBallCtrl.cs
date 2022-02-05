using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneBallCtrl : MonoBehaviour
{

    public float speed;
    public GameObject tail;
    public int tailNum;
    public int tailDistanceDelta;
    public AudioClip wallAudio;
    public AudioClip stringAudio;
    public AudioSource myAudio;

    private Rigidbody2D rgbd;
    private List<Vector2> posList = new List<Vector2>();
    private GameObject[] myTails;
    [HideInInspector]public Vector2 myVector;
    [HideInInspector]public Vector2 oldVel;

    void Awake()
    {
        myAudio = GetComponent<AudioSource>();
        rgbd = GetComponent<Rigidbody2D>();

        myTails = new GameObject[tailNum];

        for (int k = 0; k < tailNum; k++)
        {
            myTails[k] = Instantiate(tail, transform.position, Quaternion.identity) as GameObject;
            myTails[k].GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1f - (((float)k + 1f) / (float)tailNum));
        }
        for (int k = 0; k < tailNum * tailDistanceDelta; k++)
        {
            posList.Add(transform.position);
        }
    }
    private void Update()
    {
        oldVel = rgbd.velocity;

        posList.RemoveAt(0);
        posList.Add(transform.position);

        for (int k = 0; k < tailNum; k++)
        {
            myTails[k].transform.position = posList[k * tailDistanceDelta];
        }
    }
    /*
    private void FixedUpdate()
    {
        transform.Translate(myVector.normalized * Time.fixedDeltaTime * speed);
    }*/
    private void OnCollisionEnter2D(Collision2D c)
    {
        Vector2 inNormal = c.contacts[0].normal;
        //myVector = Vector2.Reflect(myVector, inNormal);
        rgbd.velocity = Vector2.Reflect(oldVel, inNormal);
        if (c.gameObject.GetComponent<Animator>() != null) c.gameObject.GetComponent<Animator>().SetTrigger("Hit");
        if (c.collider.tag == "Popup")
        {
            myAudio.clip = stringAudio;
            myAudio.Play();
        }

    }

}
