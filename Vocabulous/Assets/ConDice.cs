using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConDice : MonoBehaviour
{
    private GC gc;
    public string letter;
    public int ID = -1; //default to a -1 UNLESS you want it reported
    public Tile_Controlller tc;
    public GameGrid myGrid;
    public Renderer FaceTop;
    public Renderer Faceside1;
    public Renderer Faceside2;
    public Renderer DiceBody;


    void Awake ()
    {
        gc = GC.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        tc.setID(ID);
        tc.SetLetter(letter);
        tc.SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
