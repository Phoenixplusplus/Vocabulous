using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashPro_Tester : MonoBehaviour
{
    public FlashProManager FM;

    public bool TEST_ME = false;
    [SerializeField]
    private Vector2 CurrentMouse = new Vector2();
    [Header("FlashProTemplate")]
    public bool SingleLerp;
    public bool UseXLerpOnly;

    public Vector2 StartPos;
    public Vector2 MiddlePos;
    public Vector2 FinishPos;

    public float StartWidth;
    public float MiddleWidth;
    public float FinishWidth;

    public float StartAlpha;
    public float MiddleAlpha;
    public float FinishAlpha;

    public string myMessage1;
    public string myMessage2;

    public Color TextColor1;
    public Color TextColor2;

    public Tween Xtween1;
    public Tween Xtween2;
    public Tween Ytween1;
    public Tween Ytween2;

    public float AnimTime;
    public float MiddleTimeRatio;

    // Update is called once per frame
    void Update()
    {
        CurrentMouse.x = Input.mousePosition.x / Screen.width;
        CurrentMouse.y = Input.mousePosition.y / Screen.height;

        if (TEST_ME)
        {
            FlashProTemplate ret = new FlashProTemplate();
                ret.SingleLerp = SingleLerp;
                ret.UseXLerpOnly = UseXLerpOnly;
                ret.StartPos = StartPos;
                ret.MiddlePos = MiddlePos;
                ret.FinishPos = FinishPos;
                ret.StartWidth = StartWidth;
                ret.MiddleWidth = MiddleWidth;
                ret.FinishWidth = FinishWidth;
                ret.StartAlpha = StartAlpha;
                ret.MiddleAlpha = MiddleAlpha;
                ret.FinishAlpha = FinishAlpha;
                ret.myMessage1 = myMessage1;
                ret.myMessage2 = myMessage2;
                ret.TextColor1 = TextColor1;
                ret.TextColor2 = TextColor2;
                ret.Xtween1 = Xtween1;
                ret.Ytween1 = Ytween1;
                ret.Xtween2 = Xtween2;
                ret.Ytween2 = Ytween2;
                ret.AnimTime = AnimTime;
                ret.MiddleTimeRatio = MiddleTimeRatio;
            FM.CustomFlash(ret);
            TEST_ME = false;
        }
    }
}
