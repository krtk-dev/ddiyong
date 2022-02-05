using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    [HideInInspector]public int starNum;
    public Vector3 bounceLimit;
    public bool isAds = true;
    public int themMode = 0;
    public GameObject firstHint;
    public GameObject secondHint;
    public GameObject hints;
    [HideInInspector]public int[] strings;

    private List<GameObject> myAssets = new List<GameObject>();
    private List<GameObject> myStrings = new List<GameObject>();
    private Coroutine coroutune;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().ishintOn = false;
        starNum = GameObject.FindGameObjectsWithTag("Star").Length;
        GameObject.FindGameObjectWithTag("Popup").GetComponent<Animator>().SetBool("isClear", false);
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("MoreStar"))
        {
            starNum += obj.GetComponent<MoreStarCtrl>().num;
        }

        FindStrings();

        myAssets.Add(GameObject.FindGameObjectWithTag("StartArea"));
        myAssets.AddRange(GameObject.FindGameObjectsWithTag("Star"));
        myAssets.AddRange(GameObject.FindGameObjectsWithTag("MoreStar"));
        //myAssets.AddRange(GameObject.FindGameObjectsWithTag("Barrier"));

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().ResetButton();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().ChangeTheme(themMode);

        //ResetString();
    }
    private void FindStrings()
    {
        strings[0] = GameObject.FindGameObjectsWithTag("Basic").Length / 2; //콜라이더가 두개여서 tag를 두개씀
        strings[1] = GameObject.FindGameObjectsWithTag("Harf").Length / 2;
        strings[2] = GameObject.FindGameObjectsWithTag("Once").Length / 2;

        GameObject.FindGameObjectWithTag("Popup").GetComponent<PopUpControler>().DataSetting();

        //리스트에 add
        myStrings.AddRange(GameObject.FindGameObjectsWithTag("Basic"));
        myStrings.AddRange(GameObject.FindGameObjectsWithTag("Harf"));
        myStrings.AddRange(GameObject.FindGameObjectsWithTag("Once"));

        //hint 생성
        int i = 0;
        for(int k = 0; k < myStrings.Count; k++)
        {
            if(myStrings[k].GetComponentInChildren<SpriteRenderer>() != null)
            {
                GameObject prf = i % 2 == 0 ? firstHint : secondHint;
                GameObject obj = Instantiate(prf, myStrings[k].transform.position, myStrings[k].transform.rotation, hints.transform) as GameObject;
                obj.GetComponent<SpriteRenderer>().size = new Vector2(myStrings[k].GetComponentInChildren<SpriteRenderer>().size.x, obj.GetComponent<SpriteRenderer>().size.y);
                i++;
            }
        }
    }
    public void HintSwitch(bool isOn)
    {
        hints.SetActive(isOn);
    }
    public void ResetString()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stringData.Clear();
        for (int k = 0; k < strings.Length; k++)
        {
            for (int i = 0; i < strings[k]; i++)
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stringData.Add(k);
            }
        }
        GameObject.FindGameObjectWithTag("Palette").GetComponent<PaletteCtrl>().CreatString();

        foreach(GameObject obj in myAssets)
        {
            if(obj.tag == "MoreStar")
            {
                obj.GetComponent<MoreStarCtrl>().DestroyInfo();
            }
            obj.SetActive(false);
            obj.SetActive(true);
        }

        StopAllCoroutines();
        StartCoroutine(ScaleControl());
    }
    IEnumerator ScaleControl()
    {
        foreach (GameObject obj in myAssets)
        {
            obj.GetComponent<Animator>().SetTrigger("Start");
            yield return new WaitForSeconds(0.1f);
        }
    }
}
