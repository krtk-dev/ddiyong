using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;

/* 클릭처리까지 
 * 
 * 
 */ 
public class GameManager : MonoBehaviour {

    //static
    public static int myBarMode = 0;
    

    //Animator
    public Animator barAnim;
    public Animator arrowAnim;
    public Animator noticeAnim;
    public Animator scoreAnim;
    public Animator hintAnim;
    public Animator hintAnim2;
    public Animator popupAnim;

    //Theme
    public GameObject[] myThemeUnits;
    public Color normalColor;
    public Color redColor;
    public Color lerpColor;
    public float colorLerpSpeed;
    private Coroutine myThemCorutine;

    //String
    //public GameObject stringsParents;
    public GameObject[] strings;

    //Line
    public GameObject dotsParents;
    public GameObject nail;
    public GameObject dot;
    public int dotsNum;
    public float dotsLerpSmooth;
    private Vector2[] dotsPos;
    private GameObject[] dots;

    //LineRevise
    public float angleReviseRange;
    private bool isFirstRevise = true;

    //stringMove
    [HideInInspector]public GameObject sellectedString;

    //Raycast
    private RaycastHit firstHit;
    private RaycastHit downHit;
    private RaycastHit upHit;
    private int barMod; //0 = idle, 1 = menu, 2 = string, 3 = movement
    private bool isBarOpened = false;
    private GameObject nail1, nail2;
    public float menuRange; //Radius
    public float lineRange; //Radius
    private bool isDowned = false;

    //StringHighlight;
    public GameObject ArrowParent;

    //Palette
    [HideInInspector]public List<int> stringData;
    public PaletteCtrl paletteCtrl;

    //Playing
    [HideInInspector]public bool isPlaying = false;
    public float doubleClickTime;
    private bool isTouched = false;
    [HideInInspector] public bool isFirstSceneLoad = true;

    //Player
    public GameObject ball;
    private GameObject myBall;
    private bool isWin = false;
    
    //Grid
    public GameObject grid;
    private bool isGridOpened = true;

    //StageValue
    private GameObject startPos;
    private GameObject directionPos;
    private Animator startOptionAnim;
    [HideInInspector]public int stageIndex = 1; //수정
    private Scene myStageScene;
    private bool isFirst = true;

    //hint
    [HideInInspector]public int stopCount = 0;
    [HideInInspector]public bool ishintOn = false;

    //Advertisement
    public BannerCtrl banner;

    //StopBtn
    public GameObject stopBtn;
    private Coroutine stopBtnCoroutine;

    //Audio
    public Sprite audioOn;
    public Sprite audioOff;
    public Image audioImage;
    public AudioSource myAudio;
    public AudioClip dragDown;
    public AudioClip dragUp;
    public AudioClip angle45;
    public AudioClip startAudio;
    public AudioClip installStringAudio;

    //Tutorial
    public GameObject tutorialCanvas;
    public GameObject tutorialCanvas2;

    //else
    public IngameSceneChanger sceneChanger;
    

    private const string appid = "3012329";


    private void Awake()
    {
        if (GameData.stageNum != 0) stageIndex = GameData.stageNum;
        if (stageIndex == 1)
        {
            grid.SetActive(false);
            isGridOpened = false;
            tutorialCanvas.SetActive(true);
        }
        if (PlayerPrefs.GetString("Mute") == "True")
        {
            audioImage.sprite = audioOff;
            AudioListener.pause = true;
        }
    }

    private void Start()
    {
        Advertisement.Initialize(appid);
        dots = new GameObject[dotsNum];
        dotsPos = new Vector2[dotsNum];
    }



    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPlaying)
            {
                sceneChanger.Fade2Scene(PlayerPrefs.GetInt("LastScene"));
            }
            else StopGesture();
        }
