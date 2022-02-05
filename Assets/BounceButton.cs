using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceButton : MonoBehaviour
{

    public float speed;
    public Vector2 direction;

    private Rigidbody2D rgbd;

    void Awake()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rgbd.velocity = direction.normalized * speed;   
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        Vector2 inNormal = c.contacts[0].normal;
        rgbd.velocity = Vector2.Reflect(rgbd.velocity, inNormal);

        if (c.gameObject.GetComponentInParent<Animator>() != null) c.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
    }
}
