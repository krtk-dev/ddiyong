using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringCtrl : MonoBehaviour {

    public GameObject nail1;
    public GameObject nail2;

    private void Update()
    {
        transform.position = (nail1.transform.position + nail2.transform.position) / 2;
        transform.localScale = new Vector3(transform.localScale.x, Vector3.Magnitude(nail1.transform.position - nail2.transform.position), transform.localScale.z);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan((nail2.transform.position.y - nail1.transform.position.y) / (nail2.transform.position.x - nail1.transform.position.x)) * Mathf.Rad2Deg + 90);
    }
}
