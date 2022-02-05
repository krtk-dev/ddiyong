using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryCtrl : MonoBehaviour
{
    public StageScrollCtrl stage;
    public StageSellectManager myManager;
    public SceneChanger sceneChanger;
    public GameObject contents;
    public Animator curtain;
    public Animator curtain2;
    public Animator parenthesis;
    public GameObject[] cards;
    public float minVelocity;
    public float smooth;
    public float range;
    public float minScale;
    public Sprite[] buttonImage;
    public bool isDown = false;
    private int minObj = 0; //수정 
    private int exceptCard = -1;
    public GameObject[] info;
    public Color infoColor;
    [HideInInspector] public int categoryNum;

    private void Update()
    {

        int num = 0;
        float minDistance = 1000;
        for (int k = 0; k < cards.Length; k++)
        {
            if (minDistance > Mathf.Abs(cards[k].transform.position.x))
            {
                minDistance = Mathf.Abs(cards[k].transform.position.x);
                num = k;
            }
        }
        if (minObj != num) curtain.SetBool("On", true);
        else curtain.SetBool("On", false);

        if (categoryNum == 0)
        {
            foreach (GameObject obj in info)
            {
                obj.GetComponent<Image>().color = infoColor;
                obj.GetComponent<Image>().raycastTarget = true;
            }
        }
        else
        {
            foreach (GameObject obj in info)
            {
                obj.GetComponent<Image>().color = Color.clear;
                obj.GetComponent<Image>().raycastTarget = false;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            isDown = false;
            StartCoroutine(Check2Lerp());
        }
        if (PlayerPrefs.GetInt("Stage") >= (categoryNum - 1) * 20 + stage.stageNum)
        {
            for(int k = 1; k < cards.Length; k++)
            {
                if(cards[k].GetComponentInChildren<Button>() != null) cards[k].GetComponentInChildren<Button>().gameObject.GetComponent<Image>().sprite = buttonImage[0];
            }
        }
        else
        {
            for (int k = 1; k < cards.Length; k++)
            {
                if(cards[k].GetComponentInChildren<Button>() != null) cards[k].GetComponentInChildren<Button>().gameObject.GetComponent<Image>().sprite = buttonImage[1];
            }
        }
    }
    private void OnMouseDown()
    {
        isDown = true;
        StopAllCoroutines();
    }

    public void ValueChanged()
    {
        if (isDown)
        {
            parenthesis.SetBool("On", true);
            for(int k = 1; k < cards.Length; k++)
            {
                cards[k].GetComponent<CategoryBoxCtrl>().AnimOnOff(false);
            }
        }
        foreach (GameObject obj in cards)
        {
            if (Mathf.Abs(obj.transform.position.x) < range) obj.GetComponent<CategoryBoxCtrl>().ChangeScale(1 - ((Mathf.Abs(obj.transform.position.x) * (1 - minScale)) / range));
            else obj.GetComponent<CategoryBoxCtrl>().ChangeScale(minScale);
        }
    }
    public void ButtonClicked()
    {
        if(PlayerPrefs.GetInt("Stage") >= (categoryNum - 1) * 20 + stage.stageNum)
        {
            myManager.ChangeTargetStage();
            sceneChanger.Fade2Scene(3);
        }
        else
        {
            curtain.SetTrigger("Open");
            int chepter = ((PlayerPrefs.GetInt("Stage") - 1) / 20) + 1;
            contents.GetComponent<RectTransform>().anchoredPosition = new Vector3((contents.GetComponent<HorizontalLayoutGroup>().spacing + 100) * -chepter, 0, 0);
        }
    }
    IEnumerator Check2Lerp()
    {
        while (Mathf.Abs(GetComponent<ScrollRect>().velocity.x) > minVelocity)
        {
            yield return null;
        }
        GetComponent<ScrollRect>().velocity = Vector2.zero;

        curtain.SetBool("On", false);
        parenthesis.SetBool("On", false);


        float minDistance = 1000; //임의의 큰수
        int num = 0;


        for (int k = 0; k < cards.Length; k++)
        {
            if (minDistance > Mathf.Abs(cards[k].transform.position.x))
            {
                minDistance = Mathf.Abs(cards[k].transform.position.x);
                num = k;
            }
        }
        minObj = num;
        if (num != 0)
        {
            cards[num].GetComponent<CategoryBoxCtrl>().AnimOnOff(true);
        }
        categoryNum = num;

        while (/*Mathf.Abs(contents.GetComponent<RectTransform>().anchoredPosition.x - ((contents.GetComponent<HorizontalLayoutGroup>().spacing + 100) * -num)) > 0.5*/true)
        {
            contents.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(contents.GetComponent<RectTransform>().anchoredPosition, new Vector3((contents.GetComponent<HorizontalLayoutGroup>().spacing + 100) * -num, 0, 0), smooth * Time.deltaTime);
            yield return null;
        }
    }
}
