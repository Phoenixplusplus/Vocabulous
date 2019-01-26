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

    public void press()
    {
        if (!animating) StartCoroutine("myPress");
    }

    IEnumerator myPress ()
    {
        animating = true;
        bool dropping = true;
        while (animating)
        {
            float rate = transform.localScale.y * 0.9f;
            float bottom = OPosition.y - (transform.localScale.y * 0.2f);
            if (dropping && transform.position.y > bottom)
            {
                transform.Translate(new Vector3(0, -rate * Time.deltaTime, 0));
                if (transform.position.y <= bottom) dropping = false;
            }
            if (!dropping && transform.position.y < OPosition.y)
            {
                transform.Translate(new Vector3(0, rate * Time.deltaTime, 0));
                if (transform.position.y <= OPosition.y)
                {
                    transform.position = OPosition;
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
        //myTile.setID(myHoverID);
        OPosition = transform.position;
    }


    // Start is called before the first frame update
    void Start()
    {
        //press();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
