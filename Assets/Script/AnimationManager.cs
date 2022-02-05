using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 0 = main, 1 = clicked, 2 = btnUp, 3 = setting, 4 = map, 5 = store, 6 = item
 * 0 <- 초기화
 * 
 */ 
public class AnimationManager : MonoBehaviour {

    public Animator canvasAnim;
    public Animator arrowAnim;
    public GameObject ball;
    public GameObject[] nails;
    public GameObject[] arrows;
    public Vector2[] posMain;   //해상도에 일반화
    public Vector2[] posTop;
    public Vector2[] posRight;
    public Vector2[] posBtm;
    public Vector2[] posLeft;
    public float smooth;

    private int myMod = 0;
    private Vector2[] myVector;
    private Vector3[] arrowPos = new Vector3[4];
    private bool isFirstLoad = true;

    private void Start()
    {
        for(int k = 0; k < arrows.Length; k++)
        {
            arrowPos[k] = arrows[k].transform.position;
        }
        ChangeMode(0);
    }

    public void ChangeMode(int n)
    {
        myMod = n;
        canvasAnim.SetInteger("Mode", n);
        arrowAnim.SetInteger("Mode", n);

        switch (n)
        {
            case 0: myVector = posMain; StartCoroutine(Mode0()); break;
            case 3: myVector = posTop; break;
            case 4: myVector = posRight; break;
            case 5: myVector = posBtm; break;
            case 6: myVector = posLeft; break;
        }

    }
    IEnumerator Mode0()
    {
        //ball
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.position = Vector3.zero;
        ball.transform.rotation = Quaternion.Euler(90, 0, 0);
        ball.GetComponent<PlayerMove>().ColliderTriger(false);

        isFirstLoad = false;


        
        yield return new WaitForEndOfFrame();//초기화
        GetComponent<ClickManager>().ResetValue();

        for (int k = 0; k < arrows.Length; k++)
        {
            arrows[k].transform.position = arrowPos[k];
            arrows[k].transform.rotation = Quaternion.Euler(0, 0, -k * 90);
        }

        arrows[0].GetComponent<ArrowRaycast>().IsHightlight(false);
    }

    private void FixedUpdate()
    {
        if(myMod >= 3 || myMod == 0)
        {
            for(int k = 0; k < nails.Length; k++)
            {
                nails[k].transform.position = Vector2.Lerp(nails[k].transform.position, myVector[k], smooth * Time.fixedDeltaTime);
            }
        }
    }


}
