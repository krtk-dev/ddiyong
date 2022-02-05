using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class StageScrollCtrl : MonoBehaviour
{
    public GameObject contents;
    public GameObject[] cards;
    public Color[] colors;
    public Sprite rocked;
    public Sprite[] tutorial;
    public Sprite[] sprites;
    public float maxScale;
    public float range;
    public float minVelocity;
    public float smooth;
    [HideInInspector] public int stageNum; //0으로 초기화 해주기 curtain후에

    private int scores;

    private void Awake()
    {
        ChangeChepter(0);
    }

    private void Update()
    {
        foreach(GameObject obj in cards)
        {
            if (Mathf.Abs(obj.transform.position.x) < range) obj.GetComponent<StageBoxCtrl>().ChangeScale(maxScale - Mathf.Abs((obj.transform.position.x * (maxScale - 1)) / range));
            else obj.GetComponent<StageBoxCtrl>().ChangeScale(1f);
        }

    }
    public void ChangeChepter(int chepter, int Stage = 0)
    {
        StopAllCoroutines();
        contents.GetComponent<RectTransform>().anchoredPosition = new Vector2((contents.GetComponent<HorizontalLayoutGroup>().spacing + 100) * -Stage, 0);
        GetComponent<ScrollRect>().velocity = Vector2.zero;

        stageNum = Stage;

        for(int k = 0; k < 21; k++)
        {
            if (chepter != 0)
            {
                cards[k].SetActive(true);
                if(k == 0)
                {
                    scores = 0;
                    for(int i = 0; i < 20; i++)
                    {
                        if (PlayerPrefs.HasKey(((chepter - 1) * 20 + i + 1).ToString()))
                        {
                            if(PlayerPrefs.GetInt(((chepter - 1) * 20 + i + 1).ToString()) == 0) scores++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (scores == 20) cards[k].GetComponent<StageBoxCtrl>().ChangeInfo(sprites[(chepter - 1) * 21 + k], colors[0], "");
                    else cards[k].GetComponent<StageBoxCtrl>().ChangeInfo(sprites[(chepter - 1) * 21 + k], colors[3], "");
                    cards[k].GetComponent<StageFirstBoxCtrl>().ChangeFirstBoxInfo(scores);
                }
                else
                {
                    if (PlayerPrefs.HasKey(((chepter - 1) * 20 + k).ToString()))
                    {
                        cards[k].GetComponent<StageBoxCtrl>().ChangeInfo(sprites[(chepter - 1)* 21 + k], colors[PlayerPrefs.GetInt(((chepter - 1) * 20 + k).ToString())], ((chepter - 1) * 20 + k).ToString());
                    }
                    else if(PlayerPrefs.GetInt("Stage") == (chepter - 1) * 20 + k)
                    {
                        cards[k].GetComponent<StageBoxCtrl>().ChangeInfo(sprites[(chepter - 1) * 21 + k], colors[3], "GO");
                    }
                    else
                    {
                        cards[k].GetComponent<StageBoxCtrl>().ChangeInfo(sprites[(chepter - 1) * 21 + k], colors[2], "???");
                    }
                }
            }
            else
            {
                if (k == 0)
                {
                    cards[k].GetComponent<StageFirstBoxCtrl>().ChangeFirstBoxInfo(-1);
                    cards[k].GetComponent<StageBoxCtrl>().ChangeInfo(tutorial[k], colors[3], "");
                }
                else if (k >= tutorial.Length) cards[k].SetActive(false);
                else cards[k].GetComponent<StageBoxCtrl>().ChangeInfo(tutorial[k], colors[3], "");
            }

        }
    }

    public void PointerDown()
    {
        StopAllCoroutines();
    }
    public void PointerUp()
    {
        StartCoroutine(Check2Lerp());
    }
    IEnumerator Check2Lerp()
    {
        while(Mathf.Abs(GetComponent<ScrollRect>().velocity.x) > minVelocity)
        {
            yield return null;
        }
        GetComponent<ScrollRect>().velocity = Vector2.zero;
        float minDistance = 1000; //임의의 큰수
        int num = 0;


        for(int k = 0; k < cards.Length; k++)
        {
            if(minDistance > Mathf.Abs(cards[k].transform.position.x))
            {
                minDistance = Mathf.Abs(cards[k].transform.position.x);
                num = k;
            }
        }
        stageNum = num;
        while(/*Mathf.Abs(contents.GetComponent<RectTransform>().anchoredPosition.x - ((contents.GetComponent<HorizontalLayoutGroup>().spacing + 100) * -num)) > 0.5*/true)
        {
            contents.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(contents.GetComponent<RectTransform>().anchoredPosition, new Vector3((contents.GetComponent<HorizontalLayoutGroup>().spacing + 100) * -num, 0, 0), smooth * Time.deltaTime);
            yield return null;
        }
    }

}
