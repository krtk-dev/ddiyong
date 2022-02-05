using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    public GameObject myBall;
    public float maxSize;
    public float minSize;
    public float maxDistance;
    public float smooth;


    private Camera myCamera;
    private float cameraSize;

    private void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    void Update () {

        Vector3 pos = new Vector3(myBall.transform.position.x, myBall.transform.position.y, -10f);
        //transform.position = Vector3.Lerp(transform.position, pos, smooth * Time.deltaTime);
        transform.position = pos;
        myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, cameraSize, smooth * Time.deltaTime);
	}
    public void SetCameraSize(float minDistance)
    {
        if (maxDistance <= minDistance) cameraSize = maxSize;
        else cameraSize = minSize + (minDistance / maxDistance * (maxSize - minSize));
    }
}
