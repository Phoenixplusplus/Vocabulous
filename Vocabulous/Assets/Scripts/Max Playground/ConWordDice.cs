using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConWordDice : MonoBehaviour
{
    #region VARIABLE DECLARATION
    private GC gc;
    private GameGrid grid;
    private MaxTrie trie;
    public GameObject myDice;         // dice instances of letter dice in GameGrid
    public GameObject FoundList;      // dice "word" instances of player finds 
    public bool Selecting = false;
    private bool GameRunning = false;
    private double StartTime = 0.00;
    public string CurrentWord = "";
    public List<string> FoundWords = new List<string>(); // string list of what the player has found
    private int GSize; // populated from gc.player
    [SerializeField]
    private List<string> BoggleWords; // for reference, list of solutions for this grid
    private int[] dicelist = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
    private string[] faces = {
        "S","T","O","S","I","E",
        "D","R","E","X","I","L",
        "U","I","E","N","S","E",
        "N","A","E","E","A","G",
        "B","O","O","B","A","J",
        "W","H","E","G","E","N",
        "A","O","O","T","T","W",
        "L","E","R","T","T","Y",
        "L","N","Z","R","H","N",
        "M","U","N","H","I","Qu",
        "W","T","V","H","E","R",
        "C","A","H","P","O","S",
        "C","O","T","M","U","I",
        "L","R","Y","V","E","D",
        "S","T","Y","D","I","T",
        "P","S","K","F","F","A" };
    #endregion

    #region UNITY API

    // Called before anything else ... lets connect to the world
    void Awake()
    {
        // Connect to Game Controller and establish links
        gc = GC.Instance;
        if (gc != null) Debug.Log("ConWordDice:Awake() - connected to Game Controller");
        trie = gc.maxTrie;
        if (trie == null) Debug.Log("ConWordDice:Awake() - CANNOT connect to gc.maxTrie");
    }


    // Start is called before the first frame update
    void Start()
    {
        StartGame(); // placeholder tester ... will be called by GC eventually
    }

    // Update is called once per frame
    void Update()
    {
        // Just for Inspector debug purposes
        if (Selecting)
        {
            CurrentWord = grid.GetCurrentPath(); // purely for debug
        }
        else
        {
            CurrentWord = "";
        }

        // Where the game logic really lies
        CheckHoverOver();   // Checks for change to HoverOver (and defines behaviour)
        CheckMouseClicks(); // Defines what happens if the user clicks the mouse
    }

    #endregion

    #region GAMECONTROLLER (GC) CALLABLE METHODS

    public void StartGame()
    {
        StartTime = Time.realtimeSinceStartup;
        // TESTER for gc.player ... IT WORKS <<yah me>>
        //gc.player.WordDiceSize = 4;
        //gc.SaveStats();
        SetGrid();
        PopulateGrid();
        BoggleWords = grid.AllWordStrings; // will be empty as each game has a new grid (since size may vary)
        SpawnDice();
        MakeFoundList();

        Debug.Log("ConWordDice:: Started - (" + BoggleWords.Count.ToString() + " answers found): " + (Time.realtimeSinceStartup - StartTime).ToString() + " seconds");
    }


    #endregion

    #region GAME (RE)SET METHODS

    void SetGrid()
    {
        GSize = gc.player.WordDiceSize;                     // it may have changed between games
        grid = new GameGrid() { dx = GSize, dy = GSize };   // new grid
        grid.trie = gc.maxTrie;                             // link to trie
        grid.init();                                        // initialise
        if (grid.trie == null) Debug.Log("ConWordDice::grid FAILED to connected to Trie");
    }

    void PopulateGrid()
    {
        // current set for 4x4 (default) ... needs customised for different sizes (GSIZE)

        // Using boggle dice.  
        // Shuffle "dicelist" an array of 0-15 representing the dice
        // Loop and establish letter from dice, face (rand 0-5) and faces[] Array
        // Feed the loop counter and letter into the GameGrid
        // Shuffle as per https://forum.unity.com/threads/randomize-array-in-c.86871/
        for (int i = 0; i < dicelist.Length; i++)
        {
            int tmp = dicelist[i];
            int r = Random.Range(i, 16);
            dicelist[i] = dicelist[r];
            dicelist[r] = tmp;
        }
        for (int i = 0; i < GSize * GSize; i++)
        {
            int dice = Random.Range(0, 6);
            grid.PopulateBin(i, "" + faces[(6 * dicelist[i]) + dice]);
        }
        grid.PopulateBOGGLEStrings();

    }

    void SpawnDice()
    {
        int count = 0;
        for (int z = GSize; z > 0; z--)
        {
            for (int x = 0; x < GSize; x++)
            {
                GameObject dice = gc.assets.SpawnDice(grid.bins[count], new Vector3(x, 0, z));
                dice.transform.parent = myDice.transform;
                ConDice con = dice.GetComponent<ConDice>();
                con.ID = count;
                con.myGrid = grid;
                count++;
            }
        }
    }

    void MakeFoundList ()
    {
        GameObject Found = gc.assets.MakeWordFromDiceQ("Found", new Vector3(4.5f, 0, 6), 1f);
        Found.transform.parent = FoundList.transform;
    }

    #endregion

    #region GAME RUNNING METHODS
    void CheckMouseClicks()
    {
        if (!Selecting) // not currently selecting anything
        {
            // Mouse goes down over a non -1 or 9999 IisOverlayTile object (i.e. a valid letter)
            if (Input.GetMouseButtonDown(0) && gc.NewHoverOver != -1 && gc.NewHoverOver != 9999)
            {
                Selecting = true;
                grid.AddToPath(gc.NewHoverOver);
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Selecting = false;
                string res = grid.FinishPath();
                if (res.Length >= 3)
                {
                    if (trie.CheckWord(res))
                    {
                        if (FoundWords.Contains(res))
                        {
                            Debug.Log("You already got that one!");
                        }
                        else
                        {
                            Debug.Log("You got " + res);
                            FoundWords.Add(res);
                            GameObject newWord = gc.assets.MakeWordFromDiceQ(res, new Vector3(4.5f, 0, 5.6f - (FoundWords.Count * 0.6f)), 0.5f);
                            newWord.transform.parent = FoundList.transform;
                        }
                    }
                    else
                    {
                        Debug.Log("Sorry, " + res + " is not in our Dictionary");
                    }
                }
            }
        }
    }

    void CheckHoverOver()
    {

        if (gc.HoverChange && Selecting) // We have a change (whilst selecting)
        {
            if (gc.NewHoverOver == -1) // moves off grid .... reset path
            {
                grid.ClearPath();
                Selecting = false;
            }
            if (grid.legals.Contains(gc.NewHoverOver) || grid.GetPathSecondFromEnd() == gc.NewHoverOver) grid.AddToPath(gc.NewHoverOver);
        }
    }
    #endregion
}
