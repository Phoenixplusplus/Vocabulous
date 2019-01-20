using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GC : MonoBehaviour
{
    [Header("This won't stay empty for long")]
    public int forKickOff = 0;
    public bool HoverChange = false; // quick "has hover changed bool" for frame to frame checking


    // a lot of messing about ... but ... https://answers.unity.com/questions/915032/make-a-public-variable-with-a-private-setter-appea.html
    // changed in Update() to reflect code of what mouse is over
    public int NewHoverOver { get { return _NewHoverOver; } private set { _NewHoverOver = value; } }
    [SerializeField]
    private int _NewHoverOver = -1;
    // reflects the HoverOvervalue from the PREVIOUS frame (for frame by frame comparison)
    public int OldHoverOver { get { return _OldHoverOver; } private set { _OldHoverOver = value; } }
    [SerializeField]
    private int _OldHoverOver = -1;
    public MaxTrie maxTrie;
    public TrieTest phoenixTrie;


    // --------------------//
    // establish Singelton //
    // ------------------- //
    public static GC Instance
    {
        get
        {
            return instance;
        }
    }
    private static GC instance = null;
    void Awake()
    {
        if (instance)
        {
            Debug.Log("Already a GameController - going to die now .....");
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //---------------------------//
    // Finished Singelton set up //
    // --------------------------//


    // Start is called before the first frame update
    void Start()
    {
        NewHoverOver = -1;
        OldHoverOver = -1;
    }

    // Update is called once per frame
    void Update()
    {
        // sets HoverOver values to the returned value from any IisOverlayTile class (if none, then -1)
        CheckHoverOver(); 
    }

    void CheckHoverOver()
    {
        OldHoverOver = NewHoverOver;
        NewHoverOver = -1;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider != null)
            {
                IisOverlayTile tile = hit.collider.GetComponent<IisOverlayTile>();
                if (tile != null)
                {
                    NewHoverOver = tile.getID();
                }
            }
        }
        if (OldHoverOver == NewHoverOver) HoverChange = false;
        else { HoverChange = true; }
    }


}
