using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* string별로 reflect주면될듯
 * isplaying에 하나씩 지우기?
 * 
 */ 
public class AimDotsCtrl : MonoBehaviour {

    public GameObject dotPrefab;

    public float dotsDeltaDistance;
    public float radius;
    public int bounceNum;

    private RaycastHit2D hit;
    private Vector2 myPos;
    private Vector2 myDr;
    private List<GameObject> myDots = new List<GameObject>();
    private float remainDistance;
    private bool isOpen = false;

    private GameObject StartPos;
    private GameObject direction;

    private void Update()
    {
        dotsDeltaDistance = Mathf.Abs(dotsDeltaDistance);
        if (dotsDeltaDistance == 0) dotsDeltaDistance = 1f;


        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AimDot")) Destroy(obj);

        if(!GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().isPlaying && isOpen)
        {
            myPos = GameObject.FindGameObjectWithTag("StartPos").transform.position;
            myDr = GameObject.FindGameObjectWithTag("Direction").transform.position - GameObject.FindGameObjectWithTag("StartPos").transform.position;
            remainDistance = 0;

            for (int k = 0; k < bounceNum; k++)
            {
                hit = Physics2D.Raycast(myPos, myDr);

                for (float i = remainDistance; i <= Vector3.Magnitude(hit.point - myPos) - radius; i += dotsDeltaDistance)
                {
                    Instantiate(dotPrefab, myPos + (new Vector2(Mathf.Cos(Mathf.Atan2(myDr.y, myDr.x)), Mathf.Sin(Mathf.Atan2(myDr.y, myDr.x))) * i) /*+ new Vector2(Mathf.Cos(Mathf.Atan2(myDr.y, myDr.x)), Mathf.Sin(Mathf.Atan2(myDr.y, myDr.x)) + remainDistance)*/, Quaternion.identity, transform);
                }

                remainDistance = (Vector3.Magnitude(hit.point - myPos) - radius) % dotsDeltaDistance;
                myPos = hit.point - (new Vector2(Mathf.Cos(Mathf.Atan2(myDr.y, myDr.x)), Mathf.Sin(Mathf.Atan2(myDr.y, myDr.x))) * radius);
                //if (hit.collider.tag == "Wall") 
                myDr = Vector2.Reflect(myDr, hit.normal);
            }
        }
    }

    public void SetOpen()
    {
        isOpen = !isOpen;
    }
}
