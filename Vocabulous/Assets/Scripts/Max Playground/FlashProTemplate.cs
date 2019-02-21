using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashProTemplate
{
    public bool SingleLerp;

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

    public Tween tween1;
    public Tween tween2;

    public float AnimTime;
    public float MiddleTimeRatio;

    public FlashProTemplate() // default constructor
    {
        SingleLerp = true;

        StartPos = new Vector2(0.5f, 0.5f);
        MiddlePos = new Vector2(0.5f, 0.5f);
        FinishPos = new Vector2(0.5f, 0.5f);

        StartWidth = 0.05f;
        MiddleWidth = 0.05f;
        FinishWidth = 0.05f;

        StartAlpha = 1f;
        MiddleAlpha = 1f;
        FinishAlpha = 1f;

        myMessage1 = "Default Message 1";
        myMessage2 = "Default Message 2";

        TextColor1 = Color.red;
        TextColor2 = Color.green;

        tween1 = Tween.LinearUp;
        tween2 = Tween.LinearUp;

        AnimTime = 2f;
        MiddleTimeRatio = 1f;
    }

    public FlashProTemplate Copy()
    {
        FlashProTemplate Ret = new FlashProTemplate();
            Ret.SingleLerp = SingleLerp;
            Ret.StartPos = StartPos;
            Ret.MiddlePos = MiddlePos;
            Ret.FinishPos = FinishPos;
            Ret.StartWidth = StartWidth;
            Ret.MiddleWidth = MiddleWidth;
            Ret.FinishWidth = FinishWidth;
            Ret.StartAlpha = StartAlpha;
            Ret.MiddleAlpha = MiddleAlpha;
            Ret.FinishAlpha = FinishAlpha;
            Ret.myMessage1 = myMessage1;
            Ret.myMessage2 = myMessage2;
            Ret.TextColor1 = TextColor1;
            Ret.TextColor2 = TextColor2;
            Ret.tween1 = tween1;
            Ret.tween2 = tween2;
            Ret.AnimTime = AnimTime;
            Ret.MiddleTimeRatio = MiddleTimeRatio;
        return Ret;
    }


}
