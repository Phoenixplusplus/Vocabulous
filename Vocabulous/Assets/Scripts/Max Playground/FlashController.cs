using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    public Vector2 StartPos = new Vector2(50,50);
    public Vector2 FinishPos = new Vector2(600, 600);
    private Vector3 Velocity;
    public float StartScale = 0.5f;
    public float FinishScale = 1.5f;
    private float ScaleMod;
    public float AnimTime = 10.0f;
    RectTransform rt;
    SpriteRenderer sr;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        sr = GetComponent<SpriteRenderer>();
    }


    // Start is called before the first frame update
    void Start()
    {
        rt.transform.localPosition = new Vector3(0, StartPos.y, StartPos.x);
        //rt.localScale = new Vector3(StartScale, StartScale, StartScale);
        SetValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimTime > 0)
        {
            rt.transform.Translate(Velocity * Time.deltaTime);
            float s = rt.localScale.x * ScaleMod * Time.deltaTime;
            //rt.localScale = new Vector3(s, s, s);
            AnimTime -= Time.deltaTime;
        }
        else
        {
            //Destroy(transform.gameObject);
        }
    }

    private void SetValues ()
    {
        Vector2 move = (FinishPos - StartPos) / AnimTime;
        Velocity = new Vector3(0,move.y,move.x);
        ScaleMod = (FinishScale - StartScale) / AnimTime;
    }

}
