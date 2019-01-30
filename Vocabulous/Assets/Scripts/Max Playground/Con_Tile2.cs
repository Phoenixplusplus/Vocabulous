using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Con_Tile2 : MonoBehaviour
{
    public Tile_Controlller TC_front;
    public Tile_Controlller TC_back;
    public int ID_front = -1;
    public int ID_Back = -1;
    public string Value_front;
    public string Value_back;
    public GameObject Text_Body;
    public GameObject Tile_Body;
    public GameObject Internal; // holds the guts of the tile, rotate this to change orientation
    public Collider collider;
    private Material Text_Material;
    private Material Body_Material;
    public GameGrid myGrid; // should we need it sometime
    private GC gc;
    public bool vertical = true;
    private bool oldVert = true;
    public bool forward = true;
    private bool oldforw = true;
    public float Rot_rate = 4.0f;
    public bool animating = false;
    private Vector3 VF = new Vector3(0, 0, 0);
    private Vector3 VB = new Vector3(0, -180, 0);
    private Vector3 HF = new Vector3(90, 0, 0);
    private Vector3 HB = new Vector3(-90, -180, 0);

    void Awake()
    {
        Text_Material = Text_Body.GetComponent<Renderer>().material;
        Body_Material = Tile_Body.GetComponent<Renderer>().material;
        gc = GC.Instance;
    }




    // Start is called before the first frame update
    void Start()
    {
        // TESTING
        SetID(54, 77);
    }

    // Update is called once per frame
    void Update()
    {
        // TESTING
        if (oldforw != forward || oldVert != vertical)
        {
            FlipTo(vertical, forward);
            oldforw = forward;
            oldVert = vertical;
        }
    }

    public void FlipTo(bool Vertical, bool Forward)
    {
        Vector3 dir = new Vector3();
        if (Vertical)
        {
            if (Forward) dir = VF;
            else { dir = VB; }
        }
        else
        {
            if (Forward) dir = HF;
            else { dir = HB; }
        }
        Internal.transform.localEulerAngles = dir;
    }

    public void RollTo(bool Vertical, bool Forward)
    {
        // work in progress ....
    }

    public void ChangeDiceColor (Color color)
    {
        Body_Material.color = color;
    }

    public void ChangeTextColor (Color color)
    {
        Text_Material.color = color;
    }

    public void SetID(int frontID, int backID)
    {
        TC_front.setID(frontID);
        TC_back.setID(backID);
    }


}
