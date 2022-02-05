using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringMoveButtonControler : MonoBehaviour {

    public GameObject Arrow;

    public float unit;
    public float unitAngle;
    public AudioSource myAudio;

    private bool isDown = false;
    private int myButtonNum;
    private float myTime;

    public void ButtonDown(int buttonNum)
    {
        myButtonNum = buttonNum;

        if (myButtonNum < 4) StringMove(Num2Vector(myButtonNum));
        else if (myButtonNum == 4) StringRotation(-unitAngle);
        else if (myButtonNum == 5) StringRotation(unitAngle);

        myAudio.Play();

        isDown = true;
        myTime = 0;
    }
    public void ButtonExit()
    {
        isDown = false;
    }
    public void ButtonUp()
    {
        myAudio.Play();

        isDown = false;
    }

    private void Update()
    {
        if(myTime > 0.5f && isDown)
        {
            if (myButtonNum < 4) StringMove(Num2Vector(myButtonNum));
            else if (myButtonNum == 4) StringRotation(-unitAngle);
            else if (myButtonNum == 5) StringRotation(unitAngle);
        }
        if (isDown) myTime += Time.deltaTime;
    }

    private void StringMove(Vector2 direction)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.Translate(direction, Space.World);
        Arrow.transform.Translate(direction, Space.World);
    }
    private void StringRotation(float angle)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().sellectedString.transform.Rotate(Vector3.forward, angle);
        Arrow.transform.Rotate(Vector3.forward, angle);
    }
    private Vector2 Num2Vector(int num)
    {
        switch(num)
        {
            case 0: return new Vector2(-unit, 0);
            case 1: return new Vector2(unit, 0);
            case 2: return new Vector2(0, unit);
            case 3: return new Vector2(0, -unit);
            default : return Vector2.zero;
        }
    }
}
