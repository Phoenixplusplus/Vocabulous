using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public WordSearchController wordSearchController;
    public Text clockText;
    public bool clockOn;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clockOn)
        {
            time -= Time.deltaTime;
            if (time >= 10.0f) clockText.text = "0:" + time.ToString("0");
            if (time < 9.5f) clockText.text = "0:0" + time.ToString("0");
            if (time <= 0)
            {
                StopClock();
            }
        }
    }

    public void StartClock(int startTime)
    {
        time = startTime;
        clockOn = true;
        wordSearchController.timeUp = false;
    }

    public void StopClock()
    {
        clockOn = false;
        wordSearchController.timeUp = true;
        clockText.text = "0:00";
    }
}
