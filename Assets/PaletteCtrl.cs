using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteCtrl : MonoBehaviour {

    public GameObject[] bigPrefabs;

    private GameObject[] myStrings; //←↓두개 합칠 수 있을텐데..
    private List<int> temp = new List<int>();
    private int lastData;

    public void CreatString()
    {
        for (int j = 0; myStrings != null && j < myStrings.Length; j++)
        {
            Destroy(myStrings[j]);
        }
        myStrings = null;
        temp.Clear();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stringData.Sort();
        int k = -1;
        int i = 0;
        bool isFirst = true;
        
        foreach(int data in GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stringData) //중복 제거
        {
            if (!temp.Contains(data))//temp에 list[i]의 원소가 있으면 true 아니면 false이므로 (!가 있어서 원소가 없을때 true로 바뀌어서 추가된다.)
                temp.Add(data);//temp list에 추가한다.
        }
        
        myStrings = new GameObject[temp.Count]; 
        foreach (int data in GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stringData)
        {
            i++;
            if (isFirst || lastData != data)
            {
                i = 1;
                k++;
                myStrings[k] = Instantiate(bigPrefabs[data], transform) as GameObject;
            }

            myStrings[k].GetComponent<PaletteStringTextCtrl>().ChangeValue(i);
            lastData = data;
            isFirst = false;
        }
    }
    public void StringValueUpdate()
    {
        foreach(GameObject obj in myStrings)
        {
            if(obj != null) obj.GetComponent<PaletteStringTextCtrl>().ChangeValue(0);
        }

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stringData.Sort();
        int i = 0;
        bool isFirst = true;
        GameObject myObj = myStrings[0];

        foreach (int data in GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stringData)
        {
            i++;
            if (isFirst || lastData != data)
            {
                i = 1;
                for(int j = 0; j < myStrings.Length; j++)
                {
                    if (myStrings[j] != null && data == GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().StringObject2StringNumber(myStrings[j])) myObj = myStrings[j];
                }
            }
            myObj.GetComponent<PaletteStringTextCtrl>().ChangeValue(i);
            lastData = data;
            isFirst = false;
        }
        foreach (GameObject obj in myStrings)
        {
            obj.GetComponent<Animator>().SetBool("isDown", false);
        }
        StopAllCoroutines();
    }
    public void StartMode2Anim()
    {
        foreach (GameObject obj in myStrings)
        {
            obj.GetComponent<Animator>().SetBool("isDown", false);
        }
        StopAllCoroutines();
        StartCoroutine(OpenStringsAnim());
    }
    IEnumerator OpenStringsAnim()
    {
        yield return null;

        for (int k = 0; k < myStrings.Length; k++)
        {
            myStrings[k].GetComponent<Animator>().SetBool("isDown", true);
            StartCoroutine(AnimSetBoolFalse(myStrings[k].GetComponent<Animator>()));
            yield return new WaitForSeconds(0.06f);
        }

    }
    IEnumerator AnimSetBoolFalse(Animator anim)
    {
        //yield return new WaitForSeconds(0.3f);
        yield return null;
        anim.SetBool("isDown", false);
    }
}
