using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeKey : MonoBehaviour
{
    private ConTypeWriter myCon;
    private Tile_Controlller myTile;
    private ConDice myDiceCon;
    public string myKey;
    public int myHoverID;
    private bool animating = false;
    private Vector3 OPosition;

    public void press(Transform trans)
    {
        if (!animating) StartCoroutine("myPress",trans);
    }

    IEnumerator myPress (Transform trans)
    {
        animating = true;
        bool dropping = true;
        while (animating)
        {
            float rate = transform.localScale.y * 0.9f;
            float bottom = OPosition.y - (transform.localScale.y * 0.2f);
            if (dropping && transform.localPosition.y > bottom)
            {
                transform.Translate(new Vector3(0, -rate * Time.deltaTime, 0));
                if (transform.localPosition.y <= bottom) dropping = false;
            }
            if (!dropping && transform.localPosition.y < OPosition.y)
            {
                transform.Translate(new Vector3(0, rate * Time.deltaTime, 0));
                if (transform.localPosition.y <= OPosition.y)
                {
                    transform.localPosition = OPosition;
                    animating = false;
                }
            }
        yield return null;
        }
    }



    void Awake()
    {
        myCon = GetComponentInParent<ConTypeWriter>();
        myDiceCon = GetComponentInChildren<ConDice>();
        myDiceCon.ID = myHoverID;
        myTile = GetComponentInChildren<Tile_Controlller>();        
    }


    // Start is called before the first frame update
    void Start()
    {
        OPosition = transform.localPosition;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
