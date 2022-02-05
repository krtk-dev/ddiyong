using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringShadow : MonoBehaviour {

    public Vector2 shadowDirection;
    public SpriteRenderer parentSprite;
    public Color normalColor;

    private SpriteRenderer mySprite;
    
    public void ChangeColor(bool isNormal)
    {
        if (isNormal) mySprite.color = normalColor;
        else mySprite.color = Color.clear;
    }

    private void Awake()
    {
        mySprite = GetComponent<SpriteRenderer>();
    }

    void Update () {
        transform.position = (Vector2)parentSprite.transform.position + shadowDirection;
        mySprite.size = parentSprite.size;
	}
}
