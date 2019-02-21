using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveMyText : MonoBehaviour
{

    public RectTransform RT;
    public Vector2 StartPos;
    public Vector2 FinishPos;

    public float StartWidth;
    public float FinishWidth;

    private bool going;
    public float time;
    private float _time;
    private float WidthMod;
    private Vector2 _fin;
    private Vector2 _srt;
    private float _sw;
    private float _fw;


    // Start is called before the first frame update
    void Start()
    {
        _time = 0;     
        going = true;
        _fin = new Vector2(Screen.width * FinishPos.x, Screen.height * FinishPos.y);
        _srt = new Vector2(Screen.width * StartPos.x, Screen.height * StartPos.y);
        _sw = Screen.width * StartWidth;
        _fw = Screen.width * FinishWidth;
        WidthMod = _fw - _sw;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Pos2;
        if (going)
        {
            Pos2 = Vector2.Lerp(_srt, _fin, _time);
            RT.sizeDelta = new Vector2(_sw + (WidthMod * _time), 0);
        }
        else
        {
            Pos2 = Vector2.Lerp(_fin, _srt, _time);
            RT.sizeDelta = new Vector2(_fw - (WidthMod * _time), 0);
        }
        RT.SetPositionAndRotation(new Vector3(Pos2.x,Pos2.y,0), Quaternion.identity);

        _time += Time.deltaTime / time;
        if (_time > 1)
        {
            _time = 0;
            going = !going;
        }
    }
}
