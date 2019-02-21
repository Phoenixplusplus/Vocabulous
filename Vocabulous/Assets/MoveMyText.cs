using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMyText : MonoBehaviour
{

    public RectTransform RT;
    public Vector2 StartPos;
    public Vector2 FinishPos;
    public Vector2 StartSize;
    public float FinishScale;
    private bool going;
    public float time;
    private float _time;
    private float ScaleMod;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = RT.position;
        StartSize = RT.sizeDelta;
        _time = 0;
        ScaleMod = FinishScale - 1;
        going = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Pos2;
        if (going)
        {
            Pos2 = Vector2.Lerp(StartPos, FinishPos, _time);
            RT.sizeDelta = StartSize * (1 + (ScaleMod * _time));
        }
        else
        {
            Pos2 = Vector2.Lerp(FinishPos, StartPos, _time);
            RT.sizeDelta = StartSize * (1 + (ScaleMod * (1 - _time)));
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
