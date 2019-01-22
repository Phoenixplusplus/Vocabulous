using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConWordDice : MonoBehaviour
{
    private GC gc;
    public GameObject OverlayPrefab;
    private GameGrid grid;
    private MaxTrie trie;
    public GameObject tiles;
    public GameObject myDice;
    public bool Selecting = false;
    public string CurrentWord = "";
    public List<string> FoundWords = new List<string>();
    public List<int> GridLegals;    // for debug
    public int currDirection;       // for debug
    [SerializeField]
    private List<string> BoggleWords;
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

    // Start is called before the first frame update
    void Start()
    {
        // Connect to Game Controller and establish links
        gc = GC.Instance;
        if (gc != null) Debug.Log("GAME:Start() - connected to Game Controller");
        trie = gc.maxTrie;
        if (trie == null) Debug.Log("oops");

        // Set up GameGrid
        grid = new GameGrid() { dx = 4, dy = 4 };
        grid.trie = gc.maxTrie;
        grid.init();
        if (grid.trie != null) Debug.Log("grid connected to Trie");
        GridLegals = grid.legals; // purely for debug purposes
        currDirection = grid.currDir; // ditto
        PopulateGrid();
        BoggleWords = grid.AllWordStrings;
        Debug.Log("New Grid x: " + grid.dx.ToString() + " y: " + grid.dy.ToString());

        // Set up Overlay Tiles in a grid, link each tile to the new GameGrid
        int count = 0;
        //for (int y = 4; y > 0; y--)
        //{
        //    for (int x = 0; x < 4; x++)
        //    {
        //        GameObject tile = Instantiate(OverlayPrefab, new Vector3(x, y, 0),Quaternion.identity);
        //        tile.transform.parent = tiles.transform;
        //        Tile_Controlller tilecon = tile.GetComponent<Tile_Controlller>();
        //        tilecon.setID(count);
        //        count++;
        //        tilecon.myGrid = grid;
        //        tilecon.SetVisible(false);
        //    }
        //}
        double Start = Time.realtimeSinceStartup;
        grid.PopulateBOGGLEStrings();
        Debug.Log("Boggle Path Strings - Loaded ("+BoggleWords.Count.ToString()+" found): " + (Time.realtimeSinceStartup - Start).ToString() + " seconds");

        count = 0;
        for (int z = 4; z > 0; z--)
        {
            for (int x = 0; x < 4; x++)
            {
                GameObject dice = gc.assets.SpawnDice(grid.bins[count], new Vector3(x, 0, z));
                dice.transform.parent = myDice.transform;
                ConDice con = dice.GetComponent<ConDice>();
                con.ID = count;
                con.myGrid = grid;
                count++;
            }
        }

        GameObject Found = gc.assets.MakeWordFromDiceQ("Found", new Vector3(4.5f, 0, 6), 1f);


    }

    void PopulateGrid()
    {
        // Using boggle dice.  
        // Shuffle "dicelist" an array of 0-15 representing the dice
        // Loop and establish letter from dice, face (rand 0-5) and faces[] Array
        // Feed the loop counter and letter into the GameGrid
        // Shuffle as per https://forum.unity.com/threads/randomize-array-in-c.86871/
        for (int i = 0; i < 16; i++)
        {
            int tmp = dicelist[i];
            int r = Random.Range(i, 16);
            dicelist[i] = dicelist[r];
            dicelist[r] = tmp;
        }
        for (int i = 0; i < 16; i++)
        {
            int dice = Random.Range(0, 6);
            grid.PopulateBin(i, "" + faces[(6 * dicelist[i]) + dice]);
        }


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
}
