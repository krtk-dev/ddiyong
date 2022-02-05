using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailContoler : MonoBehaviour {

    public GameObject nailPrf;
    public int x;
    public int y;
    public float distance;
    public Vector2 btnPos;
    public float smooth;
    public Color myColor;

    private GameObject[] nails;
    private Vector2[] nailPos;
    private bool isNailOn = false;
    private Color targetColor;
    private Vector2 targetPos;
    private float colorSmooth;



	void Start () {
        nails = new GameObject[x * y];
        nailPos = new Vector2[x * y];
        int count = 0;

        for (int k = 0; k < y; k++)
        {
            for(int i = 0; i < x; i++)
            {
                float xPos = -(x - 1) / 2 + i;
                float yPos = -(y - 1) / 2 + k;

                nails[count] = Instantiate(nailPrf, btnPos, Quaternion.identity, transform) as GameObject;
                nailPos[count] = new Vector2(xPos * distance, yPos * distance);
                count++;
            }
        }

        targetColor = myColor;
	}

    private void Update()
    {
        if(isNailOn)
        {
            for(int k = 0; k < nails.Length; k++)
            {
                nails[k].transform.position = Vector2.Lerp(nails[k].transform.position, nailPos[k] + (Vector2)transform.position, smooth * Time.deltaTime);
            }
        }
        else
        {
            for (int k = 0; k < nails.Length; k++)
            {
                nails[k].transform.position = Vector2.Lerp(nails[k].transform.position, btnPos, smooth * Time.deltaTime);
            }
        }
        for(int k = 0; k < nails.Length; k++)
        {
            nails[k].GetComponent<SpriteRenderer>().color = Color.Lerp(nails[k].GetComponent<SpriteRenderer>().color, targetColor, colorSmooth * Time.deltaTime);
        }

    }

    public void ColorChanger(bool active)
    {
        if (!active || !isNailOn)
        {
            targetColor = Color.clear;
            colorSmooth = 10f;
        }
        else
        {
            targetColor = myColor;
            colorSmooth = 3f;
        }

    }
    public void PosChanger()
    {
        isNailOn = !isNailOn;
        if (isNailOn) ColorChanger(true);
        else ColorChanger(false);
    }
}
