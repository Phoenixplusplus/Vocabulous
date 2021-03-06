﻿//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

/* random letter based on weighting: http://pi.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html?fbclid=IwAR3gpj-HzjT6s2GQ2wBlYq4eZbdJ7uA6SjFhSrcDYb-CXHBtpaB3cdCjyr0 */

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordSearchController : MonoBehaviour
{
    GC gameController;
    GameGrid grid;
    public GameObject diceHolder;
    public GameObject OverlayPrefab;
    public GameObject[] instancedDice = new GameObject[100];  
    public WordSearchTable wordSearchTable;
    public TrieTest trie;
    // set by player preferences (setup and stats)
    [SerializeField] int gridXLength;
    [SerializeField] int gridYLength;
    [SerializeField] int minimumLengthWord;
    [SerializeField] int maximumLengthWord;
    [SerializeField] int fourLetterWordsCount;
    [SerializeField] int fiveLetterWordsCount;
    [SerializeField] int sixLetterWordsCount;
    [SerializeField] int sevenLetterWordsCount;
    [SerializeField] int eightLetterWordsCount;
    [SerializeField] int gameTime;
    [SerializeField] int bestTime;
    [SerializeField] int averageTime;
    [SerializeField] int worstTime;
    [SerializeField] int timesCompleted;
    [SerializeField] int timesQuit;
    [SerializeField] string g_High;
    [SerializeField] string g_mean;
    //
    public List<string> foundWords = new List<string>();
    public List<string> unfoundWords = new List<string>();
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
    int initialWordsCounter = 0;
    string defaultString = "0";
    int debugPlacedWords = 0;
    public bool timeUp;
    bool showingRestart;
    string clockString;
    bool waiting;
    private AnagramLevels AL;

    public FlashProTemplate f_foundWord;
    public FlashProTemplate f_foundSame;
    public FlashProTemplate f_timeNotification;
    public FlashProTemplate f_endNotification;
    public TextMeshProUGUI g_times;

    #region StartUp

    // main initialise function, will call subsequent StartUp functions
    // called by the GC on the correct state if not already initialised
    public void Initialise()
    {
        // gamecontroller and initialising variables
        gameController = GC.Instance;
        trie = gameController.phoenixTrie;
        AL = GetComponent<AnagramLevels>();

        // set variables based on player preferences
        LoadPlayerPreferences();

        ConfigureFlashes();
        ConfigureGUI();
    
        // hide/unhide table prefabs 
        wordSearchTable.IngameSetup();

        // check maximumWordLength against maximum word in dictionary and bounds of grid so everything fits/is a legal word
        if (maximumLengthWord > 17) maximumLengthWord = 17;
        if (gridXLength < maximumLengthWord)
        {
            maximumLengthWord = gridXLength;
            if (gridYLength < maximumLengthWord)
            {
                maximumLengthWord = gridYLength;
            }
        }

        // sum weights.Value for 'totalWeight' from start as this wont change
        foreach (KeyValuePair<string, uint> weight in weights)
        {
            totalWeight = totalWeight + weight.Value;
        }

        // setup grid and tiles
        grid = new GameGrid() { dx = gridXLength, dy = gridYLength };
        grid.init();
        grid.directional = true;

        isInitialised = true;

        StartCoroutine(PauseForOptionsMenu());
    }

    // recursive populate 'unfoundWords' list, called in the Initialise()
    void PopulateInitialWords(int len, int numberOfWordsCount, bool basedOnSameAnagram)
    {
        // create a random string to the lenth 'len' we want
        string s = "";
        for (int i = 0; i < len; i++)
        {
            if (Random.value < 0.7) s = s + GetRandomLetter(totalWeight, true);
            else s = s + GetRandomLetter(0, false);
        }

        // search trie with string 's' and store result with..
        // s = string, anagram = true, exactCompare = false, storeWords = true, lengthOfStoredWords = len, debug = false
        bool success = trie.SearchString(s, true, false, true, len, false);

        if (trie.lastStoredWords.Count < len) success = false;

        if (success)
        {
            // grab the amount 'numberOfWordsCount' we need at random if it's not already in the List 'unfoundWords'
            if (basedOnSameAnagram)
            {
                for (int i = 0; i < numberOfWordsCount; i++)
                {
                    int randIndex = Random.Range(0, trie.lastStoredWords.Count - 1);
                    if (!unfoundWords.Contains(trie.lastStoredWords[randIndex])) unfoundWords.Add(trie.lastStoredWords[randIndex]);
                    // if the random number made the controller add the same word twice or more, go mental
                    else
                    {
                        for (int a = 0; a < numberOfWordsCount - i; a++)
                        {
                            if (!unfoundWords.Contains(trie.lastStoredWords[a])) unfoundWords.Add(trie.lastStoredWords[a]);
                        }
                    }
                }
            }
            // do the same but pick at random for 1 word, not the same anagram for all words
            else
            {
                PopulateUnfoundWordsList(len, numberOfWordsCount, basedOnSameAnagram);
            }
        }
        // recursion
        else
        {
            PopulateInitialWords(len, numberOfWordsCount, false);
        }
    }

    // populate unfound words with random word from anagram, and check to see if not already in list, if so recurse
    void PopulateUnfoundWordsList(int len, int numberOfWordsCount, bool basedOnSameAnagram)
    {
        int randIndex = Random.Range(0, trie.lastStoredWords.Count);
        if (unfoundWords.Contains(trie.lastStoredWords[randIndex]))
        {
            PopulateInitialWords(len, numberOfWordsCount, basedOnSameAnagram);
        }
        else unfoundWords.Add(trie.lastStoredWords[randIndex]);
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

    // place words in 'unfoundWords' into the grid at random start position while checking if it would be legal
    void InsertWordToGrid(string word)
    {
        List<int[]> precalculatedRoutes = new List<int[]>();
        precalculatedRoutes = PrecalculateRoutes(word, gridXLength, gridYLength);
        int rand = Random.Range(0, precalculatedRoutes.Count);

        // it's possible that there may not be any legal moves left for this word, restart from scratch to be safe
        if (precalculatedRoutes.Count == 0) Restart();

        // all checks done: choosing a random path, populate each part of the string for each path in the grid we took
        for (int i = 0; i < precalculatedRoutes[rand].Length; i++)
        {
            grid.PopulateBin(precalculatedRoutes[rand][i], word[i].ToString());
        }
        debugPlacedWords++;
    }

    // with a word, this function will check all legal possible paths to place the word inside the grid 
    // as this is called word by word, it will also check by char, if it can put it's string over another string
    List<int[]> PrecalculateRoutes(string word, int gridX, int gridY)
    {
        List<int[]> successfulRoutes = new List<int[]>();
        int wordLength = word.Length;
        int positionsCount = gridX * gridY;
        for (int i = 0; i < positionsCount; i++)
        {
            grid.AddToPath(i);
            int legalsCount = grid.legals.Count;
            for (int a = 0; a < legalsCount; a++)
            {
                // tidy up for new legal iteration
                int arrayCounter = 0;
                int[] routeTaken = new int[word.Length];
                bool success = true;
                grid.FinishPath();

                routeTaken[arrayCounter] = i;
                grid.AddToPath(i);
                // check if we can actually place part of the string here based on if char is != default string, or a char that matches
                if (grid.bins[grid.GetPathEnd()] != defaultString)
                {
                    if (grid.bins[grid.GetPathEnd()] != word[arrayCounter].ToString()) success = false;
                }
                arrayCounter++;

                routeTaken[arrayCounter] = grid.legals[a];
                grid.AddToPath(grid.legals[a]);
                if (grid.bins[grid.GetPathEnd()] != defaultString)
                {
                    if (grid.bins[grid.GetPathEnd()] != word[arrayCounter].ToString()) success = false;
                }
                arrayCounter++;

                // only 1 possible legal move starts to kick in
                for (int b = 0; b < wordLength - 2; b++)
                {
                    // before we do the rest of the loop, are we at an edge with no legal moves? set success to false, make sure to call FINISHPATH() not CLEARPATH() or there will be errors (direction will not be reset)
                    if (grid.legals.Count == 0)
                    {
                        success = false;
                        break;
                    }

                    // now just keep going to the only legal move
                    routeTaken[arrayCounter] = grid.legals[0];
                    grid.AddToPath(grid.legals[0]);
                    if (grid.bins[grid.GetPathEnd()] != defaultString)
                    {
                        if (grid.bins[grid.GetPathEnd()] != word[arrayCounter].ToString()) success = false;
                    }
                    arrayCounter++;

                    // check again if we are at edge
                    if (grid.legals.Count == 0)
                    {
                        success = false;
                        break;
                    }
                }
                if (success)
                {
                    grid.FinishPath();
                    successfulRoutes.Add(routeTaken);
                }
                else
                {
                    grid.FinishPath();
                }
            }
        }
        return successfulRoutes;
    }

    // add a word from the anagram levels dictionary (hand picked common words)
    private void AddWord (int Len)
    {
        bool looking = true;
        while (looking)
        {
            string me = AL.GetWord(Len);
            if (!unfoundWords.Contains(me))
            {
                unfoundWords.Add(me);
                looking = false;
            }
        }
    }

    // triggers recursive function, places words into the grid where it can, then spawns the actual cube meshes according to strings/random weighted letters
    public void PlaceCubesInGrid()
    {
        debugPlacedWords = 0;

        // populate with dummy data so the grid can check 'legals' for placing words in
        for (int i = 0; i < gridXLength * gridYLength; i++)
        {
            grid.PopulateBin(i, defaultString);
        }

        // HISTORIC CODE _ RETAINED FOR POSSIBLE REVERT
        // populate with initial words to be placed 'unfoundWords' List
        // for (int i = 0; i < fourLetterWordsCount; i++) { PopulateInitialWords(4, fourLetterWordsCount, false); }
        // for (int i = 0; i < fiveLetterWordsCount; i++) { PopulateInitialWords(5, fiveLetterWordsCount, false); }
        // for (int i = 0; i < sixLetterWordsCount; i++) { PopulateInitialWords(6, sixLetterWordsCount, false); }
        // for (int i = 0; i < sevenLetterWordsCount; i++) { PopulateInitialWords(7, sevenLetterWordsCount, false); }
        // for (int i = 0; i < eightLetterWordsCount; i++) { PopulateInitialWords(8, eightLetterWordsCount, false); }

        for (int i = 0; i < fourLetterWordsCount; i++)
        {
            if (Random.Range(0f,1f) >= 0.3f) AddWord(4);
            else
            {
                PopulateInitialWords(4, 1, false);
            }
        }
        for (int i = 0; i < fiveLetterWordsCount; i++)
        {
            if (Random.Range(0f, 1f) >= 0.3f) AddWord(5);
            else
            {
                PopulateInitialWords(5, 1, false);
            }
        }
        for (int i = 0; i < sixLetterWordsCount; i++)
        {
            if (Random.Range(0f, 1f) >= 0.3f) AddWord(6);
            else
            {
                PopulateInitialWords(6, 1, false);
            }
        }
        for (int i = 0; i < sevenLetterWordsCount; i++)
        {
            if (Random.Range(0f, 1f) >= 0.3f) AddWord(7);
            else
            {
                PopulateInitialWords(7, 1, false);
            }
        }
        for (int i = 0; i < eightLetterWordsCount; i++)
        {
            if (Random.Range(0f, 1f) >= 0.3f) AddWord(8);
            else
            {
                PopulateInitialWords(8, 1, false);
            }
        }

        // start placing words in the grid and check their legality (possible recursion for each word)
        foreach (string word in unfoundWords)
        {
            InsertWordToGrid(word);
        }

        // bosh in some random weighted letters where there are only 'defaultLetters' left
        for (int i = 0; i < gridXLength * gridYLength; i++)
        {
            if (grid.bins[i] == defaultString) grid.PopulateBin(i, GetRandomLetter(totalWeight, true));
        }

        // now pop the cubes in
        int count = 0;
        for (int z = gridYLength; z > 0; z--)
        {
            for (int x = 0; x < gridXLength; x++)
            {
                GameObject dice = gameController.assets.SpawnDice(grid.bins[count], new Vector3(diceHolder.transform.position.x + x, diceHolder.transform.position.y, diceHolder.transform.position.z + z));
                dice.transform.parent = diceHolder.transform;
                ConDice diceCon = dice.GetComponent<ConDice>();
                diceCon.ID = count;
                diceCon.myGrid = grid;
                instancedDice[count] = dice;
                count++;
            }
        }
        diceHolder.transform.localRotation = transform.localRotation;
    }

    // used for spawning the initial '?' cubes with no smart behaviour attached
    public void PlaceEmptyCubes()
    {
        int count = 0;
        for (int z = gridYLength; z > 0; z--)
        {
            for (int x = 0; x < gridXLength; x++)
            {
                GameObject dice = gameController.assets.SpawnDice("?", new Vector3(diceHolder.transform.position.x + x, diceHolder.transform.position.y, diceHolder.transform.position.z + z));
                dice.transform.parent = diceHolder.transform;
                instancedDice[count] = dice;
                count++;
            }
        }
        diceHolder.transform.localRotation = transform.localRotation;
    }

    // set variables to what's in the player preferences
    void LoadPlayerPreferences()
    {
        gridXLength = gameController.player.WordSearchSize;
        gridYLength = gameController.player.WordSearchSize;
        minimumLengthWord = gameController.player.WordSearchMinimumLengthWord;
        maximumLengthWord = gameController.player.WordSearchMaximumLengthWord;
        fourLetterWordsCount = gameController.player.WordSearchFourLetterWordsCount;
        fiveLetterWordsCount = gameController.player.WordSearchFiveLetterWordsCount;
        sixLetterWordsCount = gameController.player.WordSearchSixLetterWordsCount;
        sevenLetterWordsCount = gameController.player.WordSearchSevenLetterWordsCount;
        eightLetterWordsCount = gameController.player.WordSearchEightLetterWordsCount;
        gameTime = gameController.player.WordSearchGameLength;
        bestTime = gameController.player.WordSearchBestTime;
        if (bestTime == 0) bestTime = 599;
        averageTime = gameController.player.WordSearchAverageTime;
        worstTime = gameController.player.WordSearchWorstTime;
        timesCompleted = gameController.player.WordSearchTimesCompleted;
        timesQuit = gameController.player.WordSearchTimesQuit;

    }

    string TimeToString(int time)
    {
        int sec = time % 60;
        int min = (time - sec)/60;
        string ret = "";
        if (min < 10) ret += "0";
        ret += min.ToString() + ":";
        if (sec < 10) ret += "0";
        return ret + sec.ToString();
    }

    // configure flashes
    void ConfigureFlashes()
    {
        // found word flash
        f_foundWord = new FlashProTemplate();
        f_foundWord.SingleLerp = false;
        f_foundWord.StartPos = new Vector2(0.85f, 0.2f);
        f_foundWord.MiddlePos = new Vector2(0.85f, 0.4f);
        f_foundWord.FinishPos = new Vector2(0.85f, 1.0f);
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
        f_foundSame.StartPos = new Vector2(0.85f, 0.8f);
        f_foundSame.FinishPos = new Vector2(0.85f, 0.1f);
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
        f_timeNotification.StartPos = new Vector2(0.2f, 0.55f);
        f_timeNotification.MiddlePos = new Vector2(0.15f, 0.75f);
        f_timeNotification.FinishPos = new Vector2(0.1f, 0.9f);
        f_timeNotification.StartWidth = 0.1f;
        f_timeNotification.MiddleWidth = 0.15f;
        f_timeNotification.FinishWidth = 0.1f;
        f_timeNotification.StartAlpha = 0.8f;
        f_timeNotification.MiddleAlpha = 1f;
        f_timeNotification.FinishAlpha = 0;
        f_timeNotification.myMessage1 = "Found a word!";
        f_timeNotification.myMessage2 = "Good/Great/Excellent!";
        f_timeNotification.TextColor1 = Color.yellow;
        f_timeNotification.TextColor2 = Color.green;
        f_timeNotification.Xtween1 = Tween.LinearUp;
        f_timeNotification.Xtween2 = Tween.QuinUp;
        f_timeNotification.AnimTime = 2.5f;
        f_timeNotification.MiddleTimeRatio = .3f;

        // end notification
        f_endNotification = new FlashProTemplate();
        f_endNotification.SingleLerp = false;
        f_endNotification.StartPos = new Vector2(0.8f, 0.55f);
        f_endNotification.MiddlePos = new Vector2(0.85f, 0.75f);
        f_endNotification.FinishPos = new Vector2(0.9f, 0.95f);
        f_endNotification.StartWidth = 0.1f;
        f_endNotification.MiddleWidth = 0.15f;
        f_endNotification.FinishWidth = 0.1f;
        f_endNotification.StartAlpha = 0.8f;
        f_endNotification.MiddleAlpha = 1f;
        f_endNotification.FinishAlpha = 0;
        f_endNotification.myMessage1 = "Found a word!";
        f_endNotification.myMessage2 = "Good/Great/Excellent!";
        f_endNotification.TextColor1 = Color.yellow;
        f_endNotification.TextColor2 = Color.green;
        f_endNotification.Xtween1 = Tween.LinearUp;
        f_endNotification.Xtween2 = Tween.QuinUp;
        f_endNotification.AnimTime = 4f;
        f_endNotification.MiddleTimeRatio = .6f;
    }

    void ConfigureGUI ()
    {
        GameObject obj_times = gameController.FM.AddGUIItem("TIMES: Best: 0  Average: 0", 0.81f, 0.95f, 0.24f, Color.white);
        g_times = obj_times.GetComponent<TextMeshProUGUI>();
        g_times.alpha = 0;
        SetGUI();
    }

    void SetGUI ()
    {
        g_High = TimeToString(bestTime);
        g_mean = TimeToString(averageTime);
        if (averageTime == 0) g_mean = "TBA";

        g_times.text = "TIMES: Best: "+g_High+"  Average: "+ g_mean;
    }

    #endregion


    #region Update Loop and Functions

    // update
    void Update()
    {
        if (!waiting)
        {
            if (!timeUp) if (selecting) grid.GetCurrentPath();

            if (gameController != null)
            {
                CheckGCHoverValue();
                if (!timeUp) InputAndSearch();
            }

            if (foundWords.Count == 10) wordSearchTable.clock.GetComponent<Clock>().StopClock();

            if (timeUp)
            {
                if (!showingRestart)
                {
                    wordSearchTable.RestartSetup();
                    RunEndFlashesAndSaveStats();
                    gameController.SM.PlayMiscSFX((MiscSFX)Random.Range(0, 3));
                    showingRestart = true;
                }
                if (Input.GetMouseButtonDown(0) && gameController.NewHoverOver == 4442) Restart();
            }
        }
        else wordSearchTable.clock.GetComponent<Clock>().SetTime(0f);
    }

    // input and trie search
    void InputAndSearch()
    {
        // hit a tile, mouse down
        if (!selecting)
        {
            if (Input.GetMouseButtonDown(0) && gameController.NewHoverOver != -1)
            {
                selecting = true;
                grid.AddToPath(gameController.NewHoverOver);
            }
        }
        // search string, mouse up
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                selecting = false;
                bool isFound = false;
                string res = grid.GetCurrentPath();
                if (res.Length >= minimumLengthWord)
                {
                    if (trie.SearchString(res, false, true, false, 0, false))
                    {
                        if (foundWords.Count > 0)
                        {
                            if (foundWords.Contains(res))
                            {
                                gameController.FM.CustomFlash(f_foundSame, "Already found " + res.ToUpper());
                                gameController.SM.PlayWordSFX(WordSFX.SameWord);
                            }
                        }
                        // do not use List.Contains(), it will find 'dog' if 'padog' is searched - better to look and find exact string matching
                        for (int i = 0; i < unfoundWords.Count; i++)
                        {
                            if (unfoundWords[i] == res)
                            {
                                Debug.Log("You got " + res);
                                isFound = true;
                                wordSearchTable.unfoundWordObjects[i].ScrubWord(1f);
                                wordSearchTable.unfoundWordObjects[i].UseScrubber(1f);
                                wordSearchTable.foundWordObjects[foundWords.Count].WriteWord(res, 1f);
                                wordSearchTable.foundWordObjects[foundWords.Count].UseChalk(1f);
                                gameController.SM.PlayWordSFX((WordSFX)Random.Range(0, 6));
                                foundWords.Add(res);
                                unfoundWords[i] = "";
                                foreach (int ID in grid.GetCurrentPathIDs())
                                {
                                    instancedDice[ID].GetComponent<ConDice>().ChangeDiceColorAdditive(gameController.ColorBodyHighlight);
                                }
                                switch (res.Length)
                                {
                                    case 4:
                                        {
                                            f_foundWord.TextColor2 = Color.green;
                                            gameController.FM.CustomFlash(f_foundWord, "4 letter word", "Good!");
                                            break;
                                        }
                                    case 5:
                                        {
                                            f_foundWord.TextColor2 = Color.yellow;
                                            gameController.FM.CustomFlash(f_foundWord, "5 letter word", "Great!");
                                            break;
                                        }
                                    case 6:
                                        {
                                            f_foundWord.TextColor2 = Color.red;
                                            gameController.FM.CustomFlash(f_foundWord, "6 letter word", "Excellent!");
                                            break;
                                        }
                                }
                                if (foundWords.Count == 5)
                                {
                                    if ((averageTime / 2) > wordSearchTable.clock.GetComponent<Clock>().time)
                                    {
                                        gameController.FM.CustomFlash(f_timeNotification, "Great Time!", "Half way!");
                                    }
                                    else { gameController.FM.CustomFlash(f_timeNotification, "Keep Going!", "Half way!"); }
                                }
                                break;
                            }
                        }
                        if (!isFound) Debug.Log("Sorry, " + res + " is not on the list!");
                        grid.FinishPath();
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


    #region Restart / TidyUp / SaveStats

    // restarting fresh / initialising functions
    public void Restart()
    {
        wordSearchTable.IngameSetup();
        grid.init();
        gameController.FM.KillAllFlashes();
        gameController.FM.KillStaticGUIs();
        gameController.SM.KillSFX();

        diceHolder.transform.localRotation = Quaternion.identity;
        ClearCubesAndBoard();
        PlaceEmptyCubes();

        gameController.SM.PlayTileSFX((TileSFX)Random.Range(13, 15));

        LoadPlayerPreferences();
        ConfigureGUI();

        gameController.Fire_Start_Flash();
        StartCoroutine(RealStart(4.5f));
    }

    // clear previous words on the board, delete all dice, change tabledice to view when around table
    public void TidyUp()
    {
        foundWords.Clear();
        unfoundWords.Clear();
        gameController.FM.KillAllFlashes();
        gameController.FM.KillStaticGUIs();
        gameController.SM.KillSFX();
        gameController.FM.KillStaticGUIs();
        foreach (ConDice dice in diceHolder.GetComponentsInChildren<ConDice>())
        {
            Destroy(dice.gameObject);
        }
        ClearCubesAndBoard();
        wordSearchTable.StartSetup();
    }

    // essentially same as TidyUp, though we do not change any prefabs
    public void ClearCubesAndBoard()
    {
        foundWords.Clear();
        unfoundWords.Clear();
        foreach (ConDice dice in diceHolder.GetComponentsInChildren<ConDice>())
        {
            Destroy(dice.gameObject);
        }
    }

    // populate chalk board with words (animation)
    public void RunInitialBoardAnimations()
    {
        for (int i = 0; i < unfoundWords.Count; i++)
        {
            wordSearchTable.unfoundWordObjects[i].Init();
            wordSearchTable.unfoundWordObjects[i].WriteWord(unfoundWords[i], 2f);
            wordSearchTable.foundWordObjects[i].Init();
            wordSearchTable.foundWordObjects[i].GetComponent<Text>().text = unfoundWords[i];
            wordSearchTable.foundWordObjects[i].ScrubWord(2f);
        }
    }

    void SaveStats()
    {
        gameController.player.WordSearchBestTime = bestTime;
        gameController.player.WordSearchAverageTime = averageTime;
        gameController.player.WordSearchWorstTime = worstTime;
        gameController.player.WordSearchTimesCompleted = timesCompleted;
        gameController.player.WordSearchTimesQuit = timesQuit;
        gameController.SaveStats();
    }

    public void ResetStats()
    {
        gridXLength = 10;
        gridYLength = 10;
        minimumLengthWord = 4;
        maximumLengthWord = 8;
        fourLetterWordsCount = 4;
        fiveLetterWordsCount = 4;
        sixLetterWordsCount = 2;
        sevenLetterWordsCount = 0;
        eightLetterWordsCount = 0;
        gameTime = 599;
        bestTime = 599;
        averageTime = 0;
        worstTime = 599;
        timesCompleted = 0;
        timesQuit = 0;
    }

    void RunEndFlashesAndSaveStats()
    {
        float flashDelay = 0;
        float endtime = wordSearchTable.clock.GetComponent<Clock>().time;
        string timeStr = TimeToString((int)endtime);

        if ((int)endtime < bestTime || bestTime == 0)
        {
            bestTime = (int)endtime;
            flashDelay++;
            gameController.FM.CustomFlash(f_endNotification, "New Best Time!", timeStr, flashDelay + .5f);
            gameController.SM.PlayMiscSFX(MiscSFX.SwishQuick, flashDelay + .5f);
            gameController.SM.PlayMiscSFX((MiscSFX)Random.Range(3, 9), (flashDelay + .5f) + (f_endNotification.AnimTime * f_endNotification.MiddleTimeRatio));
        }

        averageTime = (int)(((averageTime * timesCompleted) + (int)endtime) / (timesCompleted + 1));
        if (endtime < averageTime || (int)endtime == averageTime)
        {
            flashDelay++;
            gameController.FM.CustomFlash(f_endNotification, "New Best Average Time!", TimeToString(averageTime), flashDelay + .5f);
            gameController.SM.PlayMiscSFX(MiscSFX.SwishQuick, flashDelay + .5f);
            gameController.SM.PlayMiscSFX((MiscSFX)Random.Range(3, 9), (flashDelay + .5f) + (f_endNotification.AnimTime * f_endNotification.MiddleTimeRatio));
        }

        timesCompleted++;
        SaveStats();
        LoadPlayerPreferences();
        SetGUI();
    }

    public void PlayPauseForOptionsMenu() { StartCoroutine(PauseForOptionsMenu()); }
    IEnumerator PauseForOptionsMenu()
    {
        wordSearchTable.IngameSetup();
        grid.init();
        diceHolder.transform.localRotation = Quaternion.identity;
        PlaceEmptyCubes();
        ConfigureGUI();
        waiting = true;
        bool w = true;
        while (w)
        {
            if (!gameController.UIController.isWSOpen)
            {
                waiting = false;
                w = false;
            }
            yield return null;
        }
        Restart(); // which in turn will call "RealStart" after 4.5 seconds
    }

    IEnumerator RealStart(float delay)
    {
        waiting = true;
        yield return new WaitForSeconds(delay);
        g_times.alpha = 1;
        waiting = false;
        wordSearchTable.clock.GetComponent<Clock>().StartClock(0, gameTime);
        showingRestart = false;
        ClearCubesAndBoard();
        diceHolder.transform.localRotation = Quaternion.identity;
        PlaceCubesInGrid();
        RunInitialBoardAnimations();
        gameController.SM.PlayTileSFX((TileSFX)Random.Range(13, 15));
        wordSearchTable.IngameSetup();
    }

    #endregion
}