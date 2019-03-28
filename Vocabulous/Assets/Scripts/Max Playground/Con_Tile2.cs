//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using UnityEngine;

public class Con_Tile2 : MonoBehaviour
{
    #region Member Declaration
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
    public bool isFreeWord;
    #endregion

    #region Unity API
    void Awake()
    {
        Text_Material = Text_Body.GetComponent<Renderer>().material;
        Body_Material = Tile_Body.GetComponent<Renderer>().material;
        gc = GC.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (myGrid != null && isFreeWord)
        {
            if (myGrid.legals.Contains(TC_front.ID))
            {
                Text_Material.color = gc.ColorLegal;
            }
            else if (myGrid.path.Contains(TC_front.ID))
            {
                Text_Material.color = gc.ColorSelected;
            }
            else if (!myGrid.highlights.Contains(TC_front.ID))
            {
                Text_Material.color = gc.ColorBase;
            }
            else
            {
                Text_Material.color = gc.ColorHighlight;
            }
            if (myGrid.bodyHighlights.Contains(TC_front.ID))
            {
                Body_Material.color = (Body_Material.color + gc.ColorBodyHighlight) / 2f;
            }
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    #endregion

    #region Public Methods
    // flip the tile to/from horizontal/vertical and forward/back facing
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
        vertical = Vertical;
        forward = Forward;
    }

    // rolls the tile (at specified speed) from front to back (or vice versa)
    public void Roll(float speed)
    {
        if (animating) // sanity check to prevent multiple calls
        {
            Debug.Log("Tile already Animating");
            return;
        }
        Rot_rate = speed;
        animating = true;
        StartCoroutine("myRoll");
    }

    IEnumerator myRoll()
    {
        float oRot = transform.localEulerAngles.z;
        float oY = transform.localPosition.y;
        float nRot = oRot - 180;
        forward = !forward;
        while (nRot < oRot)
        {
            oRot -= 180 / Rot_rate * Time.deltaTime;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, oRot);
            transform.localPosition = new Vector3(transform.localPosition.x, oY + (transform.localScale.x/2) * Mathf.Sin((oRot - nRot) * Mathf.Deg2Rad), transform.localPosition.z);
            yield return null;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, oY, transform.localPosition.z);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, nRot);
        animating = false;
    }

    // Color change methods
    public void ChangeTileColorAdditive(Color myColor)
    {
        Body_Material.color = (Body_Material.color + gc.ColorBodyHighlight) / 2f;
    }

    public void ChangeTileColor (Color color)
    {
        Body_Material.color = color;
    }

    public void ChangeTextColor (Color color)
    {
        Text_Material.color = color;
    }

    // set ID methods (so that's what is reported to the GC)
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

    // KILL method to be called if a controller wants "dumb" tile that does not report to the GC
    public void killOverlayTile()
    {
        Destroy(TC_front);
        Destroy(TC_back);
        Destroy(GetComponent("OverlayTile"));
    }
    #endregion
}
