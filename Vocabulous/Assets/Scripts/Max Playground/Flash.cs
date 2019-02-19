using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// thanks to http://www.gizma.com/easing/ (Feb 2019)
public enum Tween { LinearUp, LinearDown, ParametricUp, ParametricDown, QuadUp, QuadDown, QuinUp, QuinDown, SineUp, SineDown,BounceUp, BounceDown, SinePop};

public class FlashTemplate
{
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
}

public class Flash : MonoBehaviour
{
    public RectTransform myRect;
    public Text myText;
    public FlashTemplate FT;

    private bool doubleLerp;
    private bool firstLerp = true;
    private float _time = 0;
    private float X1;
    private float Y1;
    private float X2;
    private float Y2;
    private float HS;
    private float HMod;
    private float AS;
    private float AMod;
    private float TimeMod;

    public void ConfigureAndGoGo (FlashTemplate myTemplate)
    {
        FT = myTemplate;
        firstLerp = true;
        SetMeUp();
    }

    public void ConfigureAndGoGo(FlashTemplate myTemplate, string message1)
    {
        FT = myTemplate;
        firstLerp = true;
        SetMeUp();
        FT.myMessage1 = message1;
    }
    public void ConfigureAndGoGo(FlashTemplate myTemplate, string message1, string message2)
    {
        FT = myTemplate;
        firstLerp = true;
        SetMeUp();
        FT.myMessage1 = message1;
        FT.myMessage2 = message2;
    }

    void SetMeUp()
    {
        if (FT.MiddlePos != null)
        {
            doubleLerp = true;
        }
        if (firstLerp)
        {
            if (doubleLerp)
            {
                X1 = Screen.width * FT.StartPos.x;
                Y1 = Screen.height * FT.StartPos.y;
                X2 = Screen.width * FT.MiddlePos.x;
                Y2 = Screen.height * FT.MiddlePos.y;
                myText.text = FT.myMessage1;
                if (FT.StartHeight < 0)
                {
                    HS = Screen.height * FT.StartHeight;
                    HMod = Screen.height * (FT.MiddleHeight - FT.StartHeight);
                }
                else
                {
                    HS = FT.StartHeight;
                    HMod = FT.MiddleHeight - FT.StartHeight;
                }
                AS = FT.StartAlpha;
                AMod = FT.MiddleAlpha - FT.StartAlpha;
                _time = 0;
                TimeMod = FT.AnimTime * FT.MiddleTimeRatio;
            }
            else
            {
                X1 = Screen.width * FT.MiddlePos.x;
                Y1 = Screen.height * FT.MiddlePos.y;
                X2 = Screen.width * FT.FinishPos.x;
                Y2 = Screen.height * FT.FinishPos.y;
                myText.text = FT.myMessage2;
                if (FT.MiddleHeight < 0)
                {
                    HS = Screen.height * FT.MiddleHeight;
                    HMod = Screen.height * (FT.FinishHeight - FT.MiddleHeight);
                }
                else
                {
                    HS = FT.MiddleHeight;
                    HMod = FT.FinishHeight - FT.MiddleHeight;
                }
                AS = FT.MiddleAlpha;
                AMod = FT.FinishAlpha - FT.MiddleAlpha;
                _time = 0;
                TimeMod = FT.AnimTime * (1f - FT.MiddleTimeRatio);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
    //    Vector3 Pos = Vector3.Lerp(new Vector3(SX, SY, 0), new Vector3(FX, FY, 0), _time);
    //    myRect.SetPositionAndRotation(Pos, Quaternion.identity);
    //    myRect.sizeDelta = new Vector2((SW + (SMod * _time)), myRect.sizeDelta.y);
    ////    _time += Time.deltaTime / AnimTime;
    //    if (_time >= 1) Destroy(gameObject);
    }

    // TWEENING FUNCTIONS
    private float LinearUp(float t)
    {
        return Sanitise(t);
    }
    private float LinearDown(float t)
    {
        t = Sanitise(t);
        return 1f - t;
    }

    private float ParametricUp(float t)
    {
        t = Sanitise(t);
        float sqr = t * t;
        return sqr / (2.0f * (sqr - t) + 1.0f);
    }
    private float ParametricDown(float t)
    {
        t = Sanitise(t);
        return ParametricUp(1f - t);
    }

    private float QuadUp(float t)
    {
        t = Sanitise(t);
        return t * t;
    }
    private float QuadDown(float t)
    {
        t = Sanitise(t);
        return QuadUp(1f - t); ;
    }

    private float QuinUp(float t)
    {
        t = Sanitise(t);
        return t * t * t * t * t;
    }
    private float QuinDown(float t)
    {
        t = Sanitise(t);
        return QuinUp(1f - t); ;
    }

    private float SineUp (float t)
    {
        t = Sanitise(t);
        return Mathf.Sin(Mathf.PI / 2 * t);
    }
    private float SineDown (float t)
    {
        t = Sanitise(t);
        return SineUp(1f - t);
    }

    private float SinePop (float t)
    {
        t = Sanitise(t);
        return Mathf.Sin(Mathf.PI * t);
    }

    //// Thanks to https://github.com/d3/d3-ease/blob/master/src/bounce.js#L12 (24 Nov 2018)
    float BounceUp(float t)
    {
        t = Sanitise(t);
        float b1 = 4f / 11f,
            b2 = 6f / 11f,
            b3 = 8f / 11f,
            b4 = 3f / 4f,
            b5 = 9f / 11f,
            b6 = 10f / 11f,
            b7 = 15f / 16f,
            b8 = 21f / 22f,
            b9 = 63f / 64f,
            b0 = 1f / b1 / b1;
        if (t < b1) return b0 * t * t;
        else if (t < b3)
        {
            t = t - b2;
            return b0 * t * t + b4;
        }
        else if (t < b6)
        {
            t = t - b5;
            return b0 * t * t + b7;
        }
        else
        {
            t = t - b8;
            return b0 * t * t + b9;
        }
    }
    float BounceDown(float t)
    {
        t = Sanitise(t);
        return BounceUp(1f - t);
    }
    float BouncePop(float t)
    {
        t = Sanitise(t);
        if (t <=0.5f)
        {
            return BounceUp(t * 2f);
        }
        else
        {
            return BounceDown((t - 0.5f)*2f);
        }
    }

    private float Sanitise(float t)
    {
        if (t < 0.0f) return 0.0f;
        else if (t > 1.0f) return 1.0f;
        else return t;
    }

}
