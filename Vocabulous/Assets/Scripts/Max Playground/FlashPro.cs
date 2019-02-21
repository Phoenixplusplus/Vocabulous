using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class FlashPro : MonoBehaviour
{
    public RectTransform myRect;
    public TextMeshProUGUI myText;
    public FlashProTemplate FT;

    private bool firstLerp = true;
    private float _time = 0;
    private float X1;
    private float Y1;
    private float X2;
    private float Y2;
    private float Xmod;
    private float YMod;
    private bool moving;
    private float WS;
    private float WMod;
    private bool scaling;
    private float AS;
    private float AMod;
    private bool Alphachange;
    private float TimeMod;
    private Color CurrCol;
    private Tween currXTween;
    private Tween currYTween;

    public void ConfigureAndGoGo(FlashProTemplate myTemplate)
    {
        FT = myTemplate;
        firstLerp = true;
        SetMeUp();
    }

    public void ConfigureAndGoGo(FlashProTemplate myTemplate, string message1)
    {
        FT = myTemplate;
        FT.myMessage1 = message1;
        firstLerp = true;
        SetMeUp();
    }
    public void ConfigureAndGoGo(FlashProTemplate myTemplate, string message1, string message2)
    {
        FT = myTemplate;
        FT.myMessage1 = message1;
        FT.myMessage2 = message2;
        firstLerp = true;
        SetMeUp();
    }

    void SetMeUp()
    {
        if (firstLerp)
        {
            CurrCol = FT.TextColor1;
            if (!FT.SingleLerp) // A Double Lerp, middle values should be populated
            {
                X1 = Screen.width * FT.StartPos.x;
                Y1 = Screen.height * FT.StartPos.y;
                X2 = Screen.width * FT.MiddlePos.x;
                Y2 = Screen.height * FT.MiddlePos.y;
                Xmod = X2 - X1;
                YMod = Y2 - Y1;
                myText.text = FT.myMessage1;
                if (FT.StartWidth < 1)
                {
                    WS = Screen.width * FT.StartWidth;
                    WMod = Screen.width * (FT.MiddleWidth - FT.StartWidth);
                }
                else
                {
                    WS = FT.StartWidth;
                    WMod = FT.MiddleWidth - FT.StartWidth;
                }
                AS = FT.StartAlpha;
                AMod = FT.MiddleAlpha - FT.StartAlpha;
                _time = 0;
                TimeMod = 1f / (FT.AnimTime * FT.MiddleTimeRatio);


            }
            else // single lerp
            {
                X1 = Screen.width * FT.StartPos.x;
                Y1 = Screen.height * FT.StartPos.y;
                X2 = Screen.width * FT.FinishPos.x;
                Y2 = Screen.height * FT.FinishPos.y;
                Xmod = X2 - X1;
                YMod = Y2 - Y1;
                myText.text = FT.myMessage1;
                if (FT.StartWidth < 1)
                {
                    WS = Screen.width * FT.StartWidth;
                    WMod = Screen.width * (FT.FinishWidth - FT.StartWidth);
                }
                else
                {
                    WS = FT.StartWidth;
                    WMod = FT.FinishWidth - FT.StartWidth;
                }
                AS = FT.StartAlpha;
                AMod = FT.FinishAlpha - FT.StartAlpha;
                _time = 0;
                TimeMod = 1 / FT.AnimTime;
            }
            currXTween = FT.Xtween1;
            currYTween = FT.Ytween1;
        }
        else // Only get here if NOT first Lerp .. so second by default
        {
            CurrCol = FT.TextColor2;
            currXTween = FT.Xtween2;
            currYTween = FT.Ytween2;
            myText.text = FT.myMessage2;
            X1 = Screen.width * FT.MiddlePos.x;
            Y1 = Screen.height * FT.MiddlePos.y;
            X2 = Screen.width * FT.FinishPos.x;
            Y2 = Screen.height * FT.FinishPos.y;
            Xmod = X2 - X1;
            YMod = Y2 - Y1;
            if (FT.MiddleWidth < 1)
            {
                WS = Screen.width * FT.MiddleWidth;
                WMod = Screen.width * (FT.FinishWidth - FT.MiddleWidth);
            }
            else
            {
                WS = FT.MiddleWidth;
                WMod = FT.FinishWidth - FT.MiddleWidth;
            }
            AS = FT.MiddleAlpha;
            AMod = FT.FinishAlpha - FT.MiddleAlpha;
            _time = 0;
            TimeMod = 1f / (FT.AnimTime * (1f - FT.MiddleTimeRatio));


        }
        moving = (X1 != X2 || Y1 != Y2);
        scaling = (WMod != 0);
        Alphachange = (AMod != 0);

        InitialSet();

    }



    // Update is called once per frame
    void Update()
    {
        setTo(_time, currXTween, currYTween);
        _time += Time.deltaTime * TimeMod;
        if (_time >= 1)
        {
            if (firstLerp && !FT.SingleLerp)
            {
                firstLerp = false;
                SetMeUp();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void InitialSet()
    {
        myRect.SetPositionAndRotation(new Vector3(X1, Y1, 0), Quaternion.identity);
        myRect.sizeDelta = new Vector2(WS, 0);
        Color nCol = CurrCol;
        nCol.a = AS;
        myText.color = nCol;
    }

    void setTo(float t, Tween Xtween, Tween Ytween)
    {
        float tmod = DoTween(Xtween, t);
        float dx = DoTween(Xtween, t);
        float dy = DoTween(Ytween, t);
        if (moving)
        {
            if (FT.UseXLerpOnly)
            {
                Vector3 Pos = Vector3.Lerp(new Vector3(X1, Y1, 0), new Vector3(X2, Y2, 0), tmod);
                myRect.SetPositionAndRotation(Pos, Quaternion.identity);
            }
            else
            {
                myRect.SetPositionAndRotation(new Vector3(X1 + (Xmod * dx), Y1 + (YMod * dy), 0), Quaternion.identity);
            }
        }
        if (scaling)
        {
            myRect.sizeDelta = new Vector2(WS + (WMod * tmod), 0);
        }
        if (Alphachange)
        {
            Color nCol = myText.color;
            nCol.a = (AS + (AMod * tmod));
            myText.color = nCol;
        }
    }

    private float DoTween(Tween tween, float t)
    {
        // LinearUp, LinearDown, ParametricUp, ParametricDown, QuadUp, QuadDown, QuinUp, QuinDown, SineUp, SineDown,BounceUp, BounceDown, SinePop
        switch (tween)
        {
            case Tween.LinearUp: return LinearUp(t);
            case Tween.LinearDown: return LinearDown(t);
            case Tween.ParametricUp: return ParametricUp(t);
            case Tween.ParametricDown: return ParametricDown(t);
            case Tween.QuadUp: return QuadUp(t);
            case Tween.QuadDown: return QuadDown(t);
            case Tween.QuinUp: return QuinUp(t);
            case Tween.QuinDown: return QuinDown(t);
            case Tween.SineUp: return SineUp(t);
            case Tween.SineDown: return SineDown(t);
            case Tween.SinePop: return SinePop(t);
            case Tween.BounceUp: return BounceUp(t);
            case Tween.BounceDown: return BounceDown(t);
            case Tween.BouncePop: return BouncePop(t);
            default: return t;
        }
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

    private float SineUp(float t)
    {
        t = Sanitise(t);
        return Mathf.Sin(Mathf.PI / 2 * t);
    }
    private float SineDown(float t)
    {
        t = Sanitise(t);
        return SineUp(1f - t);
    }

    private float SinePop(float t)
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
        if (t <= 0.5f)
        {
            return BounceUp(t * 2f);
        }
        else
        {
            return BounceDown((t - 0.5f) * 2f);
        }
    }

    private float Sanitise(float t)
    {
        if (t < 0.0f) return 0.0f;
        else if (t > 1.0f) return 1.0f;
        else return t;
    }

}
