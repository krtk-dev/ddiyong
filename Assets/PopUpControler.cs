using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpControler : MonoBehaviour {

    public Text stageNum;
    public Text bounceText;
    public GameObject[] stringBoxs;
    public GameObject parent;
    public GameObject normal;
    public GameObject over;
    private GameObject[] dots;
    private int limitBounce;

    [HideInInspector] public int bounceCount = 0;

    public void DataSetting()
    {
        stageNum.text = "LV " + GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex.ToString();
        Vector3 bounceV3 = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().bounceLimit;
        bounceText.text = bounceV3.x.ToString();
        for(int k = 0; k < stringBoxs.Length; k++)
        {
            stringBoxs[k].gameObject.SetActive(false);
            if(GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().strings[k] > 0)
            {
                stringBoxs[k].gameObject.SetActive(true);
                stringBoxs[k].GetComponentInChildren<Text>().text = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().strings[k].ToString();
            }
        }
        bounceCount = 0;
        for(int k = 0; dots != null && k < dots.Length; k++)
        {
            Destroy(dots[k]);
        }
        limitBounce = (int)GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().bounceLimit.x;
        dots = new GameObject[limitBounce + 1];
        for (int k = 0; k < limitBounce; k++)
        {
            dots[k] = Instantiate(normal, parent.transform) as GameObject;
        }
        dots[limitBounce] = Instantiate(over, parent.transform) as GameObject;
    }
    public void Bounced()
    {
        if(bounceCount <= limitBounce)
        {
            dots[bounceCount].GetComponent<Animator>().SetBool("Hit", true);
            bounceCount++;
        }
    }
    public void BounceReset()
    {
        for(int k = 0; k < dots.Length; k++)
        {
            dots[k].GetComponent<Animator>().SetBool("Hit", false);
        }
        bounceCount = 0;
    }
}
