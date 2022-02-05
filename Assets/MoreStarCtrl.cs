using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreStarCtrl : MonoBehaviour
{
    public int num; //10이하로
    public GameObject littleStar;
    public GameObject starParent;
    public GameObject Silhouette;

    private List<GameObject> starList = new List<GameObject>();
    private float distance = 1.2f;
    private float angle = 40;
    private int starCount = 0;
    private int remainStars;
    private float smooth = 5f;

    private void Start()
    {
        Destroy(Silhouette);
    }

    IEnumerator OpenStar()
    {
        
        remainStars = num;
        for(int k = 0; k < num; k++)
        {
            if (k == 0)
            {
                starList.Add(Instantiate(littleStar, gameObject.transform) as GameObject);
                starList[0].GetComponent<Animator>().SetBool("isMain", true);
            }
            else
            {
                starList.Add(Instantiate(littleStar, starParent.transform) as GameObject);
                starList[k].transform.localPosition = new Vector2(0, distance);
                starList[k].transform.RotateAround(starParent.transform.position, Vector3.back, angle * starCount);
                starCount++;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    public void DestroyInfo()
    {
        foreach (GameObject obj in starList) Destroy(obj);
        starList.Clear();
    }
    public void RestoreStars()
    {
        if (remainStars != 0) StartCoroutine(AddStars(num - remainStars));
        GetComponent<Animator>().SetBool("isDied", false);
    }
    IEnumerator AddStars(int n)
    {
        remainStars = num;
        for (int k = 0; k < n; k++)
        {
            GameObject obj = Instantiate(littleStar, starParent.transform) as GameObject;
            obj.transform.localPosition = new Vector2(0, distance);
            obj.transform.RotateAround(starParent.transform.position, Vector3.back, angle * starCount);
            starList.Add(obj);
            
            starCount++;
            yield return new WaitForSeconds(0.1f); //너무빨리 실행하면 버그 생길 수도 
        }
    }
    public void BallHit()
    {
        starList[0].GetComponent<Animator>().SetTrigger("Die");
        Destroy(starList[0], 5);//임이의 시간 대강
        starList.RemoveAt(0);
        if (remainStars > 1)
        {
            StartCoroutine(ChangeMain(starList[0]));
        }
        else
        {
            GetComponent<Animator>().SetBool("isDied", true);
        }
        remainStars--;

    }
    IEnumerator ChangeMain(GameObject obj)
    {
        

        obj.GetComponent<Animator>().SetBool("isMain", true);
        obj.transform.SetParent(gameObject.transform);
        while(Vector3.Magnitude(obj.transform.position - gameObject.transform.position) > 0.05f || Mathf.Abs(obj.transform.rotation.eulerAngles.z % 360) > 0.05f)
        {
            obj.transform.position = Vector2.Lerp(obj.transform.position, gameObject.transform.position, smooth * Time.deltaTime);
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, Quaternion.identity, smooth * Time.deltaTime);
            yield return null;
        }
        obj.transform.position = gameObject.transform.position;
        obj.transform.rotation = Quaternion.identity;
    }


    void Update()
    {
        
    }
}
