using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float speed;
    public float bumpSpeed;
    public float smooth;

    private Vector2 oldVel;
    private Rigidbody myRigidbody;
    private float mySpd;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        mySpd = Mathf.Lerp(mySpd, speed, smooth * Time.deltaTime);
        myRigidbody.velocity = myRigidbody.velocity.normalized * mySpd;
        oldVel = myRigidbody.velocity;
    }

    private void OnCollisionEnter(Collision c)
    {
        switch (c.transform.name)
        {
            case "SETTING": GameObject.FindGameObjectWithTag("GameController").GetComponent<AnimationManager>().ChangeMode(3); break;
            case "MAP": GameObject.FindGameObjectWithTag("GameController").GetComponent<AnimationManager>().ChangeMode(4); break;
            case "STORE": GameObject.FindGameObjectWithTag("GameController").GetComponent<AnimationManager>().ChangeMode(5); break;
            case "ITEM": GameObject.FindGameObjectWithTag("GameController").GetComponent<AnimationManager>().ChangeMode(6); break;
        }

        Vector2 inNormal = c.contacts[0].normal;

        myRigidbody.velocity = Vector2.Reflect(oldVel, inNormal);

        mySpd = bumpSpeed;

        ColliderTriger(true);
    }

    public void ColliderTriger(bool isTriger)
    {
        GetComponent<CapsuleCollider>().isTrigger = isTriger;
    }


}
