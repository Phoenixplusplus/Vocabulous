using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// thanks to http://www.gizma.com/easing/ (Feb 2019)
public enum Tween { LinearUp, LinearDown, ParametricUp, ParametricDown, QuadUp, QuadDown, QuinUp, QuinDown, SineUp, SineDown,BounceUp, BounceDown, SinePop, BouncePop};

public class FlashTemplate
{
    public bool SingleLerp;

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

    public FlashTemplate() // default constructor
    {
        SingleLerp = true;

        StartPos = new Vector2(0.5f,0.5f);
        MiddlePos = new Vector2(0.5f, 0.5f);
        FinishPos = new Vector2(0.5f, 0.5f);

        StartAlpha = 1f;
        MiddleAlpha = 1f;
        FinishAlpha = 1f;

        StartHeight = 0.05f;
        MiddleHeight = 0.05f;
        FinishHeight = 0.05f;

        myMessage1 = "Default Message 1";
        myMessage2 = "Default Message 2";

        TextColor1 = Color.red;
        TextColor2 = Color.green;

        tween1 = Tween.LinearUp;
        tween2 = Tween.LinearUp;

        AnimTime = 2f;
        MiddleTimeRatio = 1f;
    }

    public FlashTemplate Copy ()
    {
        FlashTemplate Ret = new FlashTemplate();
        Ret.SingleLerp = SingleLerp;
        Ret.StartPos = StartPos;
        Ret.MiddlePos = MiddlePos;
        Ret.FinishPos = FinishPos;
        Ret.StartAlpha = StartAlpha;
        Ret.MiddleAlpha = MiddleAlpha;
        Ret.FinishAlpha = FinishAlpha;
        Ret.StartHeight = StartHeight;
        Ret.MiddleHeight = MiddleHeight;
        Ret.FinishHeight = FinishHeight;
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

public class Flash : MonoBehaviour
{
    public RectTransform myRect;
    public Text myText;
    public FlashTemplate FT;

    private bool firstLerp = true;
    private float _time = 0;
    private float X1;
    private float Y1;
    private float X2;
    private float Y2;
    private bool moving;
    private float HS;
    private float HMod;
    private bool scaling;
    private float AS;
    private float AMod;
    private bool Alphachange;
    private float TimeMod;
    private Color CurrCol;
    private Tween currTween;

    public void ConfigureAndGoGo (FlashTemplate myTemplate)
    {
        FT = myTemplate;
        firstLerp = true;
        SetMeUp();
    }

    public void ConfigureAndGoGo(FlashTemplate myTemplate, string message1)
    {
        FT = myTemplate;
        FT.myMessage1 = message1;
        firstLerp = true;
        SetMeUp();
    }
    public void ConfigureAndGoGo(FlashTemplate myTemplate, string message1, string message2)
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

                myText.text = FT.myMessage1;
                if (FT.StartHeight < 1)
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
                TimeMod = 1f / (FT.AnimTime * FT.MiddleTimeRatio);


            }
            else // single lerp
            {
                X1 = Screen.width * FT.StartPos.x;
                Y1 = Screen.height * FT.StartPos.y;
                X2 = Screen.width * FT.FinishPos.x;
                Y2 = Screen.height * FT.FinishPos.y;
                myText.text = FT.myMessage1;
                if (FT.StartHeight < 1)
                {
                    HS = Screen.height * FT.StartHeight;
                    HMod = Screen.height * (FT.FinishHeight - FT.StartHeight);
                }
                else
                {
                    HS = FT.StartHeight;
                    HMod = FT.FinishHeight - FT.StartHeight;
                }
                AS = FT.StartAlpha;
                AMod = FT.FinishAlpha - FT.StartAlpha;
                _time = 0;
                TimeMod = 1 / FT.AnimTime;
            }
            currTween = FT.tween1;
        }
        else // Only get here if NOT first Lerp .. so second by default
        {
            CurrCol = FT.TextColor2;
            currTween = FT.tween2;
            myText.text = FT.myMessage2;
            X1 = Screen.width * FT.MiddlePos.x;
            Y1 = Screen.height * FT.MiddlePos.y;
            X2 = Screen.width * FT.FinishPos.x;
            Y2 = Screen.height * FT.FinishPos.y;
            if (FT.MiddleHeight < 1)
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
            TimeMod = 1f / (FT.AnimTime * (1f - FT.MiddleTimeRatio));

            
        }
        moving = (X1 != X2 || Y1 != Y2);
        scaling = (HMod != 0);
        Alphachange = (AMod != 0);

        InitialSet();

    }



    // Update is called once per frame
    void Update()
    {
        setTo(_time, currTween);
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
        myText.fontSize = (int)HS;
        Color nCol = CurrCol;
        nCol.a = AS;
        myText.color = nCol;
    }

    void setTo(float t, Tween tween)
    {
        float tmod = DoTween(tween,t);
        if (moving)
        {
            Vector3 Pos = Vector3.Lerp(new Vector3(X1, Y1, 0), new Vector3(X2, Y2, 0), tmod);
            myRect.SetPositionAndRotation(Pos, Quaternion.identity);
        }
        if (scaling)
        {
            myText.fontSize = (int)(HS + (HMod * tmod));
        }
        if (Alphachange)
        {
            Color nCol = myText.color;
            nCol.a = (AS + (AMod * tmod));
            myText.color = nCol;
        }
    }

    private float DoTween (Tween tween,float t)
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
