using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowRaycast : MonoBehaviour {

    public GameObject[] walls;
    public Material normal;
    public Material highlight;
    

    private RaycastHit hit;
    private GameObject myWall;
    private bool isHightlight = false;

    private void Update()
    {
        if(isHightlight)
        {
            LayerMask mask = LayerMask.GetMask("Map");

            if (Physics.Raycast(Vector3.zero, transform.position, out hit, Mathf.Infinity, mask))
            {
                if (hit.collider != null)
                {
                    for (int k = 0; k < walls.Length; k++)
                    {
                        walls[k].GetComponent<MeshRenderer>().material = normal;
                        if (walls[k].name == hit.collider.name) walls[k].GetComponent<MeshRenderer>().material = highlight;
                    }
                }
            }
        }
        else
        {
            for(int k = 0; k < walls.Length; k++)
            {
                walls[k].GetComponent<MeshRenderer>().material = normal;
            }
        }
    }
    public void IsHightlight(bool b)
    {
        isHightlight = b;
    }
}
