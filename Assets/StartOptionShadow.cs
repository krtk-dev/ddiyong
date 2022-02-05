using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOptionShadow : MonoBehaviour {

    public Vector2 distance;
    public GameObject image;

	void Update () {
        transform.position = (Vector2)image.transform.position + distance;
	}
}
