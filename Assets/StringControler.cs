using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringControler : MonoBehaviour {

    [HideInInspector] public bool isWarning = false;

    public float smooth;
    public float alpha;
    public GameObject childCollider;
    public SpriteRenderer mySprite;

    private Vector2 endPos;
    private Vector2 targetSize;
    private bool isStringAnimDoing = false;

    public void CreatMotion(Vector2 startPos, Vector2 _endPos, float _targetSize)
    {
        
        endPos = _endPos;
        targetSize = new Vector2(_targetSize, mySprite.size.y);

        mySprite.size = new Vector2(0, targetSize.y);
        transform.position = startPos;
        childCollider.GetComponent<CapsuleCollider2D>().size = new Vector2(targetSize.x, childCollider.GetComponent<CapsuleCollider2D>().size.y);
        isStringAnimDoing = true;
        StartCoroutine(StringAnim());
    }
    public void ChangeSize(float width)
    {
        isStringAnimDoing = false;
        StopAllCoroutines();
        mySprite.size = new Vector2(width, mySprite.size.y);
        childCollider.GetComponent<CapsuleCollider2D>().size = new Vector2(width, childCollider.GetComponent<CapsuleCollider2D>().size.y);
        GetComponent<CapsuleCollider>().height = width;
    }

    IEnumerator StringAnim()
    {
        while(Mathf.Abs(targetSize.x - mySprite.size.x) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, endPos, smooth * Time.deltaTime);
            mySprite.size = Vector2.Lerp(mySprite.size, targetSize, smooth * Time.deltaTime);

            yield return null;
        }
        transform.position = endPos;
        mySprite.size = targetSize;
        isStringAnimDoing = false;
    }
    private void LateUpdate()
    {
        if(!isStringAnimDoing)
        {
            Vector2 startPos = new Vector2(transform.position.x - (Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * mySprite.size.x / 2), transform.position.y - (Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * mySprite.size.x / 2));
            Vector2 endPos = new Vector2(transform.position.x + (Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * mySprite.size.x / 2), transform.position.y + (Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * mySprite.size.x / 2));


            Ray ray = new Ray(startPos, endPos - startPos);
            int layer = LayerMask.GetMask("StartOptionWall");

            if (Mathf.Abs(startPos.x) > 11.4f || Mathf.Abs(endPos.x) > 11.4f || Mathf.Abs(startPos.y) > 20.1f || Mathf.Abs(endPos.y) > 20.1f || mySprite.size.x <= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().lineRange)
            {
                isWarning = true;

                mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, alpha);

                if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString != gameObject || GameManager.myBarMode != 3)
                {
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().DestroyString(gameObject);
                }
            }
            else if (Physics.Raycast(ray, mySprite.size.x - 0.1f, layer))
            {
                isWarning = true;

                mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, alpha);

                if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString != gameObject || GameManager.myBarMode != 3)
                {
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().DestroyString(gameObject);
                }
            }
            else
            {
                mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 1);
                isWarning = false;
            }
        }
    }
    /*
    private void OnCollisionStay(Collision collision)
    {
        
        if(collision.collider.tag == "Wall") //시작점도 추가
        {
            print("1");
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, minusAlpha);
            if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString != gameObject || GameManager.myBarMode != 3)
            {
                Destroy(gameObject);
            }
        }
        
    }*/
}
