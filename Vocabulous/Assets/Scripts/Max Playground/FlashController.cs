using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    public Vector2 StartPos = new Vector2(0.1f,0.1f);
    public Vector2 FinishPos = new Vector2(0.9f,0.9f);
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


    //

 //   var pos = Camera.main.WorldToViewportPoint(transform.position);
 //   pos.x = Mathf.Clamp(pos.x, 0.1, 0.9);
 //pos.y = Mathf.Clamp(pos.y, 0.1, 0.9);
 //transform.position = Camera.main.ViewportToWorldPoint(pos);

    // Start is called before the first frame update
    void Start()
    {

        rt.localPosition = Camera.main.WorldToViewportPoint(StartPos);

    //StartPos = new Vector3(-Screen.width, 0, 0);
    //FinishPos = new Vector3(Screen.width, 0, 0);
    //rt.transform.localPosition = StartPos;
        //rt.localScale = new Vector3(StartScale, StartScale, StartScale);
        SetValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimTime > 0)
        {
            //rt.transform.Translate(Velocity * Time.deltaTime);
            //float s = rt.localScale.x * ScaleMod * Time.deltaTime;
            //rt.localScale = new Vector3(s, s, s);
            //AnimTime -= Time.deltaTime;
        }
        else
        {
            //Destroy(transform.gameObject);
        }
    }

    private void SetValues ()
    {
        //Vector3 move = (FinishPos - StartPos) / AnimTime;
        //ScaleMod = (FinishScale - StartScale) / AnimTime;
    }

}
