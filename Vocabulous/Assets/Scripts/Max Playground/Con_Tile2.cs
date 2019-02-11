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
        // Roll(1f);
        //SetID(54, 77);
    }

    // Update is called once per frame
    void Update()
    {
        // TESTING
        //if (oldforw != forward || oldVert != vertical)
        //{
        //    FlipTo(vertical, forward);
        //    oldforw = forward;
        //    oldVert = vertical;
        //}
    }

    void OnDestroy()
    {
        StopAllCoroutines();
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

    public void Roll(float speed)
    {
        if (animating)
        {
            Debug.Log("Tile already Animating");
            return;
        }
        // work in progress ....
        Rot_rate = speed;
        animating = true;
        StartCoroutine("myRoll");
    }

    IEnumerator myRoll()
    {
        float oRot = transform.localEulerAngles.z;
        float oY = transform.localPosition.y;
        float nRot = oRot - 180;
        while (nRot < oRot)
        {
            oRot -= 180 / Rot_rate * Time.deltaTime;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, oRot);
            transform.localPosition = new Vector3(transform.localPosition.x, oY + (transform.localScale.x/2) * Mathf.Sin((oRot - nRot) * Mathf.Deg2Rad), transform.localPosition.z);
            yield return null;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, oY, transform.localPosition.z);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, nRot);
        vertical = !vertical;
        animating = false;

    }

    public void ChangeTileColor (Color color)
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

    public void SetFullFaceID (int frontID, int backID)
    {
        TC_front.SetBothID(frontID);
        TC_back.SetBothID(backID);
    }


}
