using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControler : MonoBehaviour {
    
    public float speed;
    public float posLerpSmooth;
    public GameObject tail;
    public int tailNum;
    public int tailDistanceDelta;
    public AudioClip[] myclips;
    public AudioSource myAudio;

    private Rigidbody2D rgbd;
    private float mySpd;
    private GameObject[] stars;
    private int myStars = 0;
    private List<Vector2> posList = new List<Vector2>();
    private GameObject[] myTails;
    private int starNum;
    [HideInInspector]public int myBounce = 0;
    private bool isEnd = false;
    private RaycastHit2D hit;
    private Vector2 oldVel;
    private GameManager GM;
    private StageManager SM;
    private string colliderName = "null";
    private PopUpControler POP;

    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        SM = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();
        POP = GameObject.FindGameObjectWithTag("Popup").GetComponent<PopUpControler>();
        rgbd = GetComponent<Rigidbody2D>();
        starNum = SM.starNum;
        isEnd = false;

        myTails = new GameObject[tailNum];

        for(int k = 0; k < tailNum; k++)
        {
            myTails[k] = Instantiate(tail, transform.position, Quaternion.identity) as GameObject;
            myTails[k].GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1f - (((float)k + 1f) / (float)tailNum));
        }
        for(int k = 0; k < tailNum * tailDistanceDelta; k++)
        {
            posList.Add(transform.position);
        }

	}
    public void Play(Vector2 direction)
    {
        rgbd.velocity = direction.normalized * speed;
    }

    private void Update()
    {
        oldVel = rgbd.velocity;

        posList.RemoveAt(0);
        posList.Add(transform.position);
        
        for(int k = 0; k < tailNum; k++)
        {
            myTails[k].transform.position = posList[k * tailDistanceDelta];
        }

        if(SM.themMode == 2)
        {
            if(Mathf.Abs(transform.position.y) > 20)
            {
                if (transform.position.y > 0)
                {
                    transform.position = new Vector2(transform.position.x, -20);
                }
                else transform.position = new Vector2(transform.position.x, 20);
            }
            if(Mathf.Abs(transform.position.x) > 11.25)
            {
                if(transform.position.x > 0)
                {
                    transform.position = new Vector2(-11.25f, transform.position.y);
                }
                else transform.position = new Vector2(11.25f, transform.position.y);

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        bool countBounce = true;
        
        //벽
        if (SM.themMode == 1 && c.collider.tag == "Wall")
        {
            myAudio.clip = myclips[0];
            myAudio.Play();
            StartCoroutine(Mode1CheckDie());
        }
        else if(SM.themMode == 2 && c.collider.tag == "Wall")
        {
            /*myAudio.clip = myclips[2];
            myAudio.Play();*/
            countBounce = false;
        }
        else if (SM.themMode == 0)
        {
            if (c.collider.tag == "Wall")
            {
                myAudio.clip = myclips[0];
                myAudio.Play();
                Vector2 inNormal = c.contacts[0].normal;
                rgbd.velocity = Vector2.Reflect(oldVel, inNormal);
            }
        }
        //스트링
        if (c.collider.tag == "Basic" || c.collider.tag == "BasicBarrier") 
        {
            myAudio.clip = myclips[1];
            myAudio.Play();
            Vector2 inNormal = c.contacts[0].normal;
            rgbd.velocity = Vector2.Reflect(oldVel, inNormal);

            if (c.gameObject.GetComponentInParent<Animator>() != null) c.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");

        }
        else if (c.collider.tag == "Harf" || c.collider.tag == "HarfBarrier")
        {
            float angle = Mathf.Atan2(oldVel.y, oldVel.x) * Mathf.Rad2Deg;
            float colliderAngle = c.collider.transform.rotation.eulerAngles.z;
            angle = angle < 0 ? 360 + angle : angle;
            colliderAngle = Mathf.Abs(colliderAngle) < 0.01 ? 0 : colliderAngle;
            if(angle >= 180)
            {
                float endAngle = (angle + 180) % 360;
                if((colliderAngle <= 360 && colliderAngle > angle) || (colliderAngle >= 0 && colliderAngle < endAngle))
                {
                    myAudio.clip = myclips[2];
                    myAudio.Play();
                    if (c.gameObject.GetComponentInParent<Animator>() != null) c.gameObject.GetComponentInParent<Animator>().SetTrigger("Throw");
                    countBounce = false;
                }
                else
                {
                    myAudio.clip = myclips[1];
                    myAudio.Play();
                    Vector2 inNormal = c.contacts[0].normal;
                    rgbd.velocity = Vector2.Reflect(oldVel, inNormal);

                    if (c.gameObject.GetComponentInParent<Animator>() != null) c.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
                }
            }
            else
            {
                float endAngle = angle + 180;
                if(colliderAngle > angle && colliderAngle < endAngle)
                {
                    myAudio.clip = myclips[2];
                    myAudio.Play();
                    if (c.gameObject.GetComponentInParent<Animator>() != null) c.gameObject.GetComponentInParent<Animator>().SetTrigger("Throw");
                    countBounce = false;
                }
                else
                {
                    myAudio.clip = myclips[1];
                    myAudio.Play();
                    Vector2 inNormal = c.contacts[0].normal;
                    rgbd.velocity = Vector2.Reflect(oldVel, inNormal);

                    if (c.gameObject.GetComponentInParent<Animator>() != null) c.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
                }
            }
        }
        else if (c.collider.tag == "Once" || c.collider.tag == "OnceBarrier")
        {
            myAudio.clip = myclips[3];
            myAudio.Play();
            Vector2 inNormal = c.contacts[0].normal;
            rgbd.velocity = Vector2.Reflect(oldVel, inNormal);

            if (c.gameObject.GetComponentInParent<Animator>() != null) c.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
        }


        if (!isEnd && countBounce)
        {
            myBounce++;
            POP.Bounced();
            if (myBounce == SM.bounceLimit.x + 1)
            {
                GM.ChangeTheme(1);
                GM.ChangeBtnColor(1);
            }/*
            else if (myBounce == SM.bounceLimit.y + 1)
            {
                scoreAnim.SetTrigger("NotBad");
            }
            else if (myBounce == SM.bounceLimit.z + 1)
            {
                scoreAnim.SetTrigger("Last");
            }*/
        }   
    }
    IEnumerator Mode1CheckDie()
    {
        yield return new WaitForSeconds(1);
        if(GM.isPlaying) GM.StopGesture();

    }
    private void OnTriggerEnter2D(Collider2D c)
    {

        if (c.tag == "Star" && colliderName != c.name)
        {
            if(starNum == 1) //Stop
            {
                GM.Win();
                
                isEnd = true;
            }
            else
            {
                starNum--;
            }
            c.gameObject.GetComponent<Animator>().SetBool("isEnter", true);
            colliderName = c.name;
        }
        else if(c.tag == "MoreStar")
        {
            if(starNum == 1)
            {
                GM.Win();
                isEnd = true;
            }
            else
            {
                starNum--;
            }
            c.GetComponent<MoreStarCtrl>().BallHit();
        }
    }
}
