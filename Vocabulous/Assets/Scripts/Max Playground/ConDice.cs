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


    void Awake ()
    {
        gc = GC.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        tc.setID(ID);
        //tc.SetLetter(letter);
        tc.SetVisible(false);
        top = FaceTop.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

}
