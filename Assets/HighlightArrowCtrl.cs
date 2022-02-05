using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightArrowCtrl : MonoBehaviour {

    public float smooth;
    public float colorSmooth;
    public Color normalColor;
    public Color highlightColor;
    public float gap;
    public float range;
    public GameObject arrow1;
    public GameObject arrow2;
    public AudioSource myAudio;

    public Animator rotationAnim;

    private Color myColor = Color.clear;
    private bool isFirstTouch = true;
    [HideInInspector]public bool isDown = false;
    private RaycastHit hit;
    private GameObject myObj;
    private GameObject notObj;
    private bool isFirst = true;

	void Update () {
        
        if (GameManager.myBarMode == 3)
        {
            arrow1.GetComponent<BoxCollider>().enabled = true;
            arrow2.GetComponent<BoxCollider>().enabled = true;
            if (isDown)
            {
                float slope = (arrow1.transform.position.y - arrow2.transform.position.y) / (arrow1.transform.position.x - arrow2.transform.position.x);

                Ray ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    float posX = (hit.point.x + (hit.point.y * slope) + (notObj.transform.position.x * slope * slope) - (notObj.transform.position.y * slope)) / ((slope * slope) + 1);
                    float posY = slope * (posX - notObj.transform.position.x) + notObj.transform.position.y;
                    Vector2 pos = new Vector2(posX, posY);

                    if (Mathf.Abs(myObj.transform.position.x - notObj.transform.position.x) < 0.01) //infinity
                    {
                        pos = new Vector2(myObj.transform.position.x, hit.point.y);

                    }
                    if (Vector3.Magnitude((Vector2)(myObj.transform.position - notObj.transform.position).normalized - (pos - (Vector2)notObj.transform.position).normalized) > 0.9 || Vector3.Magnitude(pos - (Vector2)notObj.transform.position) < range) //0.9는 1보다작은 임의의 수
                    {
                        pos = (Vector2)notObj.transform.position + ((Vector2)(myObj.transform.position - notObj.transform.position).normalized * range);
                    }
                    
                    myObj.transform.position = pos;
                    float mag = Vector3.Magnitude(pos - (Vector2)notObj.transform.position);

                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.position = (pos + (Vector2)notObj.transform.position) / 2;
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.GetComponent<StringControler>().ChangeSize(mag - (2 * gap));

                    transform.position = (pos + (Vector2)notObj.transform.position) / 2;
                    arrow1.transform.localPosition = new Vector2(mag / 2, 0);
                    arrow2.transform.localPosition = new Vector2(-mag / 2, 0);

                }
                arrow1.GetComponent<SpriteRenderer>().color = Color.Lerp(arrow1.GetComponent<SpriteRenderer>().color, myColor, colorSmooth * Time.deltaTime);
                arrow2.GetComponent<SpriteRenderer>().color = Color.Lerp(arrow2.GetComponent<SpriteRenderer>().color, myColor, colorSmooth * Time.deltaTime);
            }
            else
            {
                Quaternion targetRot = Quaternion.Euler(0, 0, Mathf.Abs(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.rotation.eulerAngles.z % 180));
                Vector2 arrowPos = new Vector2(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.GetComponentInChildren<SpriteRenderer>().size.x / 2 + gap, 0);
                if (isFirstTouch)
                {
                    transform.position = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.position;
                    transform.rotation = targetRot;
                    arrow1.transform.localPosition = arrowPos;
                    arrow2.transform.localPosition = -arrowPos;
                    
                }
                isFirstTouch = false;

                transform.position = Vector2.Lerp(transform.position, GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.position, smooth * Time.deltaTime);


                //Quaternion targetRot = Quaternion.Euler(0, 0, GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, smooth * Time.deltaTime);

                arrow1.transform.localPosition = Vector2.Lerp(arrow1.transform.localPosition, arrowPos, smooth * Time.deltaTime);
                arrow2.transform.localPosition = Vector2.Lerp(arrow2.transform.localPosition, -arrowPos, smooth * Time.deltaTime);

                arrow1.GetComponent<SpriteRenderer>().color = Color.Lerp(arrow1.GetComponent<SpriteRenderer>().color, myColor, colorSmooth * Time.deltaTime);
                arrow2.GetComponent<SpriteRenderer>().color = Color.Lerp(arrow2.GetComponent<SpriteRenderer>().color, myColor, colorSmooth * Time.deltaTime);
            }
            if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.tag == "Harf") rotationAnim.SetBool("isOpen", true);
            else rotationAnim.SetBool("isOpen", false);
        }
        else
        {
            isFirstTouch = true;
            arrow1.GetComponent<SpriteRenderer>().color = new Color(normalColor.r, normalColor.g, normalColor.b, 0);
            arrow2.GetComponent<SpriteRenderer>().color = new Color(normalColor.r, normalColor.g, normalColor.b, 0);
            arrow1.GetComponent<BoxCollider>().enabled = false;
            arrow2.GetComponent<BoxCollider>().enabled = false;
            rotationAnim.SetBool("isOpen", false);
        }
        
    }
    private void ChangeSize()
    {

    }
    public void WarningCheck(bool isWarning)
    {
        if (isWarning)
        {
            myColor = highlightColor;
            if (isFirst)
            {
                isFirst = false;
                myAudio.Play();
            }
        }
        else
        {
            isFirst = true;
            myColor = normalColor;
        }
    }
    public void PointerDown(bool isLeft)
    {
        isDown = true;
        if (isLeft)
        {
            myObj = arrow2;
            notObj = arrow1;
        }
        else
        {
            myObj = arrow1;
            notObj = arrow2;
        }
    }
    public void RotationPointerDown()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.RotateAround(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.position, Vector3.back, 180);
    }
}
