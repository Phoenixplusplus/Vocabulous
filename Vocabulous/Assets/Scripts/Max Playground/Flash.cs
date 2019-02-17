using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlashTemplate
{
    public Vector2 StartPos;
    public Vector2 FinishPos;
    public float StartWidth;
    public float FinishWidth;
    public string myMessage;
    public Color TextColor;
    public float AnimTime;
}

public class Flash : MonoBehaviour
{
    public Vector2 StartPos;
    public Vector2 FinishPos;
    public float StartWidth;
    public float FinishWidth;
    public Text myText;
    public RectTransform myRect;
    public string myMessage;
    public Color TextColor;
    public float AnimTime;

    private float _time = 0;
    private float SX;
    private float SY;
    private float FX;
    private float FY;
    private float SW;
    private float SMod;

    public void configureAndGoGo (string text, Vector2 Start, Vector2 Finish, float SWidth, float FWidth, Color color,float ATime)
    {
        myMessage = text;
        StartPos = Start;
        FinishPos = Finish;
        StartWidth = SWidth;
        FinishWidth = FWidth;
        TextColor = color;
        AnimTime = ATime;
        SetMeUp();
    }

    public void ConfigureAndGoGo (FlashTemplate myTemplate)
    {
        myMessage = myTemplate.myMessage;
        StartPos = myTemplate.StartPos;
        FinishPos = myTemplate.FinishPos;
        StartWidth = myTemplate.StartWidth;
        FinishWidth = myTemplate.FinishWidth;
        TextColor = myTemplate.TextColor;
        AnimTime = myTemplate.AnimTime;
        SetMeUp();
    }

    public void ConfigureAndGoGo(FlashTemplate myTemplate, string message)
    {
        myMessage = message;
        StartPos = myTemplate.StartPos;
        FinishPos = myTemplate.FinishPos;
        StartWidth = myTemplate.StartWidth;
        FinishWidth = myTemplate.FinishWidth;
        TextColor = myTemplate.TextColor;
        AnimTime = myTemplate.AnimTime;
        SetMeUp();
    }

    void SetMeUp()
    {
        SX = Screen.width * StartPos.x;
        SY = Screen.height * StartPos.y;
        FX = Screen.width * FinishPos.x;
        FY = Screen.height * FinishPos.y;
        myText.text = myMessage;
        myText.color = TextColor;
        SW = Screen.width * StartWidth;
        SMod = Screen.width * (FinishWidth - StartWidth);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMeUp();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = Vector3.Lerp(new Vector3(SX, SY, 0), new Vector3(FX, FY, 0), _time);
        myRect.SetPositionAndRotation(Pos, Quaternion.identity);
        myRect.sizeDelta = new Vector2((SW + (SMod * _time)), myRect.sizeDelta.y);
        _time += Time.deltaTime / AnimTime;
        if (_time >= 1) Destroy(gameObject);
    }
}
