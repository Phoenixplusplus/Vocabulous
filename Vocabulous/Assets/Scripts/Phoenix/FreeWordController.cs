/* random letter based on weighting: http://pi.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html?fbclid=IwAR3gpj-HzjT6s2GQ2wBlYq4eZbdJ7uA6SjFhSrcDYb-CXHBtpaB3cdCjyr0 */

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeWordController : MonoBehaviour
{
    GC gameController;
    GameGrid grid;
    public GameObject tileHolder;
    GameObject[] instancedTiles = new GameObject[100];
    List<GameObject> instancedFoundTiles = new List<GameObject>();
    Vector3 instancedFoundTilesScale = new Vector3(.6f, .6f, .6f);
    public FreeWordTable freeWordTable;
    public TrieTest trie;
    int minimumLengthWord = 2;
    int gridXLength = 10;
    int gridYLength = 10;
    public List<string> foundWords = new List<string>();
    public Dictionary<string, uint> weights = new Dictionary<string, uint>
    {
        { "a", 11306 }, { "b", 9764 }, { "c", 17500 }, { "d", 11594 }, { "e", 8016 },{ "f", 7074 },
        { "g", 6525 }, { "h", 7043 }, { "i", 7402 }, { "j", 1759 }, { "k", 2120 },{ "l", 5728 },
        { "m", 10863 }, { "n", 3808 }, { "o", 5490 }, { "p", 16442 }, { "q", 1086 },{ "r", 9993 },
        { "s", 22310 }, { "t", 10543 }, { "u", 8067 }, { "v", 3423 }, { "w", 4005 },{ "x", 237 },
        { "y", 729 }, { "z", 855 }
    };
    uint totalWeight = 0;
    [SerializeField]
    bool selecting = false;
    public bool isInitialised = false;
    bool showingRestart;
    public bool timeUp;
    public int score = 0;

    FlashProTemplate f_foundWord, f_foundSame, f_timeNotification, f_endNotification;
    TextMeshProUGUI g_Score, g_highScore, g_longestWord, g_averageScore, g_averageWord;

    public int FWHighScore, FWLongestWordCount, FWTimesCompleted, FWGameTime;
    public float FWAverageScore, FWAverageWord;
    public string FWLongestWord;

    #region StartUp
    // main initialise function, will call subsequent StartUp functions
    public void Initialise()
    {
        score = 0;

        // gamecontroller and initialising variables
        gameController = GC.Instance;
        trie = gameController.phoenixTrie;

        LoadPlayerPreferences();
        ConfigureFlashes();
        SetupGUI();

        freeWordTable.IngameSetup();
        freeWordTable.clock.GetComponent<Clock>().StartClock(FWGameTime, 0);

        // sum weights.Value for 'totalWeight' from start as this wont change
        foreach (KeyValuePair<string, uint> weight in weights)
        {
            totalWeight = totalWeight + weight.Value;
        }

        // setup grid and rules
        grid = new GameGrid() { dx = gridXLength, dy = gridYLength };
        grid.init();
        grid.directional = true;

        PlaceTilesInGrid();

        isInitialised = true;
    }

    void LoadPlayerPreferences()
    {
        FWGameTime = gameController.player.FWGameTime;
        FWHighScore = gameController.player.FWHighScore;
        FWLongestWord = gameController.player.FWLongestWord;
        FWLongestWordCount = gameController.player.FWLongestWordCount;
        FWTimesCompleted = gameController.player.FWTimesCompleted;
        FWAverageScore = gameController.player.FWAverageScore;
        FWAverageWord = gameController.player.FWAverageWord;
    }

    // based on accumulated weight > random number, return string 'Key' in Dictionary 'weights'
    string GetRandomLetter(uint tw, bool isWeighted)
    {
        if (isWeighted)
        {
            float rand = Random.Range(0, tw + 1);
            float accumWeight = 0;
            int count;

            for (count = 0; count < weights.Count; count++)
            {
                accumWeight = accumWeight + weights.Values.ElementAt(count);
                if (accumWeight > rand) break;
            }
            return weights.ElementAt(count).Key;
        }
        else
        {
            string str = "abcdefghijklmnopqrstuvwxyz";
            char c = str[Random.Range(0, str.Length)];
            return str = "" + c;
        }
    }

    // spawning function
    public void PlaceTilesInGrid()
    {
        // populate with random weighted chars
        for (int i = 0; i < gridXLength * gridYLength; i++)
        {
            grid.PopulateBin(i, GetRandomLetter(totalWeight, true));
        }

        // spawn tiles
        int count = 0;
        for (int z = gridYLength; z > 0; z--)
        {
            for (int x = 0; x < gridXLength; x++)
            {
                GameObject tile = gameController.assets.SpawnTile(grid.bins[count].ToString().ToLower() + "_", new Vector3(tileHolder.transform.position.x + x, tileHolder.transform.position.y, tileHolder.transform.position.z + z), false, true);
                tile.transform.parent = tileHolder.transform;
                Con_Tile2 tileCon = tile.GetComponent<Con_Tile2>();
                tileCon.SetID(count, count);
                tileCon.myGrid = grid;
                tileCon.isFreeWord = true;
                instancedTiles[count] = tile;
                count++;
            }
        }
        tileHolder.transform.localRotation = transform.localRotation;
    }
    #endregion



    #region Update Loop and Functions
    // update
    void Update()
    {
        if (gameController != null)
        {
            CheckGCHoverValue();
            InputAndSearch();
            SetGUI();
        }

        if (timeUp)
        {
            if (!showingRestart)
            {
                freeWordTable.RestartSetup();
                ClearTiles();
                RunEndFlashesAndSaveStats();
                gameController.SM.PlayMiscSFX((MiscSFX)Random.Range(0, 3));
                showingRestart = true;
            }
            if (Input.GetMouseButtonDown(0) && gameController.NewHoverOver == 5552) Restart();
        }
    }

    // input and trie search
    void InputAndSearch()
    {
        /* hit a tile, mouse down */
        if (!selecting)
        {
            if (Input.GetMouseButtonDown(0) && gameController.NewHoverOver != -1)
            {
                selecting = true;
                grid.AddToPath(gameController.NewHoverOver);
            }
        }
        /* search string, mouse up */
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                selecting = false;
                string res = grid.GetCurrentPath();
                if (res.Length >= minimumLengthWord)
                {
                    if (trie.SearchString(res, false, true, false, 0, false))
                    {
                        if (!foundWords.Contains(res))
                        {
                            Debug.Log("You got " + res);
                            foundWords.Add(res);
                            SpawnFoundWordTiles();
                            gameController.SM.PlayWordSFX((WordSFX)Random.Range(0, 6));
                            score += GetScore(res.Length);
                            DisplayPointFlash(res.Length);
                            grid.FinishPath();
                        }
                        else
                        {
                            Debug.Log("You already found " + res + "!");
                            gameController.FM.CustomFlash(f_foundSame, "Already found " + res);
                            gameController.SM.PlayWordSFX(WordSFX.SameWord);
                            grid.FinishPath();
                        }
                    }
                    else
                    {
                        Debug.Log("Sorry, " + res + " is not on the list!");
                        grid.FinishPath();
                    }
                }
                else grid.FinishPath();
            }
        }
    }

    // middle man between GC and this class' grid
    void CheckGCHoverValue()
    {
        if (gameController.NewHoverOver != gameController.OldHoverOver && selecting)
        {
            if (gameController.NewHoverOver == -1)
            {
                grid.ClearPath();
                selecting = false;
            }
            if (grid.legals.Contains(gameController.NewHoverOver) || grid.GetPathSecondFromEnd() == gameController.NewHoverOver) grid.AddToPath(gameController.NewHoverOver);
        }
    }
    #endregion


    #region Juice / GUI
    void SpawnFoundWordTiles()
    {
        List<int> foundIDs = grid.GetCurrentPathIDs();
        int count = 2;
        foreach (int ID in foundIDs)
        {
            GameObject tile = gameController.assets.SpawnTile(grid.bins[ID].ToString().ToLower() + "_", new Vector3(instancedTiles[ID].transform.position.x, instancedTiles[ID].transform.position.y + 1, instancedTiles[ID].transform.position.z), false, true);
            tile.transform.localRotation = transform.localRotation;
            tile.GetComponent<Con_Tile2>().killOverlayTile();
            tile.transform.localScale = instancedFoundTilesScale;
            tile.GetComponent<Con_Tile2>().ChangeTileColorAdditive(gameController.ColorBodyHighlight);

            tile.AddComponent<Lerp>().Configure(tile.transform.position, (instancedTiles[99].transform.position + (instancedTiles[99].transform.right * (count * instancedFoundTilesScale.x))) + ((instancedTiles[99].transform.forward / 1.4f) * (foundWords.Count - 1)), 1f, false);
            tile.GetComponent<Lerp>().Go();
            count++;
            gameController.SM.PlayTileSFX((TileSFX)Random.Range(2, 6), .8f);
            instancedFoundTiles.Add(tile);
        }
    }

    void ConfigureFlashes()
    {
        // found word flash
        f_foundWord = new FlashProTemplate();
        f_foundWord.SingleLerp = false;
        f_foundWord.StartPos = new Vector2(0.15f, 0.2f);
        f_foundWord.MiddlePos = new Vector2(0.15f, 0.4f);
        f_foundWord.FinishPos = new Vector2(0.15f, 1.0f);
        f_foundWord.StartWidth = 0.1f;
        f_foundWord.MiddleWidth = 0.15f;
        f_foundWord.FinishWidth = 0.1f;
        f_foundWord.StartAlpha = 0.8f;
        f_foundWord.MiddleAlpha = 1f;
        f_foundWord.FinishAlpha = 0;
        f_foundWord.myMessage1 = "Found a word!";
        f_foundWord.myMessage2 = "Good/Great/Excellent!";
        f_foundWord.TextColor1 = Color.cyan;
        f_foundWord.TextColor2 = Color.green;
        f_foundWord.Xtween1 = Tween.LinearUp;
        f_foundWord.Xtween2 = Tween.QuinUp;
        f_foundWord.AnimTime = 2.5f;
        f_foundWord.MiddleTimeRatio = .3f;

        // same word flash
        f_foundSame = new FlashProTemplate();
        f_foundSame.SingleLerp = true;
        f_foundSame.StartPos = new Vector2(0.15f, 0.8f);
        f_foundSame.FinishPos = new Vector2(0.15f, 0.1f);
        f_foundSame.StartWidth = 0.3f;
        f_foundSame.FinishWidth = 0.1f;
        f_foundSame.StartAlpha = 1f;
        f_foundSame.FinishAlpha = 0.2f;
        f_foundSame.myMessage1 = "Already found!";
        f_foundSame.myMessage2 = "Already found!";
        f_foundSame.TextColor1 = Color.red;
        f_foundSame.TextColor2 = Color.red;
        f_foundSame.Xtween1 = Tween.LinearUp;
        f_foundSame.AnimTime = 2f;

        // time notification flash
        f_timeNotification = new FlashProTemplate();
        f_timeNotification.SingleLerp = false;
        f_timeNotification.StartPos = new Vector2(0.5f, 0.9f);
        f_timeNotification.MiddlePos = new Vector2(0.5f, 0.5f);
        f_timeNotification.FinishPos = new Vector2(0.5f, 0.1f);
        f_timeNotification.StartWidth = 0.1f;
        f_timeNotification.MiddleWidth = 0.15f;
        f_timeNotification.FinishWidth = 0.1f;
        f_timeNotification.StartAlpha = 0.8f;
        f_timeNotification.MiddleAlpha = 1f;
        f_timeNotification.FinishAlpha = 0;
        f_timeNotification.myMessage1 = "Found a word!";
        f_timeNotification.myMessage2 = "Good/Great/Excellent!";
        f_timeNotification.TextColor1 = Color.green;
        f_timeNotification.TextColor2 = Color.red;
        f_timeNotification.Xtween1 = Tween.LinearUp;
        f_timeNotification.Xtween2 = Tween.QuinUp;
        f_timeNotification.AnimTime = 2.5f;
        f_timeNotification.MiddleTimeRatio = .3f;

        // end notification
        f_endNotification = new FlashProTemplate();
        f_endNotification.SingleLerp = false;
        f_endNotification.StartPos = new Vector2(0.1f, 0.5f);
        f_endNotification.MiddlePos = new Vector2(0.5f, 0.5f);
        f_endNotification.FinishPos = new Vector2(0.9f, 1.0f);
        f_endNotification.StartWidth = 0.3f;
        f_endNotification.MiddleWidth = 0.3f;
        f_endNotification.FinishWidth = 0.1f;
        f_endNotification.StartAlpha = 0.8f;
        f_endNotification.MiddleAlpha = 1f;
        f_endNotification.FinishAlpha = 0;
        f_endNotification.myMessage1 = "Found a word!";
        f_endNotification.myMessage2 = "Good/Great/Excellent!";
        f_endNotification.TextColor1 = Color.yellow;
        f_endNotification.TextColor2 = Color.red;
        f_endNotification.Xtween1 = Tween.LinearUp;
        f_endNotification.Xtween2 = Tween.QuinUp;
        f_endNotification.AnimTime = 3f;
        f_endNotification.MiddleTimeRatio = .6f;
    }

    void SetupGUI()
    {
        GameObject obj_Score = gameController.FM.AddGUIItem("Score: 0", 0.25f, 0.95f, 0.12f, Color.white);
        g_Score = obj_Score.GetComponent<TextMeshProUGUI>();

        GameObject obj_highScore = gameController.FM.AddGUIItem("Highscore: 0", 0.75f, 0.95f, 0.12f, Color.white);
        g_highScore = obj_highScore.GetComponent<TextMeshProUGUI>();

        GameObject obj_averageScore = gameController.FM.AddGUIItem("Avarage Score: 0", 0.50f, 0.95f, 0.12f, Color.white);
        g_averageScore = obj_averageScore.GetComponent<TextMeshProUGUI>();

        GameObject obj_longestWord = gameController.FM.AddGUIItem("Longest Word: 0", 0.25f, 0.87f, 0.12f, Color.white);
        g_longestWord = obj_longestWord.GetComponent<TextMeshProUGUI>();

        GameObject obj_averageWord = gameController.FM.AddGUIItem("Average Words: 0", 0.50f, 0.87f, 0.12f, Color.white);
        g_averageWord = obj_averageWord.GetComponent<TextMeshProUGUI>();
    }

    void SetGUI()
    {
        g_Score.text = "Score: " + score;
        g_highScore.text = "Highscore: " + FWHighScore;
        g_averageScore.text = "Average Score: " + FWAverageScore;
        g_longestWord.text = "Longest Word: " + FWLongestWord + ", " + FWLongestWordCount;
        g_averageWord.text = "Average Words: " + FWAverageWord;
    }

    void SetGUIToRed()
    {
        g_Score.color = Color.red;
        g_highScore.color = Color.red;
        g_averageScore.color = Color.red;
        g_longestWord.color = Color.red;
        g_averageWord.color = Color.red;
    }
    #endregion


    #region Restart and TidyUp
    // restarting fresh / initialising functions
    public void Restart()
    {
        score = 0;
        freeWordTable.IngameSetup();
        grid.init();
        ClearTiles();
        gameController.FM.KillAllFlashes();
        gameController.FM.KillStaticGUIs();
        gameController.SM.KillSFX();
        SetupGUI();
        foundWords.Clear();
        freeWordTable.clock.GetComponent<Clock>().StartClock(FWGameTime, 0);
        tileHolder.transform.localRotation = Quaternion.identity;
        PlaceTilesInGrid();
        gameController.SM.PlayTileSFX((TileSFX)Random.Range(13,15));
        showingRestart = false;
    }

    // clear previous words on the board, delete all dice, change tabledice to view when around table
    public void TidyUp()
    {
        foundWords.Clear();
        foreach (Con_Tile2 tile in tileHolder.GetComponentsInChildren<Con_Tile2>())
        {
            Destroy(tile.gameObject);
        }
        foreach (GameObject tile in instancedFoundTiles)
        {
            Destroy(tile);
        }
        instancedFoundTiles.Clear();
        freeWordTable.StartSetup();
        gameController.FM.KillAllFlashes();
        gameController.FM.KillStaticGUIs();
    }

    // essentially same as TidyUp, though we do not change any prefabs
    public void ClearTiles()
    {
        foreach (Con_Tile2 tile in tileHolder.GetComponentsInChildren<Con_Tile2>())
        {
            Destroy(tile.gameObject);
        }
        foreach (GameObject tile in instancedFoundTiles)
        {
            Destroy(tile);
        }
        instancedFoundTiles.Clear();
    }

    void SaveStats()
    {
        gameController.player.FWHighScore = FWHighScore;
        gameController.player.FWLongestWord = FWLongestWord;
        gameController.player.FWLongestWordCount = FWLongestWordCount;
        gameController.player.FWTimesCompleted = FWTimesCompleted;
        gameController.player.FWAverageScore = FWAverageScore;
        gameController.player.FWAverageWord = FWAverageWord;
        gameController.SaveStats();
    }

    void RunEndFlashesAndSaveStats()
    {
        float flashDelay = 0;
        int longestWordLen = 0;
        string longestWordStr = "";

        if (score > FWHighScore)
        {
            f_endNotification.StartPos = new Vector2(0.1f, 0.3f);
            f_endNotification.MiddlePos = new Vector2(0.5f, 0.3f);
            gameController.FM.CustomFlash(f_endNotification, "New High Score!", score.ToString(), flashDelay +.5f);
            gameController.SM.PlayMiscSFX(MiscSFX.SwishQuick, flashDelay +.5f);
            gameController.SM.PlayMiscSFX((MiscSFX)Random.Range(3, 9), (flashDelay +.5f) + (f_endNotification.AnimTime * f_endNotification.MiddleTimeRatio));
            FWHighScore = score;
            flashDelay++;
        }

        if (score > FWAverageScore)
        {
            f_endNotification.StartPos = new Vector2(0.1f, 0.4f);
            f_endNotification.MiddlePos = new Vector2(0.5f, 0.4f);
            gameController.FM.CustomFlash(f_endNotification, "New Best Average Score!", score.ToString(), flashDelay +.5f);
            gameController.SM.PlayMiscSFX(MiscSFX.SwishQuick, flashDelay + .5f);
            gameController.SM.PlayMiscSFX((MiscSFX)Random.Range(3, 9), (flashDelay + .5f) + (f_endNotification.AnimTime * f_endNotification.MiddleTimeRatio));
            flashDelay++;
        }

        foreach (string word in foundWords)
        {
            if (word.Length > longestWordLen)
            {
                longestWordLen = word.Length;
                longestWordStr = word;
            }
        }
        if (longestWordLen > FWLongestWordCount)
        {
            f_endNotification.StartPos = new Vector2(0.1f, 0.5f);
            f_endNotification.MiddlePos = new Vector2(0.5f, 0.5f);
            gameController.FM.CustomFlash(f_endNotification, "New Longest Word!", longestWordStr + ", " + longestWordLen.ToString(), flashDelay + .5f);
            gameController.SM.PlayMiscSFX(MiscSFX.SwishQuick, flashDelay + .5f);
            gameController.SM.PlayMiscSFX((MiscSFX)Random.Range(3, 9), (flashDelay + .5f) + (f_endNotification.AnimTime * f_endNotification.MiddleTimeRatio));
            flashDelay++;
            FWLongestWordCount = longestWordLen;
            FWLongestWord = longestWordStr;
        }

        if (foundWords.Count > FWAverageWord)
        {
            f_endNotification.StartPos = new Vector2(0.1f, 0.6f);
            f_endNotification.MiddlePos = new Vector2(0.5f, 0.6f);
            gameController.FM.CustomFlash(f_endNotification, "New Best Average Words!", foundWords.Count.ToString(), flashDelay + .5f);
            gameController.SM.PlayMiscSFX(MiscSFX.SwishQuick, flashDelay + .5f);
            gameController.SM.PlayMiscSFX((MiscSFX)Random.Range(3, 9), (flashDelay + .5f) + (f_endNotification.AnimTime * f_endNotification.MiddleTimeRatio));
            flashDelay++;
        }

        FWAverageWord = ((FWAverageWord * FWTimesCompleted) + foundWords.Count) / (FWTimesCompleted + 1);
        FWAverageScore = ((FWAverageScore * FWTimesCompleted) + score) / (FWTimesCompleted + 1);
        FWTimesCompleted++;
        SaveStats();
    }
    #endregion

    #region Misc
    int GetScore(int strLen)
    {
        switch(strLen)
        {
            case 2: return 1;
            case 3: return 1;
            case 4: return 2;
            case 5: return 4;
            case 6: return 6;
            case 7: return 10;
            case 8: return 14;
            case 9: return 18;
            case 10: return 25;
            case 11: return 32;
            case 12: return 39;
            default: return 0;
        }
    }

    void DisplayPointFlash(int strLen)
    {
        switch (strLen)
        {
            case 2:
                {
                    f_foundWord.TextColor2 = Color.green;
                    gameController.FM.CustomFlash(f_foundWord, strLen + " letter word", GetScore(strLen).ToString() + "pt!");
                    break;
                }
            case 3:
                {
                    f_foundWord.TextColor2 = Color.green;
                    gameController.FM.CustomFlash(f_foundWord, strLen + " letter word", GetScore(strLen).ToString() + "pt!");
                    break;
                }
            default:
                {
                    f_foundWord.TextColor2 = Color.yellow;
                    gameController.FM.CustomFlash(f_foundWord, strLen + " letter word", GetScore(strLen).ToString() + "pts!");
                    break;
                }
        }
    }
    #endregion
}