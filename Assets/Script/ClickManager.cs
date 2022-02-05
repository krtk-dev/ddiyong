using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour {

    public Camera mainCamera;
    public GameObject[] arrows;
    public float smooth;
    public GameObject ball;
    public ArrowRaycast ARR;
    public GameObject light;

    private RaycastHit hit;
    private float angleSum;

    //초기화 변수
    private bool isFirstTouch = true;
    private bool isDown = false;
    private bool isOverLap = false;

    void Update ()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if(Input.GetMouseButtonDown(0))
        {
            DownEvent();

            if(isFirstTouch) GetComponent<AnimationManager>().ChangeMode(1);
        }
        if (Input.GetMouseButton(0))
        {
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if(isFirstTouch)
            {

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    isDown = true;
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            UpEvent();
        }

        if(isDown)
        {
            light.transform.LookAt(hit.point);

            angleSum = 0;

            for (int k = 0; k < arrows.Length; k++)
            {
                float angle = -Vector2.SignedAngle(arrows[k].transform.position, hit.point);
                if(!isOverLap) angle = angle * smooth * Time.deltaTime;
                arrows[k].transform.RotateAround(Vector3.zero, Vector3.back, angle);
                angleSum += Mathf.Abs(angle);
            }
            if (angleSum < 0.3 && isFirstTouch) isOverLap = true;
        }
	}

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void UpEvent()
    {
        if (isFirstTouch)
        {
            GetComponent<AnimationManager>().ChangeMode(2);
            ball.GetComponent<Rigidbody>().velocity = new Vector3(hit.point.normalized.x, hit.point.normalized.y, 0) * ball.GetComponent<PlayerMove>().speed;
        }
        isFirstTouch = false;

        ARR.IsHightlight(false);
    }
    private void DownEvent()
    {
        if(isFirstTouch) ARR.IsHightlight(true);
    }
    public void ResetValue()
    {
        isFirstTouch = true;
        isDown = false;
        isOverLap = false;
    }
}
