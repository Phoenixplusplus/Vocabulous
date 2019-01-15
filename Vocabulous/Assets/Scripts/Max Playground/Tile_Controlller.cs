using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Controlller : MonoBehaviour
{
    public Quad_report InnerQuad;
    public Quad_report OuterQuad;
    public GameObject letter;
    public Color BaseColor;
    public Color LegalColor;
    public Color SelectedColor;
    private TextMesh txt;
    public int ID = -1;
    public GameGrid myGrid = null;

    public void SetLetter (string value)
    {
        txt.text = value;
    }

    public void setID (int value)
    {
        ID = value;
        InnerQuad.ID = value;
    }

    public void SetVisible(bool value)
    {
        InnerQuad.setVisible(value);
        OuterQuad.setVisible(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        txt = letter.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myGrid != null)
        {
            SetLetter(myGrid.bins[ID]);
            if (myGrid.legals.Contains(ID))
            {
                txt.color = LegalColor;
            }
            else if (myGrid.path.Contains(ID))
            {
                txt.color = SelectedColor;
            }
            else
            {
                txt.color = BaseColor;
            }
        }
    }
}
