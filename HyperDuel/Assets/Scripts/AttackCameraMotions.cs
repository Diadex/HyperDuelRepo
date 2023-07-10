using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackCameraMotions : MonoBehaviour
{
    public bool isFirstStart = true;
    public float goDistance = 1;
    private float backgroundSize = 4.8f;
    public float firstGoSpeed;
    private Vector3 startpos;
    public AnimationCurve firstRollCurve1;
    public AnimationCurve firstRollCurve2;
    public float firstAnimTİme = 1;
    public float percentageAnim;
    public float firstDurationCounter;
    public float animationSchedule = 0;


    public AnimationCurve secondRollCurve1;
    public AnimationCurve secondRollCurve2;
    public float secondAnimTİme = 1;
    public float secondGoSpeed;
    public float goDistance2 = 4;
    public SpriteRenderer FaceSprite;
    private bool flag1 = false;
    private float waitTime = 1;
    private bool waitFlag = true;

    public float characterShowWaitDuration = 1f;
    void Update()
    {
        if (animationSchedule == 0) cameraRollRightAnim();
        else if (animationSchedule == 1) waitTimer(characterShowWaitDuration);
        else if (animationSchedule == 2) closeUpFaceAnim();
    }

    private void waitTimer(float duration)
    {
        if (waitFlag) 
        { 
            waitTime = duration;
            waitFlag = false;
        }
        if ( waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            animationSchedule++;
            waitFlag = true;
        }
    }

    private void closeUpFaceAnim()
    {
        if (isFirstStart)
        {
            firstDurationCounter = Time.deltaTime;
            percentageAnim = 0;
            isFirstStart = false;
            startpos = Vector3.zero;
            transform.position = startpos;
            flag1 = true;
        }
        if (percentageAnim < 0.5f)
        {
            firstDurationCounter = Time.deltaTime;
            percentageAnim += firstDurationCounter * secondGoSpeed / secondAnimTİme;
            Vector3 vec1 = Vector3.Lerp(startpos, new Vector3( (-1 * goDistance2 * backgroundSize), 0, 0), secondRollCurve1.Evaluate(percentageAnim * 2));
            transform.position = vec1;
        }
        else if (percentageAnim < 1)
        {
            if ( flag1) { FaceSprite.enabled = true; flag1 = false; }
            firstDurationCounter = Time.deltaTime;
            percentageAnim += firstDurationCounter * secondGoSpeed / secondAnimTİme;
            Vector3 vec1 = Vector3.Lerp(new Vector3((goDistance2 * backgroundSize), 0, 0) , Vector3.zero, secondRollCurve2.Evaluate((percentageAnim - 0.5f) * 2));
            transform.position = vec1;
        }
        else
        {
            transform.position = Vector3.zero;
            animationSchedule++;
        }
    }

    private void cameraRollRightAnim()
    {
        if (isFirstStart)
        {
            FaceSprite.enabled = false;
            firstDurationCounter = Time.deltaTime;
            percentageAnim = 0;
            isFirstStart = false;
            startpos = new Vector3((-1 * goDistance * backgroundSize), transform.position.y, transform.position.z);
            transform.position = startpos;
        }
        if (percentageAnim < 0.5f)
        {
            firstDurationCounter = Time.deltaTime;
            percentageAnim += firstDurationCounter * firstGoSpeed / firstAnimTİme;
            Vector3 vec1 = Vector3.Lerp(startpos, new Vector3(startpos.x / 2, 0, 0), firstRollCurve1.Evaluate(percentageAnim * 2));
            transform.position = vec1;
        }
        else if (percentageAnim < 1)
        {
            firstDurationCounter = Time.deltaTime;
            percentageAnim += firstDurationCounter * firstGoSpeed / firstAnimTİme;
            Vector3 vec1 = Vector3.Lerp(new Vector3(startpos.x / 2, 0, 0), Vector3.zero, firstRollCurve2.Evaluate((percentageAnim - 0.5f) * 2));
            transform.position = vec1;
        }
        else
        {
            transform.position = Vector3.zero;
            animationSchedule++;
            isFirstStart = true;
        }
    }
}
