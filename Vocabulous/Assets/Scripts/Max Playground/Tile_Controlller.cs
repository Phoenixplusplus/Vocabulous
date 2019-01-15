using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Controlller : MonoBehaviour, IisOverlayTile
{
    public GameObject quad;
    public GameObject letter;
    public Color BaseColor;
    public Color LegalColor;
    public Color SelectedColor;
    private TextMesh txt;
    public int ID = -1;
    private string rand = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public GameGrid myGrid = null;

    public void SetLetter (string value)
    {
        txt.text = value;
    }

    // Interface requirement
    public int getID()
    {
        return ID;
    }

    // Start is called before the first frame update
    void Start()
    {
        txt = letter.GetComponent<TextMesh>();
        //SetLetter(""+rand[Random.Range(0, 26)]);
    }

    // Update is called once per frame
    void Update()
    {
        if (myGrid != null)
        {
            SetLetter(myGrid.bins[this.ID]);
            //if (myGrid.legals.Contains(this.ID))
            //{
            //    txt.color = LegalColor;
            //}
           // else if (myGrid.path.Contains(this.ID))
           // {
            //    txt.color = SelectedColor;
           // }
        }
    }
}
