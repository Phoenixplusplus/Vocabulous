using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash_Tester : MonoBehaviour
{
    public bool TESTME;
    public FlashPanelManager FM;

    public Vector2 StartPos;
    public Vector2 MiddlePos;
    public Vector2 FinishPos;

    public float StartAlpha;
    public float MiddleAlpha;
    public float FinishAlpha;

    public float MiddleHeight;
    public float StartHeight;
    public float FinishHeight;

    public string myMessage1;
    public string myMessage2;

    public Color TextColor1;
    public Color TextColor2;

    public Tween tween1;
    public Tween tween2;

    public float AnimTime;
    public float MiddleTimeRatio;

    // Update is called once per frame
    void Update()
    {
        if (TESTME)
        {
            FlashTemplate FT = new FlashTemplate();
            {

            }
            FM.CustomFlash(FT);
            TESTME = false;
        }
    }
}