#endif
        if (Input.GetKeyDown(KeyCode.R))    //SceneReset
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(Input.GetMouseButtonDown(0))
        {
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            
            arrowAnim.gameObject.GetComponent<HighlightArrowCtrl>().isDown = false;
            if(Input.touchCount <= 1)
            {
                isDowned = false;
                if (isPlaying) StartCoroutine(DoubleClickCheck());
            }
        }

        //DotsLerp  iEnumerator로 수정??
        for (int k = 0; k < dots.Length; k++)
        {
            if (dots[k] != null) dots[k].transform.position = Vector2.Lerp(dots[k].transform.position, dotsPos[k], (dotsLerpSmooth + k) * Time.deltaTime);
        }

        //CheckStringWarning
        if (sellectedString != null)
        {
            arrowAnim.SetBool("isWarning", sellectedString.GetComponent<StringControler>().isWarning);
            ArrowParent.GetComponent<HighlightArrowCtrl>().WarningCheck(sellectedString.GetComponent<StringControler>().isWarning);
        }
        //print(stringData.ToArray().Length);
    }
    //IEnumerator
    IEnumerator DoubleClickCheck()
    {
        //if (isTouched) StopGesture();
        isTouched = true;
        yield return new WaitForSeconds(doubleClickTime);
        isTouched = false;
    }
    IEnumerator TouchEvent(bool isBackGround)

    {
        //FirstDownEvent
        if (isBackGround)barMod = isBarOpened ? 0 : 0; //fixed before 0:1
        else barMod = isBarOpened ? 3 : 1; //fixed before 3:1
        
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out firstHit, Mathf.Infinity))
        {
            if(isBackGround)
            {
                myAudio.clip = dragDown;
                myAudio.Play();
            }


            DestroyDotsNails();

            //nail dot creat
            nail1 = Instantiate(nail, firstHit.point, Quaternion.identity, dotsParents.transform) as GameObject;
            nail2 = Instantiate(nail, firstHit.point, Quaternion.identity, dotsParents.transform) as GameObject;

            for (int k = 0; k < dotsNum; k++)
            {
                dotsPos[k] = firstHit.point;
            dots[k] = Instantiate(dot, firstHit.point, Quaternion.identity, dotsParents.transform) as GameObject; //임의의 큰수
        }
            yield return null;

            isFirstRevise = true;
            //DowningEvent
            while (isDowned)
            {
                Ray ray1 = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray1, out downHit, Mathf.Infinity))
                {
                    RaycastHit myHit;
                    if(Physics.Raycast(nail1.transform.position, downHit.point - nail1.transform.position, out myHit, Vector3.Magnitude(downHit.point - nail1.transform.position), LayerMask.GetMask("StartOptionWall")))
                    {
                        nail2.transform.position = myHit.point;
                    }
                    else nail2.transform.position = downHit.point;

                    LineUp();

                    if (Vector3.Magnitude(downHit.point - firstHit.point) > lineRange && !isPlaying) // 각도 보정
                    {
                        AngleRevise();
                    }
                    
                    if (Vector3.Magnitude(downHit.point - firstHit.point) > menuRange)
                    {
                        barMod = 0;

                        if (myBarMode != 0)
                        {
                            BarAnimCtrl(0);
                        }
                    }
                    

                }
                yield return null;
            }

            //UpEvent
            Ray ray2 = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray2, out upHit, Mathf.Infinity))
            {
                if (isBackGround)
                {
                    myAudio.clip = dragUp;
                    myAudio.Play();
                }
                if (Vector3.Magnitude(nail2.transform.position - nail1.transform.position) > lineRange)
                {
                    if (stringData.Count == 0 && !isPlaying)
                    {
                        barMod = 0;
                        noticeAnim.SetTrigger("UseAll");
                    }
                    else barMod = 2;
                }
                if (barMod != 2)
                {
                    DestroyDotsNails();
                }

                if (!isBackGround && barMod == 1) barMod = 3;

                BarAnimCtrl(barMod);
                
            }
            if (isPlaying)
            {
                DestroyDotsNails();
                if (barMod == 2) noticeAnim.SetTrigger("DoubleClick");
            }
            if (stageIndex == 1 && barMod == 2) tutorialCanvas.GetComponent<TutorialCtrl>().Clicked();
        }

    }
    //function

    public void PlayAds2Next()
    {
        const string placementId = "video";
        if (Advertisement.IsReady())
        {
            if(PlayerPrefs.HasKey("ADS") && PlayerPrefs.GetString("ADS") == "true")
            {
                NextScene();
            }
            else
            {
                ShowOptions option = new ShowOptions { resultCallback = AdsResult };
                Advertisement.Show(placementId, option);
            }
        }
        else
        {
            NextScene();
        }
    }
    private void AdsResult(ShowResult result)
    {
        NextScene();
    }
    public void UnLoadScene()
    {
        SceneManager.UnloadSceneAsync(myStageScene);
    }
    public void NextScene()
    {
        isFirstSceneLoad = true;
        BarAnimCtrl(0);
        if(stageIndex == 1 && !isGridOpened)
        {
            isGridOpened = true;
            grid.SetActive(true);
        }
        //ResetButton();
        stageIndex++;
        if (stageIndex <= 80) SceneChanger(stageIndex + gameObject.scene.buildIndex);
        else sceneChanger.Fade2Scene(0);
        if (stageIndex == 2) tutorialCanvas2.SetActive(true);
        //OpenDetails();
    }
    public void SceneChanger(int index)//소환한 object들 삭제
    {
        //if (!isFirst) SceneManager.UnloadSceneAsync(myStageScene);
        SceneManager.LoadScene(index, LoadSceneMode.Additive);
        myStageScene = SceneManager.GetSceneByBuildIndex(index);
        //isFirst = false;
    }
    private void DestroyDotsNails()
    {
        Destroy(nail1);
        Destroy(nail2);
        for (int k = 0; k < dots.Length; k++)
        {
            Destroy(dots[k]);
        }
    }
    private void ChangeNailColor(Color color)
    {
        if (nail1 != null) nail1.GetComponent<SpriteRenderer>().color = color;
        if (nail2 != null) nail2.GetComponent<SpriteRenderer>().color = color;
        foreach(GameObject obj in dots)
        {
            obj.GetComponent<SpriteRenderer>().color = color;
        }
    }
    private void LineUp()
    {
        float deltaX = (nail2.transform.position.x - nail1.transform.position.x) / (dotsNum + 1);
        float deltaY = (nail2.transform.position.y - nail1.transform.position.y) / (dotsNum + 1);

        for (int k = 0; k < dotsNum; k++)
        {
            dotsPos[k] = new Vector2(nail1.transform.position.x + (deltaX * (k + 1)), nail1.transform.position.y + (deltaY * (k + 1)));
        }
    }
    private void AngleRevise()
    {
        float myAngle = Mathf.Atan2(nail1.transform.position.y - nail2.transform.position.y, nail1.transform.position.x - nail2.transform.position.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(myAngle) % 45 < angleReviseRange)
        {
            nail2.transform.RotateAround(nail1.transform.position ,Vector3.back, myAngle % 45);
            LineUp();
            for (int k = 0; k < dots.Length; k++)
            {
                if (dots[k] != null) dots[k].transform.position = dotsPos[k];
            }
            ChangeNailColor(redColor);
            if(isFirstRevise)
            {
                myAudio.clip = angle45;
                myAudio.Play();
                isFirstRevise = false;
            }
        }
        else if(Mathf.Abs(myAngle) % 45 > 45 - angleReviseRange)
        {
            
            nail2.transform.RotateAround(nail1.transform.position, Vector3.back, myAngle > 0 ? -(45 - (myAngle % 45)) : (45 + (myAngle % 45)));
            LineUp();
            for (int k = 0; k < dots.Length; k++)
            {
                if (dots[k] != null) dots[k].transform.position = dotsPos[k];
            }
            ChangeNailColor(redColor);
            if (isFirstRevise)
            {
                myAudio.clip = angle45;
                myAudio.Play();
                isFirstRevise = false;
            }
        }
        else
        {
            ChangeNailColor(Color.black);
            isFirstRevise = true;
        }

    }
    public void BarAnimCtrl(int modeIndex)
    {
        if(!isPlaying)
        {
            
            myBarMode = modeIndex;
            barAnim.SetInteger("Mode", modeIndex);

            if (modeIndex == 0) isBarOpened = false;
            else isBarOpened = true;
            paletteCtrl.StringValueUpdate();
        }
    }
    public void StringClick(GameObject _clickedString)
    {
        sellectedString = _clickedString;

    }
    public void ChangeTheme(int mode)
    {
        if(myThemCorutine != null) StopCoroutine(myThemCorutine);
        if (mode == 0) myThemCorutine = StartCoroutine(ColorLerp(normalColor));
        else if (mode == 1)
        {
            if (isPlaying) noticeAnim.SetBool("OverBounce", true);
            myThemCorutine = StartCoroutine(ColorLerp(redColor));
        }
        else if (mode == 2) myThemCorutine = StartCoroutine(ColorLerp(lerpColor));
    }
    public void RestoreTheme()
    {
        if (myThemCorutine != null) StopCoroutine(myThemCorutine);
        Color color = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().themMode == 0 ? normalColor : lerpColor;
        for (int k = 0; k < myThemeUnits.Length; k++)
        {
            if (k != 3) myThemeUnits[k].GetComponent<ProceduralImage>().color = color;
            else myThemeUnits[k].GetComponent<ProceduralImage>().color = color - new Color(0, 0, 0, 0.05f);
        }
    }
    IEnumerator ColorLerp(Color color)
    {
        int count = 0;
        while(count < 200)
        {
            for (int k = 0; k < myThemeUnits.Length; k++)
            {
                if (k != 3) myThemeUnits[k].GetComponent<ProceduralImage>().color = Color.Lerp(myThemeUnits[k].GetComponent<ProceduralImage>().color, color, colorLerpSpeed * Time.deltaTime);
                else myThemeUnits[k].GetComponent<ProceduralImage>().color = Color.Lerp(myThemeUnits[k].GetComponent<ProceduralImage>().color, color - new Color(0, 0, 0, 0.05f), colorLerpSpeed * Time.deltaTime);
            }
            count++;
            yield return null;
        }

    }
    public int StringObject2StringNumber(GameObject obj)
    {
        switch(obj.tag)
        {
            case "Basic": return 0;
            case "PtBasic": return 0;
            case "Harf": return 1;
            case "PtHarf": return 1;
            case "Once": return 2;
            case "PtOnce": return 2;

            default: return 0;
        }
    }
    public void DestroyString(GameObject obj)
    {
        ArrowAnimRestore();
        stringData.Add(StringObject2StringNumber(obj));
        Destroy(obj);
    }
    public void Win()
    {
        stopCount = 0;
        hintAnim.SetBool("Shake", false);
        hintAnim2.SetBool("Shake", false);
        isWin = true;
        scoreAnim.SetBool("isOpen", true);
        if (stageIndex == 1) tutorialCanvas.SetActive(false);
        else if (stageIndex == 2) tutorialCanvas2.SetActive(false);
    }

    //Button EventTrigger Gesture
    public void BackgroundDown() //EventTriger
    {
        if(Input.touchCount <= 1)
        {
            isDowned = true;
            StartCoroutine(TouchEvent(true));
        }
    }
    public void StringDown()
    {
        if(Input.touchCount <= 1)
        {
            isDowned = true;
            StartCoroutine(TouchEvent(false));
        }
    }
    public void InstantiateString(int index)
    {
        myAudio.clip = installStringAudio;
        myAudio.Play();

        Vector2 pos = (nail1.transform.position + nail2.transform.position) / 2;
        Quaternion rot = Quaternion.Euler(0, 0, Mathf.Atan2(nail2.transform.position.y - nail1.transform.position.y, nail2.transform.position.x - nail1.transform.position.x) * Mathf.Rad2Deg);
        GameObject myString = Instantiate(strings[index], pos, rot, GameObject.FindGameObjectWithTag("StringParent").transform) as GameObject;
        myString.GetComponent<CapsuleCollider>().height = Vector3.Magnitude(nail1.transform.position - nail2.transform.position);
        myString.GetComponent<StringControler>().CreatMotion(nail1.transform.position, pos, Vector3.Magnitude(nail1.transform.position - nail2.transform.position));

        stringData.Remove(StringObject2StringNumber(myString));
        DestroyDotsNails();
        StringClick(myString);
        BarAnimCtrl(3);
    }
    public void ArrowAnimRestore()
    {
        arrowAnim.SetBool("isWarning", false);
        ArrowParent.GetComponent<HighlightArrowCtrl>().WarningCheck(false);
    }

    public void ResetButton() //string종류별로 다 , 일단
    {
        for(int k = 0; k < GameObject.FindGameObjectsWithTag("Basic").Length; k++)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Basic")[k]);
        }
        for (int k = 0; k < GameObject.FindGameObjectsWithTag("Harf").Length; k++)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Harf")[k]);
        }
        for (int k = 0; k < GameObject.FindGameObjectsWithTag("Once").Length; k++)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Once")[k]);
        }
        GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().ResetString();
    }
    public void DeleteButton()
    {
        DestroyString(sellectedString);
        BarAnimCtrl(0);
    }
    public void PlayButton()
    {
        BarAnimCtrl(5);
        popupAnim.SetBool("isPlaying", true);
        isFirstSceneLoad = false;
        GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().HintSwitch(false);
        isWin = false;
        myAudio.clip = startAudio;
        myAudio.Play();
        GameObject.FindGameObjectWithTag("StartArea").GetComponent<Animator>().SetBool("isPlaying", true);
        DestroyDotsNails();
        isPlaying = true;
        isTouched = false;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Once")) if(obj.GetComponent<Animator>() != null) obj.GetComponent<Animator>().SetBool("isOpen", false);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("OnceBarrier")) if (obj.GetComponentInParent<Animator>() != null) obj.GetComponentInParent<Animator>().SetBool("isOpen", false);
        if (myBall != null) Destroy(myBall);
        myBall = Instantiate(ball, (Vector2)GameObject.FindGameObjectWithTag("StartPos").transform.position, Quaternion.identity) as GameObject;
        myBall.GetComponent<BallControler>().Play(GameObject.FindGameObjectWithTag("Direction").transform.position - GameObject.FindGameObjectWithTag("StartPos").transform.position);
    }
    public void StopGesture() //버튼은 아님
    {
        if(!isWin)
        {
            stopCount++;
            if (stopCount >= 2)
            {
                hintAnim.SetBool("Shake", true);
                hintAnim2.SetBool("Shake", true);
            }
            popupAnim.gameObject.GetComponent<PopUpControler>().BounceReset();
            noticeAnim.SetBool("OverBounce", false);
            GameObject.FindGameObjectWithTag("StartArea").GetComponent<Animator>().SetBool("isPlaying", false);
            popupAnim.SetBool("isPlaying", false);
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Tail")) Destroy(obj);
            Destroy(myBall);
            isPlaying = false;
            isTouched = false;
            BarAnimCtrl(0);
            ChangeBtnColor(0);
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Star"))
            {
                obj.GetComponent<Animator>().SetBool("isEnter", false);
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MoreStar")) obj.GetComponent<MoreStarCtrl>().RestoreStars();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Once")) if (obj.GetComponent<Animator>() != null) obj.GetComponent<Animator>().SetBool("isOpen", true);
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("OnceBarrier")) if (obj.GetComponentInParent<Animator>() != null) obj.GetComponentInParent<Animator>().SetBool("isOpen", true);
            RestoreTheme();
        }
        GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().HintSwitch(ishintOn);
    }
    public void EndScene()
    {
        popupAnim.gameObject.GetComponent<PopUpControler>().BounceReset();
        GameObject.FindGameObjectWithTag("StartArea").GetComponent<Animator>().SetBool("isPlaying", false);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Tail")) Destroy(obj);
        Destroy(myBall);
        isPlaying = false;
        isTouched = false;
        popupAnim.SetBool("isClear", true);
        popupAnim.SetBool("isPlaying", false);
        ChangeBtnColor(0);
        BarAnimCtrl(0);
        noticeAnim.SetBool("OverBounce", false);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Star"))
        {
            obj.GetComponent<Animator>().SetBool("isEnter", false);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MoreStar")) obj.GetComponent<Animator>().SetBool("isDied", false);

        GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().HintSwitch(ishintOn);
        ChangeTheme(GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().themMode);

    }

    public void GridButton()
    {
        isGridOpened = !isGridOpened;
        grid.SetActive(isGridOpened);
    }
    public void Hint()
    {
        const string RewardedPlacementId = "rewardedVideo";

        if (!Advertisement.IsReady(RewardedPlacementId) || ishintOn)
        {
            return;
        }

        ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show(RewardedPlacementId, options);

    }


    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().HintSwitch(true);
                hintAnim.SetBool("Shake", false);
                hintAnim2.SetBool("Shake", false);
                ishintOn = true;
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
        }
    }

    public void MenuBtn()
    {
        if(!isPlaying) BarAnimCtrl(1);
    }
    public void DotsCancel()
    {
        BarAnimCtrl(0);
        DestroyDotsNails();
    }
    public void ChangeBtnColor(int colorNum)
    {
       if(stopBtnCoroutine != null) StopCoroutine(stopBtnCoroutine);
        stopBtnCoroutine = StartCoroutine(BtnColorLerper(colorNum));
    }
    IEnumerator BtnColorLerper(int Num)
    {
        Color btnColor = stopBtn.GetComponent<ProceduralImage>().color;
        Color targetColor = Num == 0 ? normalColor : redColor;
        while (btnColor != targetColor)
        {
            btnColor = Color.Lerp(btnColor, targetColor, colorLerpSpeed * Time.deltaTime);
            stopBtn.GetComponent<ProceduralImage>().color = btnColor;
            yield return null;
        }
    }
    public void AudioCtrl()
    {
        AudioListener.pause = !AudioListener.pause;
        PlayerPrefs.SetString("Mute", AudioListener.pause.ToString());
        audioImage.sprite = AudioListener.pause ? audioOff : audioOn;
    }
}
