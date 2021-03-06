﻿//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public WordSearchController wordSearchController;
    public FreeWordController freeWordController;
    public Text clockText;
    public bool clockOn, countDown, countUp;
    public float time, totalSeconds, minutes, seconds;


    // Update is called once per frame
    void Update()
    {
        if (clockOn)
        {
            SetTime();
            if (countDown)
            {
                time -= Time.deltaTime;
                if (time <= totalSeconds) { StopClock(); }
            }
            else
            {
                time += Time.deltaTime;
                if (time >= totalSeconds) { StopClock(); }
            }
        }
    }

    // main method .. call this from any class in update, to get the displayed time, then do behaviour
    public void SetTime(float secs)
    {
        time = secs;
        SetTime();
    }

    public void SetTime()
    {
        minutes = Mathf.Floor(time / 60);
        seconds = Mathf.RoundToInt(time % 60);

        if (seconds >= 10.0f) clockText.text = minutes + ":" + seconds.ToString("0");
        if (seconds < 9.5f) clockText.text = minutes + ":0" + seconds.ToString("0");
    }

    public string ConvertTimeToString(float time)
    {
        minutes = Mathf.Floor(time / 60);
        seconds = Mathf.RoundToInt(time % 60);

        if (seconds >= 10.0f) return clockText.text = minutes + ":" + seconds.ToString("0");
        if (seconds < 9.5f) return clockText.text = minutes + ":0" + seconds.ToString("0");
        return clockText.text = minutes + ":" + seconds.ToString("0");
    }

    public void StartClock(int startTime, int endTime)
    {

        if (startTime < endTime) { countUp = true; }
        else { countDown = true; }

        totalSeconds = endTime;
        time = startTime;
        clockOn = true;
        if (wordSearchController != null) wordSearchController.timeUp = false;
        if (freeWordController != null) freeWordController.timeUp = false;
    }

    public void StopClock()
    {
        clockOn = false;
        if (wordSearchController != null) wordSearchController.timeUp = true;
        if (freeWordController != null) freeWordController.timeUp = true;
    }
}
