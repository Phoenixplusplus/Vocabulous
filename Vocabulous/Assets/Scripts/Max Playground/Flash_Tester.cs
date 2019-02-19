using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash_Tester : MonoBehaviour
{
    public bool TESTME;
    public FlashPanelManager FM;
    [SerializeField]
    Vector2 MouseCoords = new Vector2();
    [Header("Flash Template Creator")]
    public bool SingleLerp;

    public Vector2 StartPos;
    public Vector2 MiddlePos;
    public Vector2 FinishPos;

    public float StartAlpha;
    public float MiddleAlpha;
    public float FinishAlpha;

    public float StartHeight;
    public float MiddleHeight;
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
        MouseCoords.x = Input.mousePosition.x / Screen.width;
        MouseCoords.y = Input.mousePosition.y / Screen.height;

        if (TESTME)
        {
            FlashTemplate FT = new FlashTemplate();
            {
                FT.SingleLerp = SingleLerp;

                FT.StartPos = StartPos;
                FT.MiddlePos = MiddlePos;
                FT.FinishPos = FinishPos;

                FT.StartAlpha = StartAlpha;
                FT.MiddleAlpha = MiddleAlpha;
                FT.FinishAlpha = FinishAlpha;

                FT.StartHeight = StartHeight;
                FT.MiddleHeight = MiddleHeight;
                FT.FinishHeight = FinishHeight;

                FT.myMessage1 = myMessage1;
                FT.myMessage2 = myMessage2;

                FT.TextColor1 = TextColor1;
                FT.TextColor2 = TextColor2;

                FT.tween1 = tween1;
                FT.tween2 = tween2;

                FT.AnimTime = AnimTime;
                FT.MiddleTimeRatio = MiddleTimeRatio;
            }
            FM.CustomFlash(FT);
            TESTME = false;
        }
    }
}
