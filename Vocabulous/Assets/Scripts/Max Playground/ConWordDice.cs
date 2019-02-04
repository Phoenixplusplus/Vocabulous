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
    private bool GameRunning = false;  // sure there will be a use eventually
    public ConTableWordDice myMenu;
    public dicebox dbox;
    public Found_List_Display foundListDisplay;
    public int gameState = 0; // 0 - initialising, 1 = Starting, 2 = running, 3 = scoring (awaiting restart/quit)
    public double Timer;
    public ShowList showList;
    [SerializeField]
    private int GameTime = 30;
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
    [Header("Player Stats & Score")]
    [SerializeField] private int CurrScore;
    [SerializeField] private int GamesPlayed;
    [SerializeField] private int HighScore;
    [SerializeField] private float AverageScore;
    [SerializeField] private float AverageWords;
    [SerializeField] private int Longest;
    [SerializeField] private int F3;
    [SerializeField] private int F4;
    [SerializeField] private int F5;
    [SerializeField] private int F6;
    [SerializeField] private int F7;
    [SerializeField] private int F8;

    #endregion

    #region UNITY API

    // Called before anything else ... lets connect to the world
    void Awake()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        // Connect to Game Controller and establish links
        gc = GC.Instance;
        if (gc != null) Debug.Log("ConWordDice:Awake() - connected to Game Controller");
        trie = gc.maxTrie;
        if (trie == null) Debug.Log("ConWordDice:Awake() - CANNOT connect to gc.maxTrie");
        //transform.position = gc.PosTranWordDice;
        myMenu.OnSceneTable();
    }

    // Update is called once per frame
    void Update()
    {
        // if game running: do timer
        if (Timer > 0 && gameState == 2)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
            {
                Timer = 0;
                TimesUp();
            }
        }

        // Just for Inspector debug purposes
        if (grid != null && Selecting)
        {
            CurrentWord = grid.GetCurrentPath(); // purely for debug
        }
        else
        {
            CurrentWord = "";
        }

        // Where the game logic really lies // ONLY if game is running (or over)
        if (gameState == 2 || gameState == 3)
        {
            CheckHoverOver();   // Checks for change to HoverOver (and defines behaviour)
            CheckMouseClicks(); // Defines what happens if the user clicks the mouse
        }
    }

    #endregion

    #region GAMECONTROLLER (GC) CALLABLE METHODS

    public void StartGame()
    {

        // TESTER for gc.player ... IT WORKS <<yah me>>
        gc.player.WordDiceGameLength = 15;
        gc.SaveStats();

        LoadStats();
        GameTime = gc.player.WordDiceGameLength;
        StartTime = Time.realtimeSinceStartup;
        SetGrid();
        PopulateGrid();
        BoggleWords = grid.AllWordStrings; // will be empty as each game has a new grid (since size may vary)
        BoggleWords = gc.assets.SortList(BoggleWords);
        SpawnDice();
        //MakeFoundList();
        foundListDisplay.init();
        Debug.Log("ConWordDice:: Started - (" + BoggleWords.Count.ToString() + " answers found): " + (Time.realtimeSinceStartup - StartTime).ToString() + " seconds");
        Debug.Log("Asking gc.player for desired GameTime .. got:"+ gc.player.WordDiceGameLength.ToString());
        Timer = GameTime;
        CurrScore = 0;
        gameState = 2;
        myMenu.GameRunning();
    }

    public void TidyUp()
    {
        ResetGame();
        myMenu.OnSceneTable();
    }


    #endregion

    #region GAME (RE)SET METHODS

    public void KickOff() // to be called by GameController
    {
        StartGame();
    }


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
        myDice.transform.localRotation = Quaternion.identity;
        int count = 0;
        for (int z = GSize; z > 0; z--)
        {
            for (int x = 0; x < GSize; x++)
            {
                //GameObject dice = gc.assets.SpawnDice(grid.bins[count], new Vector3(x, 0, z) + transform.position);
                GameObject dice = gc.assets.SpawnDice(grid.bins[count], dbox.slots[count] + transform.position);
                dice.transform.parent = myDice.transform;
                ConDice con = dice.GetComponent<ConDice>();
                con.ID = count;
                con.myGrid = grid;
                count++;
            }
        }
        myDice.transform.localRotation = transform.localRotation;
    }

    //void MakeFoundList ()
    //{
    //    FoundList.transform.localRotation = Quaternion.identity;
    //    GameObject Found = gc.assets.MakeWordFromDiceQ("Found", new Vector3(4.5f, 0, 6)+transform.position, 1f);
    //    Found.transform.parent = FoundList.transform;
    //    FoundList.transform.localRotation = transform.localRotation; // phoenix edit
    //}

    void ResetGame()
    {
        showList.Clear();
        foreach (Transform child in myDice.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in FoundList.transform)
        {
            Destroy(child.gameObject);
        }
        FoundWords.Clear();
        BoggleWords.Clear();
    }

    #endregion

    #region GAME RUNNING METHODS
    void CheckMouseClicks()
    {
        if (gameState == 2) // game running
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
                                // ANIMATE
                                Debug.Log("You already got that one!");
                            }
                            else
                            {
                                // ANIMATE
                                Debug.Log("You got " + res);
                                midGameScore(res);
                                FoundWords.Add(res);
                                foundListDisplay.addWord(res, "qu");
                                //GameObject newWord = gc.assets.MakeWordFromDiceQU(res, new Vector3(4.5f, 0, 5.6f - (FoundWords.Count * 0.6f)) + transform.position, 0.5f);
                                //FoundList.transform.localRotation = Quaternion.identity;
                                //newWord.transform.parent = FoundList.transform;
                                //FoundList.transform.localRotation = transform.localRotation; // phoenix edi
                            }
                        }
                        else
                        {
                            // ANIMATE
                            Debug.Log("Sorry, " + res + " is not in our Dictionary");
                        }
                    }
                }
            }
        }
        if (gameState == 3) // looking for restart
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gc.NewHoverOver == 8882) // restart game
                {
                    Debug.Log("Attempting restart");
                    ResetGame();
                    KickOff();
                }
                if (gc.NewHoverOver == 8883) // back to menu
                {
                    Debug.Log("Want to Return to main table ... but not connected yet");
                }
            }
        }
    }

    void CheckHoverOver()
    {
        if (gc.HoverChange && Selecting && gameState == 2) // We have a change (whilst selecting and game running)
        {
            if (gc.NewHoverOver == -1) // moves off grid .... reset path
            {
                grid.ClearPath();
                Selecting = false;
            }
            if (grid.legals.Contains(gc.NewHoverOver) || grid.GetPathSecondFromEnd() == gc.NewHoverOver) grid.AddToPath(gc.NewHoverOver);
        }
    }

    void TimesUp()
    {
        gameState = 3;
        endGameScore();
        Selecting = false;
        myMenu.GameOver();
        grid.ClearPath();
        transform.localRotation = Quaternion.identity;
        showList.Print(BoggleWords,"qu");
        transform.localRotation = Quaternion.Euler(gc.RotTranWordDice);
        // TO DO
        // Scoring
        // Update / Save Player stats
        // Activate GUI for Times's up, Restart, back or see what could have had
    }

    #endregion

    #region PLAYER STATS & SCORING
    private void LoadStats()
    {
        GamesPlayed = gc.player.WDPlays;
        HighScore = gc.player.WDHighscore;
        AverageScore = gc.player.WDHighscore;
        AverageWords = gc.WordDice.AverageWords;
        Longest = gc.player.WDLongest;
    }

    private void SaveStats()
    {
        gc.player.WDPlays = GamesPlayed;
        gc.player.WDHighscore = HighScore;
        gc.player.WDAverageScore = AverageScore;
        gc.player.WDAverageWords = AverageWords;
        gc.player.WDLongest = Longest;

        gc.SaveStats();
    }

    private void midGameScore(string word)
    {
        word = word.ToLower();
        int len = word.Length;
        if (word.Contains("qu")) len--;
        switch (len)
        {
            case 3:
                CurrScore++;
                F3++;
                Debug.Log("+1 Score");
                // ANIMATE +1 score
                break;
            case 4:
                CurrScore++;
                F4++;
                Debug.Log("+1 Score");
                // ANIMATE +1 score
                break;
            case 5:
                CurrScore += 2;
                F5++;
                Debug.Log("+2 Score");
                // ANIMATE +2 Score
                break;
            case 6:
                CurrScore += 3;
                F6++;
                Debug.Log("+3 Score");
                // ANIMATE +3 Score
                break;
            case 7:
                CurrScore += 5;
                F7++;
                Debug.Log("+5 Score");
                // ANIMATE +5 Score
                break;
            default:
                F8++;
                CurrScore += 10;
                Debug.Log("+10 Score");
                // ANIMATE +10 Score
                break;
        }
        if (len > Longest)
        {
            // ANIMATE New Longest Word
            Debug.Log("New Longest Word : "+len.ToString());
            Longest = len;
        }
    }

    private void endGameScore()
    {
        if (FoundWords.Count > AverageWords)
        {
            // ANIMATE .. Average words found improved
            Debug.Log("Average Words Improved : "+ AverageWords.ToString()+" -> "+FoundWords.Count.ToString());
        }
        if (CurrScore > AverageScore)
        {
            // ANIMATE .. Average Score Improved
            Debug.Log("Average Score Improved : "+ AverageScore.ToString()+" -> "+CurrScore.ToString());
        }
        if (CurrScore > HighScore)
        {
            // ANIMATE .. New High Score
            Debug.Log("New High Score : " + CurrScore.ToString());
            HighScore = CurrScore;
        }
        AverageWords = ((AverageWords * GamesPlayed) + FoundWords.Count) / (GamesPlayed + 1);
        AverageScore = ((AverageScore * GamesPlayed) + CurrScore) / (GamesPlayed + 1);
        GamesPlayed++;
        SaveStats();
    }


    #endregion

}
