using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public WordSearchController wordSearchController;
    public Text clockText;
    public bool clockOn, countDown, countUp;
    public float time, totalSeconds, minutes, seconds;
    public string minutesStr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clockOn)
        {
            minutes = Mathf.Floor(time / 60);
            seconds = Mathf.RoundToInt(time % 60);

            if (seconds >= 10.0f) clockText.text = minutes + ":" + seconds.ToString("0");
            if (seconds < 9.5f) clockText.text = minutes + ":0" + seconds.ToString("0");

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

    public void StartClock(int startTime, int endTime)
    {

        if (startTime < endTime) { countUp = true; }
        else { countDown = true; }

        totalSeconds = endTime;
        time = startTime;
        clockOn = true;
        wordSearchController.timeUp = false;
    }

    public void StopClock()
    {
        clockOn = false;
        wordSearchController.timeUp = true;
    }
}
