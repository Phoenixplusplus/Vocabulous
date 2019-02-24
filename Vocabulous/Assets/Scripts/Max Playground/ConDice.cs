using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConDice : MonoBehaviour
{
    private GC gc;
    public string letter = "g";
    public int ID = -1; //default to a -1 UNLESS you want it reported
    public Tile_Controlller tc;
    public GameGrid myGrid;
    public GameObject FaceTop;
    private Material top;
    public GameObject Faceside1;
    public GameObject Faceside2;
    public GameObject DiceBody;
    private Material DiceMaterial;
    // currently bugged
    public bool shaking;
    private float shakeVar = 50.0f;
    private float shakeRange = 3.0f;


    void Awake ()
    {
        gc = GC.Instance;
        top = FaceTop.GetComponent<Renderer>().material;
        DiceMaterial = DiceBody.GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (tc != null)
        {
            tc.setID(ID);
            tc.SetVisible(false);
        }
        // reset shadow stuff
        Renderer MR;
        MR = FaceTop.GetComponent<Renderer>();
        MR.receiveShadows = false;
        MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        MR = Faceside1.GetComponent<Renderer>();
        MR.receiveShadows = false;
        MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        MR = Faceside2.GetComponent<Renderer>();
        MR.receiveShadows = false;
        MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        MR = DiceBody.GetComponent<Renderer>();
        MR.receiveShadows = false;
        MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

    }

    // Update is called once per frame
    void Update()
    {
        // are we shaking?
        if (shaking)
        {
            //transform.Rotate(Vector3.up * Mathf.Sin(shakeVar * Time.deltaTime) * shakeRange);
            // needs work 8-)

        }

        if (myGrid != null)
        {
            if (myGrid.legals.Contains(ID))
            {
                top.color = gc.ColorLegal;
            }
            else if (myGrid.path.Contains(ID))
            {
                top.color = gc.ColorSelected;
            }
            else if (!myGrid.highlights.Contains(ID))
            {
                top.color = gc.ColorBase;
            }
            else
            {
                top.color = gc.ColorHighlight;
            }
            if (myGrid.bodyHighlights.Contains(ID))
            {
                DiceMaterial.color = (DiceMaterial.color + gc.ColorBodyHighlight) / 2f;
            }
        }
    }

    public void ChangeDiceColor (Color myColor)
    {
        DiceMaterial.color = myColor;
    }

    public void ChangeDiceColorAdditive(Color myColor)
    {
        DiceMaterial.color = (DiceMaterial.color + gc.ColorBodyHighlight) / 2f;
    }

    public void Scale(float factor)
    {
        FaceTop.transform.localScale = FaceTop.transform.localScale * factor;
        Faceside1.transform.localScale = Faceside1.transform.localScale * factor;
        Faceside2.transform.localScale = Faceside2.transform.localScale * factor;
        DiceBody.transform.localScale = DiceBody.transform.localScale * factor;
    }

    public void killOverlayTile()
    {
        Destroy(tc.gameObject);
    }


}
