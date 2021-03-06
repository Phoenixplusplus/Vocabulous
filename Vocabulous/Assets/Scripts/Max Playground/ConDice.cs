﻿//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using UnityEngine;

// Controller for dice objects
public class ConDice : MonoBehaviour
{
    #region Members
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
    #endregion


    #region Unity API
    void Awake () // aka ... get and stash references
    {
        gc = GC.Instance;
        top = FaceTop.GetComponent<Renderer>().material;
        DiceMaterial = DiceBody.GetComponent<Renderer>().material;
    }

    void Start()
    {
        if (tc != null)
        {
            tc.setID(ID);
            tc.SetVisible(false);
        }
        // reset shadow stuff .. shortcut for Graphics optimisation
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

    void Update()
    {
        // check GameGrid (if specified) and change colour as required if (a) is a legal next move of (b) is currently on the "path"
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
    #endregion

    #region Public Manipulation methods
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

    // Call to create a "dumb" dice which does not need to report to the GC
    // Also ... DO THIS TO OPTIMISE if you can
    public void killOverlayTile()
    {
        Destroy(tc.gameObject);
    }
    #endregion

}
