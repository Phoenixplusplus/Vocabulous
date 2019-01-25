using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GC : MonoBehaviour
{
    #region Variable Declaration
    [Header("HoverOver -1 = nowt, 9999 = edgelike, 0-xxx = grid location")]
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

    [Header("The Trie's")]
    public MaxTrie maxTrie;
    public TrieTest phoenixTrie;

    [Header("Player Manager")]
    [SerializeField]
    private string PlayerName;
    [SerializeField]
    private int WorddiceLength;
    public PlayerStats player;
    public PlayerManager playerManager = new PlayerManager();

    [Header("Assets")]
    public OurAssets assets;

    [Header("SoundManager(sm)")]
    public SoundMan sm;

    [Header("Game Positions (for positional tweaking)")] // can't have a transform (more's the pity)
    public Vector3 PosWordSearch = new Vector3();
    public Vector3 PosTranWordDice = new Vector3();
    public Vector3 PosTranAnagram = new Vector3();
    public Vector3 PosTranWordrop = new Vector3();
    [Header("Game Rotations (for rotational tweaking)")]
    public Vector3 RotWordSearch = new Vector3();
    public Vector3 RotTranWordDice = new Vector3();
    public Vector3 RotTranAnagram = new Vector3();
    public Vector3 RotTranWordrop = new Vector3();
    [Header("Game Scales (for scale tweaking)")]
    public Vector3 ScaleWordSearch = new Vector3();
    public Vector3 ScaleTranWordDice = new Vector3();
    public Vector3 ScaleTranAnagram = new Vector3();
    public Vector3 ScaleTranWordrop = new Vector3();
    [Header("Default Dice/face colours")]
    public Color ColorBase = new Color();
    public Color ColorSelected = new Color();
    public Color ColorHighlight = new Color();
    public Color ColorLegal = new Color();

    [Header("THE GAME STATE")]
    [SerializeField]
    private int GameState = 0;
    //... needs to be agreed ... maybe 
    // 0 = initialising/loading
    // 1 = at table (choosing)
    // 2 = transition to game area
    // 31 =  Playing WordSearch
    // 32 = Playing WordDice
    // 33 = Playing Anagram
    // 34 = Playing WordDrop
    // 35 = <<add new game here>>
    // 4 = Menu's open
    // 5 = transitioning from a game to 1 again
    // 9 = Quitting

    [Header("The GAME OBJECTS")]
    public ConWordDice WordDice;

     #endregion


    #region Set Singelton
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
        // ESSENTIAL IF YOU ARE EDDITING PlayerStats stuff
        playerManager.ResetToDefault();
        player = playerManager.LoadPlayer();
        // Do stuff for a new player?  Say Hi ?
        PlayerName = player.Name;
        WorddiceLength = player.WordDiceGameLength;


}
    //---------------------------//
    // Finished Singelton set up //
    // --------------------------//
    #endregion


    #region Unity API
    // Start is called before the first frame update
    void Start()
    {
        NewHoverOver = -1;
        OldHoverOver = -1;
        // all loaded
        GameState = 1;
    }


    // Update is called once per frame
    void Update()
    {
        // sets HoverOver values to the returned value from any IisOverlayTile class (if none, then -1)
        CheckHoverOver();
        CheckClicks();
    }
    #endregion


    #region HoverOver and Clicks
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


    private void CheckClicks()
    {
        if (GameState == 1)  // only check clicks if "at table and selecting"
        {
            // will need "do transition" eventually ... just going to fire up my game
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 8881)
            {
                GameState = 32;
                WordDice.KickOff();
            }
        }
    }

    #endregion


    #region PlayerStats manipulation
    // Any game/GUI SHOULD use this to save any changes to gc.player (the PlayerStats Struct)
    public void SaveStats()
    {
        playerManager.SavePlayer(player);
    }
    #endregion

}
