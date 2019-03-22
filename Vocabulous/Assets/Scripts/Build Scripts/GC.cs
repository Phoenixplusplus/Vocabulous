using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GC : MonoBehaviour
{
    #region Variable Declaration
    [Header("TICK THIS to Reset to Default Player on load")]
    public bool UseDefaultPlayer = true;
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

    [Header("Player Info")]
    [SerializeField]
    private string PlayerName;
    public PlayerStats player;
    public PlayerManager playerManager = new PlayerManager();

    [Header("WordDice Info")]
    [SerializeField]
    private int WorddiceLength;

    [Header("WordSearch Info")]
    [SerializeField]
    int wordSearchGridXLength;
    [SerializeField]
    int wordSearchGridYLength;
    [SerializeField]
    int wordSearchMinimumLengthWord;
    [SerializeField]
    int wordSearchMaximumLengthWord;
    [SerializeField]
    int wordSearchFourLetterWordsCount;
    [SerializeField]
    int wordSearchFiveLetterWordsCount;
    [SerializeField]
    int wordSearchSixLetterWordsCount;
    [SerializeField]
    int wordSearchSevenLetterWordsCount;
    [SerializeField]
    int wordSearchEightLetterWordsCount;

    [Header("Assets")]
    public OurAssets assets;
    [Header("SoundManager(SM)")]
    public SoundMan SM;
    [Header("Gui Managers")]
    public FlashProManager FM;
    [Header("The GAME OBJECTS")]
    public UIC UIController;
    public CameraController cameraController;
    public ConWordDice WordDice;
    public WordSearchController wordSearchController;
    public ConTypeWriter solverController;
    public ConAnagram anagramController;
    public FreeWordController freeWordController;

    [Header("Game Positions (for positional tweaking)")] // can't have a transform (more's the pity)
    public Vector3 PosWordSearch = new Vector3();
    public Vector3 PosTranWordDice = new Vector3();
    public Vector3 PosTranAnagram = new Vector3();
    public Vector3 PosTranFreeWord = new Vector3();
    public Vector3 PosTranSolver = new Vector3();
    [Header("Game Rotations (for rotational tweaking)")]
    public Vector3 RotWordSearch = new Vector3();
    public Vector3 RotTranWordDice = new Vector3();
    public Vector3 RotTranAnagram = new Vector3();
    public Vector3 RotTranFreeWord = new Vector3();
    public Vector3 RotTranSolver = new Vector3();
    [Header("Game Scales (for scale tweaking)")]
    public Vector3 ScaleWordSearch = new Vector3();
    public Vector3 ScaleTranWordDice = new Vector3();
    public Vector3 ScaleTranAnagram = new Vector3();
    public Vector3 ScaleTranFreeWord = new Vector3();
    public Vector3 ScaleTranSolver = new Vector3();
    [Header("Default Dice/face colours")]
    public Color ColorBase = new Color();
    public Color ColorSelected = new Color();
    public Color ColorHighlight = new Color();
    public Color ColorLegal = new Color();
    public Color ColorBodyHighlight = new Color();

    [Header("THE GAME STATE")]
    public int GameState = 0;
    //... needs to be agreed ... maybe 
    // 0 = initialising/loading
    // 1 = at table (choosing)
    // 2 = transition to game area
    // 30 = In Game
    // 31 = Playing WordDice
    // 32 = Playing Solver
    // 33 = Playing Anagram
    // 34 = Playing WordDrop
    // 35 = Playing WordSearch
    // 5 = transitioning from a game to 1 again
    // 9 = Quitting

    private FlashProTemplate Flash_Welcome, Flash_Options, Flash_ShutOptions, Flash_Start;

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
    //---------------------------//
    // Finished Singelton set up //
    // --------------------------//
    #endregion


    #region Unity API
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
        // ESSENTIAL IF YOU ARE EDITING PlayerStats stuff // N.B. have an inspector toggle these days
        if (UseDefaultPlayer) playerManager.ResetToDefault();
        player = playerManager.LoadPlayer();
        // Do stuff for a new player?  Say Hi ?
        PlayerName = player.Name;

        WorddiceLength = player.WordDiceGameLength;

        wordSearchGridXLength = player.WordSearchSize;
        wordSearchGridYLength = player.WordSearchSize;
        wordSearchMinimumLengthWord = player.WordSearchMinimumLengthWord;
        wordSearchMinimumLengthWord = player.WordSearchMaximumLengthWord;
        wordSearchFourLetterWordsCount = player.WordSearchFourLetterWordsCount;
        wordSearchFiveLetterWordsCount = player.WordSearchFiveLetterWordsCount;
        wordSearchSixLetterWordsCount = player.WordSearchSixLetterWordsCount;
        wordSearchSevenLetterWordsCount = player.WordSearchSevenLetterWordsCount;
        wordSearchEightLetterWordsCount = player.WordSearchEightLetterWordsCount;

        ConfigureFlashes();
    }

    void ConfigureFlashes()
    {
        Flash_Welcome = new FlashProTemplate();
        Flash_Welcome.myMessage1 = "Welcome ";
        Flash_Welcome.StartPos = new Vector2(0.5f, 0.95f);
        Flash_Welcome.FinishPos = new Vector2(0.5f, 0.4f);
        Flash_Welcome.StartWidth = 0.05f;
        Flash_Welcome.FinishWidth = 0.6f;
        Flash_Welcome.TextColor1 = Color.yellow;
        Flash_Welcome.AnimTime = 2.5f;
        Flash_Welcome.Xtween1 = Flash_Welcome.Ytween1 = Tween.QuadUp;

        Flash_Options = new FlashProTemplate();
        Flash_Options.myMessage1 = "Options?";
        Flash_Options.StartPos = Flash_Welcome.FinishPos;
        Flash_Options.FinishPos = new Vector2(0.9f, 0.9f);
        Flash_Options.TextColor1 = Color.green;
        Flash_Options.AnimTime = 2f;
        Flash_Options.StartWidth = Flash_Welcome.FinishWidth;
        Flash_Options.FinishWidth = 0.18f;
        Flash_Options.Xtween1 = Flash_Options.Ytween1 = Tween.BounceUp;

        Flash_ShutOptions = new FlashProTemplate();
        Flash_ShutOptions.SingleLerp = true;
        Flash_ShutOptions.myMessage1 = "Shut Options To Play";
        Flash_ShutOptions.StartPos = new Vector2(0.17f, 0.35f);
        Flash_ShutOptions.FinishPos = new Vector2(0.17f, 0.85f);
        Flash_ShutOptions.TextColor1 = Color.green;
        Flash_ShutOptions.StartWidth = 0.3f;
        Flash_ShutOptions.FinishWidth = 0.3f;
        Flash_ShutOptions.AnimTime = 6f;

        Flash_Start = new FlashProTemplate();
        Flash_Start.SingleLerp = true;
        Flash_Start.myMessage1 = "TBA";
        Flash_Start.StartPos = new Vector2(0.5f, 0.3f);
        Flash_Start.FinishPos = new Vector2(0.5f, 0.7f);
        Flash_Start.TextColor1 = Color.green;
        Flash_Start.StartWidth = 0.3f;
        Flash_Start.FinishWidth = 0.5f;
        Flash_Start.AnimTime = 1.5f;
    }

    public void Fire_Start_Flash ()
    {
        Flash_Start.TextColor1 = Color.green;
        Flash_Start.StartWidth = 0.3f;
        Flash_Start.FinishWidth = 0.5f;
        FM.CustomFlash(Flash_Start,"Game Starting");
        Flash_Start.TextColor1 = Color.red;
        Flash_Start.StartWidth = 0.05f;
        Flash_Start.FinishWidth = 0.15f;
        FM.CustomFlash(Flash_Start, "3", 1f);
        Flash_Start.TextColor1 = Color.yellow;
        FM.CustomFlash(Flash_Start, "2", 2f);
        Flash_Start.TextColor1 = Color.green;
        FM.CustomFlash(Flash_Start, "1", 3f);
    }

    // Fire "Close Options to Play" flash
    public void Options_Warning()
    {
        FM.CustomFlash(Flash_Options);
    }

    // Start is called before the first frame update
    void Start()
    {
        NewHoverOver = -1;
        OldHoverOver = -1;
        // all loaded
        GameState = 1;
        // Set game locations/rotation
        if (WordDice != null)
        {
            WordDice.transform.position = PosTranWordDice;
            WordDice.transform.localRotation = Quaternion.Euler(RotTranWordDice);
            WordDice.transform.localScale = ScaleTranWordDice;
        }
        if (wordSearchController != null)
        {
            wordSearchController.transform.position = PosWordSearch;
            wordSearchController.transform.localRotation = Quaternion.Euler(RotWordSearch);
            wordSearchController.transform.localScale = ScaleWordSearch;
        }
        if (solverController != null)
        {
            solverController.transform.position = PosTranSolver;
            solverController.transform.rotation = Quaternion.Euler(RotTranSolver);
            solverController.transform.localScale = ScaleTranSolver;
        }
        if (anagramController != null)
        {
            anagramController.transform.position = PosTranAnagram;
            anagramController.transform.rotation = Quaternion.Euler(RotTranAnagram);
            anagramController.transform.localScale = ScaleTranAnagram;
        }
        if (freeWordController != null)
        {
            freeWordController.transform.position = PosTranFreeWord;
            freeWordController.transform.rotation = Quaternion.Euler(RotTranFreeWord);
            freeWordController.transform.localScale = ScaleTranFreeWord;
        }
        // Start Lobby Ambient
        SM.PlayLobbyMusic();
        Flash_Welcome.myMessage1 += player.Name;
        FM.CustomFlash(Flash_Welcome);
        FM.CustomFlash(Flash_Options, Flash_Welcome.AnimTime + 0.2f);
    }


    // Update is called once per frame
    void Update()
    {
        // testings
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    GameObject thing;
        //    if (Random.value > 0.5) thing = assets.SpawnTile("questquest", cameraController.transform.position, Random.value > 0.5, Random.value > 0.5);
        //    else thing = assets.SpawnDice("?", cameraController.transform.position);
        //    Rigidbody rb = thing.AddComponent<Rigidbody>();
        //    thing.transform.localRotation = Random.rotation;
        //    rb.AddForce((cameraController.transform.forward + new Vector3(0, 0.35f, 0)) * 1000);
        //}

        // sets HoverOver values to the returned value from any IisOverlayTile class (if none, then -1)
        CheckHoverOver();
        CheckClicks();
    }
    #endregion

    #region Toggle Options
    public void ShutAllOptionsMenus ()
    {
        if (UIController.isAOpen) UIController.ToggleAOptionsInOut();
        if (UIController.isAUOpen) UIController.ToggleAboutUsInOut();
        if (UIController.isFWOpen) UIController.ToggleFWOptionsInOut();
        if (UIController.isWDOpen) UIController.ToggleWDOptionsInOut();
        if (UIController.isWSOpen) UIController.ToggleWSOptionsInOut();
        if (UIController.isMainMenuOpen) UIController.ToggleOptionsInOut();
    }

    public void OpenOptionsMenu (string which)
    {
        if (!UIController.isMainMenuOpen) UIController.ToggleOptionsInOut();
        switch (which)
        {
            case "A": if (!UIController.isAOpen) UIController.ToggleAOptionsInOut(); break;
            case "FW": if (!UIController.isFWOpen) UIController.ToggleFWOptionsInOut(); break;
            case "WD": if (!UIController.isWDOpen) UIController.ToggleWDOptionsInOut(); break;
            case "WS": if (!UIController.isWSOpen) UIController.ToggleWSOptionsInOut(); break;
        }
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
        // when clicking the intro object for a game at the table, rotate to it
        if (GameState == 1)
        {
            // WordDice
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 8881) { cameraController.RotateToGameWordDice();}
            // Solver
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 7771) { cameraController.RotateToSolver(); }
            // Anagram
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 6661) { cameraController.RotateToGameAnagram(); }
            // FreeWord
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 5551) { cameraController.RotateToGameFreeWord(); }
            // WordSearch
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 4441) { cameraController.RotateToGameWordSearch(); }
        }
        // however, if we already transitioned to that game, go start it up
        if (GameState == 2)
        {
            // WordDice
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 8881 && cameraController.playWordDice) { SetGameState(31); }
            // Solver
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 7771 && cameraController.playSolver) { SetGameState(32); }
            // Anagram
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 6661 && cameraController.playAnagram) { SetGameState(33); }
            // FreeWord
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 5551 && cameraController.playWordDrop) { SetGameState(34); }
            // WordSearch
            if (Input.GetMouseButtonDown(0) && NewHoverOver == 4441 && cameraController.playWordSearch) { SetGameState(35); }
        }
    }

    #endregion


    #region State Controls
    // main function for all controllers
    public void SetGameState(int i)
    {
        int prevState = GameState;
        GameState = i;

        switch (i)
        {
            case 0: break;
            case 1:
                {
                    // called at Start() and by cameraController once it has finished its lerp back out of game area
                    // 1 = at table (choosing)
                    break;
                }
            case 2:
                {
                    // called by 'Play' Button in inspector
                    // 2 = transition to game area
                    cameraController.PlayClicked();
                    UIController.PlayClicked();
                    break;
                }
            case 5:
                {
                    // called by 'Quit' Button in inspector
                    // 5 = transitioning from a game to 1 again
                    SM.PlayLobbyMusic();
                    cameraController.QuitClicked();
                    //UIController.QuitClicked();
                    ReEnableAllGames();
                    OnThisGameQuit(prevState);
                    break;
                }
            case 31:
                {
                    // called in 'CheckClicks()'
                    // 31 = Playing WordDice
                    if(player.WDPlays == 0)
                    {
                        OpenOptionsMenu("WD");
                    }
                    else
                    {
                        ShutAllOptionsMenus();
                    }
                    if (UIController.isWDOpen)
                    {
                        FM.CustomFlash(Flash_ShutOptions);
                    }
                    WordDice.KickOff();
                    DisableOtherGames(WordDice.gameObject);
                    SM.PlayRandomTrack();
                    break;
                }
            case 32:
                {
                    // called in 'CheckClicks()'
                    // 32 = Playing Solver
                    solverController.KickOff();
                    DisableOtherGames(solverController.gameObject);
                    SM.PlayRandomTrack();
                    break;
                }
            case 33:
                {
                    // called in 'CheckClicks()'
                    // 33 = Playing Anagram
                    if (player.ALevel == 0)
                    {
                        OpenOptionsMenu("A");
                    }
                    else
                    {
                        ShutAllOptionsMenus();
                    }
                    if (UIController.isAOpen)
                    {
                        FM.CustomFlash(Flash_ShutOptions);
                    }
                    anagramController.KickOff();
                    DisableOtherGames(anagramController.gameObject);
                    SM.PlayRandomTrack();
                    break;
                }
            case 34:
                {
                    // called in 'CheckClicks()'
                    // 34 = Playing FreeWord
                    if (player.FWTimesCompleted == 0)
                    {
                        OpenOptionsMenu("FW");
                    }
                    else
                    {
                        ShutAllOptionsMenus();
                    }
                    if (UIController.isFWOpen)
                    {
                        FM.CustomFlash(Flash_ShutOptions);
                    }
                    if (!freeWordController.isInitialised) freeWordController.Initialise();
                    else freeWordController.Restart();
                    DisableOtherGames(freeWordController.gameObject);
                    SM.PlayRandomTrack();
                    break;
                }
            case 35:
                {
                    // called in 'CheckClicks()'
                    // 35 = Playing WordSearch
                    if (player.WordSearchTimesCompleted == 0)
                    {
                        OpenOptionsMenu("WS");
                    }
                    else
                    {
                        ShutAllOptionsMenus();
                    }
                    if (UIController.isWSOpen)
                    {
                        FM.CustomFlash(Flash_ShutOptions);
                    }
                    if (!wordSearchController.isInitialised) wordSearchController.Initialise();
                    else wordSearchController.Restart();
                    DisableOtherGames(wordSearchController.gameObject);
                    SM.PlayRandomTrack();
                    break;
                }
        }
    }

    void DisableOtherGames(GameObject thisController)
    {
        if (thisController != WordDice.gameObject) WordDice.gameObject.SetActive(false);
        if (thisController != wordSearchController.gameObject) wordSearchController.gameObject.SetActive(false);
        if (thisController != anagramController.gameObject) anagramController.gameObject.SetActive(false);
        if (thisController != freeWordController.gameObject) freeWordController.gameObject.SetActive(false);
        if (thisController != solverController.gameObject) solverController.gameObject.SetActive(false);
    }

    void ReEnableAllGames()
    {
        WordDice.gameObject.SetActive(true);
        wordSearchController.gameObject.SetActive(true);
        anagramController.gameObject.SetActive(true);
        freeWordController.gameObject.SetActive(true);
        //solverController.gameObject.SetActive(true);
    }

    void OnThisGameQuit(int i)
    {
        switch(i)
        {
            case 31:
                {
                    // we just quit WordDice, do something special
                    SM.KillSFX();
                    WordDice.TidyUp();
                    break;
                }
            case 32:
                {
                    // we just quit Solver, do something special
                    SM.KillSFX();
                    solverController.TidyUp();
                    break;
                }
            case 33:
                {
                    // we just quit Anagram, do something special
                    SM.KillSFX();
                    anagramController.TidyUp();
                    break;
                }
            case 34:
                {
                    // we just quit FreeWord, do something special
                    SM.KillSFX();
                    freeWordController.TidyUp();
                    break;
                }
            case 35:
                {
                    // we just quit WordSearch, do something special
                    SM.KillSFX();
                    wordSearchController.TidyUp();
                    break;
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

    public void ClearAnagramStats()
    {
        anagramController.ResetStats();
        playerManager.ResetAnagrams();
        player = playerManager.LoadPlayer();
    }

    public void ClearWordDiceStats()
    {
        WordDice.ResetStats();
        playerManager.ResetWordDice();
        player = playerManager.LoadPlayer();
    }

    public void ClearWordSearchStats()
    {
        wordSearchController.ResetStats();
        playerManager.ResetWordSearch();
        player = playerManager.LoadPlayer();
    }

    public void ClearFreeWordStats()
    {
        freeWordController.ResetStats();
        playerManager.ResetFreeWord();
        player = playerManager.LoadPlayer();
    }

    #endregion


    #region GUI Functions
    public void ResetWordDiceStats() { ClearWordDiceStats(); SaveStats(); }
    public void ResetAnagramsStats() { ClearAnagramStats(); SaveStats(); }
    public void ResetWordSearchStats() { ClearWordSearchStats(); SaveStats(); }
    public void ResetFreeWordStats() { ClearFreeWordStats(); SaveStats(); }
    #endregion
}
