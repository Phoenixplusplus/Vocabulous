//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Clock clock;
    public ShowList showList;
    [SerializeField]
    private int GameTime = 30;
    private double StartTime = 0.00;
    private bool TimerEndSFXFired = false;
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
    public bool ShortGame = false;
    [Header("Player Stats & Score")]
    [SerializeField] private int CurrScore;
    [SerializeField] private int GamesPlayed;
    [SerializeField] private int HighScore;
    [SerializeField] private float AverageScore;
    [SerializeField] private int MostWords;
    [SerializeField] private float AverageWords;
    [SerializeField] private int Longest;

    public FlashProTemplate FindGood;
    public FlashProTemplate FindBad;
    public FlashProTemplate FindSame;
    public FlashProTemplate Reward;
    public FlashProTemplate NewGame;
    public GameObject GUI_Backdrop;

    private TextMeshProUGUI GUIScore;
    private TextMeshProUGUI GUIHighMeanScore;
    private TextMeshProUGUI GUIWords;

    #endregion

    #region UNITY API


    // Start is called before the first frame update
    void Start()
    {
        // Connect to Game Controller and establish links
        gc = GC.Instance;
        if (gc != null) Debug.Log("ConWordDice:Awake() - connected to Game Controller");
        trie = gc.maxTrie;
        if (trie == null) Debug.Log("ConWordDice:Awake() - CANNOT connect to gc.maxTrie");
        myMenu.OnSceneTable();

        // Configure on-screen Flashes
        // 2 Part Lerp ... "You Got WORD" followed by Score Addition
        // goes mid screen -> mid right upper -> top right
        FindGood = new FlashProTemplate(); 
        FindGood.SingleLerp = false;
        FindGood.StartPos = new Vector2(0.46f, 0.75f);
        FindGood.MiddlePos = new Vector2(0.65f, 0.8f);
        FindGood.FinishPos = new Vector2(0.52f, 0.92f);
        FindGood.StartAlpha = 1f;
        FindGood.MiddleAlpha = 0.5f;
        FindGood.FinishAlpha = 1f;
        FindGood.StartWidth = 0.4f;
        FindGood.MiddleWidth = 0.2f;
        FindGood.FinishWidth = 0.1f;
        FindGood.myMessage1 = "You Found XXX";
        FindGood.myMessage2 = "+1 Pt";
        FindGood.TextColor1 = Color.green;
        FindGood.TextColor2 = Color.green;
        FindGood.UseXLerpOnly = true;
        FindGood.Xtween1 = Tween.BounceUp;
        FindGood.Xtween2 = Tween.LinearUp;
        FindGood.AnimTime = 3f;
        FindGood.MiddleTimeRatio = 0.66f;

        // Un recognised word .. mid screen -> top left (and fade out) in Red
        FindBad = FindGood.Copy();
        FindBad.SingleLerp = true;
        FindBad.FinishPos = new Vector2(0.1f, 0.85f);
        FindBad.TextColor1 = Color.red;
        FindBad.FinishAlpha = 0.1f;
        FindBad.Xtween1 = Tween.LinearUp;
        FindBad.AnimTime = 1.5f;

        // Same word (twice) .. mid screen -> mid top in Yellow
        FindSame = FindBad.Copy();
        FindSame.FinishPos = new Vector2(0.5f, 0.95f);
        FindSame.TextColor1 = Color.yellow;

        // Reward (and End Game) Flash ... mid left -> mid bottom right in Green
        // fires for 3.5 seconds, stationary for the last 0.5 seconds
        Reward = FindSame.Copy();
        Reward.SingleLerp = false;
        Reward.StartPos = new Vector2(0.21f, 0.16f);
        Reward.FinishPos = Reward.MiddlePos = new Vector2(0.65f, 0.2f);
        Reward.TextColor1 = Color.green;
        Reward.StartWidth = 0.25f;
        Reward.FinishWidth = Reward.MiddleWidth = 0.4f;
        Reward.StartAlpha = Reward.MiddleAlpha = Reward.FinishAlpha = 1f;
        Reward.AnimTime = 3.5f;
        Reward.MiddleTimeRatio = 6f / 7f;
        Reward.Xtween1 = Tween.ParametricUp;

        // New Game .. from end of "Reward" to screenposition of "New" game dice
        // 4 seconds with the last 1 being a stationary fadeout
        NewGame = Reward.Copy();
        NewGame.SingleLerp = false;
        NewGame.StartPos = Reward.FinishPos;
        NewGame.FinishPos = NewGame.MiddlePos = new Vector2(0.8f, 0.66f);
        NewGame.myMessage1 = NewGame.myMessage2 = "New Game ?";
        NewGame.Xtween1 = Tween.BounceUp;
        NewGame.Xtween2 = Tween.LinearUp;
        NewGame.StartWidth = 0.3f;
        NewGame.FinishWidth = NewGame.MiddleWidth = 0.1f;
        NewGame.StartAlpha = NewGame.MiddleAlpha = 1f;
        NewGame.FinishAlpha = 0f;
        NewGame.AnimTime = 4f;
        NewGame.MiddleTimeRatio = 0.75f;

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
            else
            {
                clock.SetTime((float)Timer);
                if (!TimerEndSFXFired && Timer <= 9.0)
                {
                    TimerEndSFXFired = true;
                    gc.SM.PlaySFX(SFX.NineSecTick);
                }
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
            SetGUI();
        }
    }

    #endregion

    #region GAMECONTROLLER (GC) CALLABLE METHODS

    public void StartGame()
    {

        // TESTER for gc.player ... IT WORKS <<yah me>>
        LoadStats();
        Debug.Log("AverageWords (start game): " + AverageWords.ToString());
        // DEBUG
        if (ShortGame)
        {
            GameTime = 15;
        }
        else
        {
            GameTime = 180;
            gc.player.WordDiceGameLength = 180;
        }
        gc.SaveStats();
        StartTime = Time.realtimeSinceStartup;
        SetGrid();
        PopulateGrid();
        BoggleWords = grid.AllWordStrings; // will be empty as each game has a new grid (since size may vary)
        BoggleWords = gc.assets.SortList(BoggleWords);
        SpawnDice();
        foundListDisplay.init();
        //Debug.Log("ConWordDice:: Started - (" + BoggleWords.Count.ToString() + " answers found): " + (Time.realtimeSinceStartup - StartTime).ToString() + " seconds");
        //Debug.Log("Asking gc.player for desired GameTime .. got:"+ gc.player.WordDiceGameLength.ToString());
        Timer = GameTime;
        clock.SetTime((float)Timer);
        TimerEndSFXFired = false;
        CurrScore = 0;
        gameState = 2;
        myMenu.GameRunning();
        SetupGUI();
        GUI_Backdrop.SetActive(false);
        if (gc.UIController.isMainMenuOpen) gc.UIController.ToggleOptionsInOut();
    }

    void PreStart()
    {
        LoadStats();
        myMenu.GameRunning();
        GUI_Backdrop.SetActive(false);
        SpawnDiceBasic();
        gc.Fire_Start_Flash();
        StartCoroutine("RealStart", 4.5f);
    }

    IEnumerator RealStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetGame();
        StartGame();
    }

    public void TidyUp()
    {
        StopAllCoroutines();
        ResetGame();
        gameState = 0;
        gc.FM.KillStaticGUIs();
        gc.SM.KillSFX();
        myMenu.OnSceneTable();
        GUI_Backdrop.SetActive(false);
    }


    #endregion

    #region GAME (RE)SET METHODS

    public void KickOff() // to be called by GameController
    {
        gc.FM.KillStaticGUIs();
        gc.SM.KillSFX();
        GUI_Backdrop.SetActive(false);
        if (gc.UIController.isWDOpen) StartCoroutine("PauseForOptionsMenu");
        else { PreStart(); }
        
    }

    IEnumerator PauseForOptionsMenu ()
    {
        bool waiting = true;
        while (waiting)
        {
            if (!gc.UIController.isWDOpen) waiting = false;
            yield return null;
        }
        PreStart();
    }

    // Because I'm a complete and utter masocist
    void SetupGUI()
    {
        GameObject gScore = gc.FM.AddGUIItem("Score: 0", 0.45f, 0.95f, 0.2f, Color.white);
        GUIScore = gScore.GetComponent<TextMeshProUGUI>();
        GameObject gHighMean = gc.FM.AddGUIItem("High: 0"+"  "+"Mean: 0", 0.75f, 0.95f, 0.30f, Color.yellow);
        GUIHighMeanScore = gHighMean.GetComponent<TextMeshProUGUI>();
        GameObject gWords = gc.FM.AddGUIItem("WORDS: Longest: 0 Most: 0 Mean: 0", 0.65f, 0.86f, 0.5f, Color.yellow);
        GUIWords = gWords.GetComponent<TextMeshProUGUI>();
    }

    void SetGUI ()
    {
        GUIScore.text = "Score: " + CurrScore;
        GUIHighMeanScore.text = "High: " + HighScore.ToString() + " Mean: " + AverageScore.ToString("#.00");
        GUIWords.text = "WORDS: Longest: "+Longest.ToString()+" Most: "+MostWords.ToString()+" Mean: "+AverageWords.ToString("#.00") ;
    }
    void SetGUIToRed ()
    {
        GUIScore.color = Color.red;
        GUIHighMeanScore.color = Color.red;
        GUIWords.color = Color.red;
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

    void SpawnDiceBasic()
    {
        myDice.transform.localRotation = Quaternion.identity;
            for (int x = 0; x < 16; x++)
            {
                GameObject dice = gc.assets.SpawnDice("?", dbox.slots[x] + transform.position);
                dice.transform.parent = myDice.transform;
            }
        myDice.transform.localRotation = transform.localRotation;
    }

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
        gc.FM.KillAllFlashes();
        gc.SM.KillSFX();
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
                                gc.FM.CustomFlash(FindSame, "Already found " + res.ToUpper());
                                gc.SM.PlayWordSFX(WordSFX.SameWord);
                            }
                            else
                            {
                                // ANIMATE
                                string score = GetWordScore(res).ToString();
                                gc.FM.CustomFlash(FindGood, "Found " + res.ToUpper(), "+" + score + " Pt");
                                // gc.SM.PlaySFX(SFX.Cheer);
                                gc.SM.PlayWordSFX((WordSFX)Random.Range(0, 6));
                                midGameScore(res);
                                FoundWords.Add(res);
                                foundListDisplay.addWord(res, "qu");
                            }
                        }
                        else
                        {
                            // ANIMATE
                            gc.FM.CustomFlash(FindBad, "Don't know " + res.ToUpper());
                            gc.SM.PlayWordSFX(WordSFX.SameWord);
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
                    ResetGame();
                    KickOff();
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
        //SetGUIToRed();
        GUI_Backdrop.SetActive(true);
        clock.SetTime(0f);
        gameState = 3;
        endGameScore();
        Selecting = false;
        myMenu.GameOver();
        grid.ClearPath();
        transform.localRotation = Quaternion.identity;
        showList.Print(BoggleWords,"qu");
        transform.localRotation = Quaternion.Euler(gc.RotTranWordDice);
    }

    #endregion

    #region PLAYER STATS & SCORING
    private void LoadStats()
    {
        GamesPlayed = gc.player.WDPlays;
        HighScore = gc.player.WDHighscore;
        AverageScore = gc.player.WDAverageScore;
        MostWords = gc.player.WDMostWords;
        AverageWords = gc.player.WDAverageWords;
        Longest = gc.player.WDLongest;
    }

    private void SaveStats()
    {
        gc.player.WDPlays = GamesPlayed;
        gc.player.WDHighscore = HighScore;
        gc.player.WDAverageScore = AverageScore;
        gc.player.WDMostWords = MostWords;
        gc.player.WDAverageWords = AverageWords;
        gc.player.WDLongest = Longest;

        gc.SaveStats();
    }

    public void ResetStats()
    {
        GamesPlayed = 0;
        HighScore = 0;
        AverageScore = 0;
        MostWords = 0;
        AverageWords = 0;
        Longest = 0;
        SaveStats();
    }

    private void midGameScore(string word)
    {
        int score = GetWordScore(word);
        CurrScore += score;

        int len = word.Length;
        if (word.Contains("qu")) len--;
        if (len > Longest)
        {
            // ANIMATE New Longest Word
            Debug.Log("New Longest Word : "+len.ToString());
            gc.FM.CustomFlash(Reward, "New Longest Word !!", "New Longest Word !!");
            Longest = len;
        }
    }

    private void endGameScore()
    {
        int count = 0;
        gc.FM.CustomFlash(Reward, "Time Up", "Time Up");
        gc.SM.PlaySFX(SFX.Bell);
        count++;

        if (FoundWords.Count > AverageWords)
        {
            // ANIMATE .. Average words found improved
            Debug.Log("Average Words Improved : "+ AverageWords.ToString()+" -> "+FoundWords.Count.ToString());
            gc.FM.CustomFlash(Reward, "Average Words Improved !", "Average Words Improved !", count * 2.5f);
            gc.SM.PlayMiscSFX(MiscSFX.SwishQuick, count * 2.5f);
            count++;
        }
        if (FoundWords.Count > MostWords)
        {
            gc.FM.CustomFlash(Reward, "Record Word Count !", "Record Word Count !", count * 2.5f);
            gc.SM.PlayMiscSFX(MiscSFX.SwishQuick, count * 2.5f);
            count++;
            MostWords = FoundWords.Count;
        }
        if (CurrScore > AverageScore)
        {
            // ANIMATE .. Average Score Improved
            //Debug.Log("Average Score Improved : "+ AverageScore.ToString()+" -> "+CurrScore.ToString());
            gc.FM.CustomFlash(Reward, "Average Score Improved !", "Average Score Improved !", count * 2.5f);
            gc.SM.PlayMiscSFX(MiscSFX.SwishQuick, count * 2.5f);
            count++;
        }
        if (CurrScore > HighScore)
        {
            // ANIMATE .. New High Score
            //Debug.Log("New High Score : " + CurrScore.ToString());
            gc.FM.CustomFlash(Reward, "New High Score !", "New High Score !", count * 2.5f);
            gc.SM.PlaySFX(SFX.Yeehaw, count * 2.5f);
            count++;
            HighScore = CurrScore;
        }
        gc.FM.CustomFlash(NewGame, count * 2.5f);
        AverageWords = ((AverageWords * GamesPlayed) + FoundWords.Count) / ((float)GamesPlayed + 1);
        Debug.Log("AverageWords (post game): " + AverageWords.ToString());
        AverageScore = ((AverageScore * GamesPlayed) + CurrScore) / ((float)GamesPlayed + 1);
        GamesPlayed++;
        SaveStats();
    }

    private int GetWordScore(string word)
    {
        word = word.ToLower();
        int len = word.Length;
        if (word.Contains("qu")) len--;
        switch (len)
        {
            case 3:
                return 1;
            case 4:
                return 1;
            case 5:
                return 2;
            case 6:
                return 3;
            case 7:
                return 5;
            default:
                return 9;
        }
    }


    #endregion

}
