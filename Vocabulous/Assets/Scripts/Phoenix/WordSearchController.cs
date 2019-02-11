﻿/* random letter based on weighting: http://pi.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html?fbclid=IwAR3gpj-HzjT6s2GQ2wBlYq4eZbdJ7uA6SjFhSrcDYb-CXHBtpaB3cdCjyr0 */

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSearchController : MonoBehaviour
{
    GC gameController;
    GameGrid grid;
    public GameObject diceHolder;
    public GameObject OverlayPrefab;
    public WordSearchTable wordSearchTable;
    public TrieTest trie;
    int numberOfWords = 15;
    // set by player prefs from GC
    int gridXLength;
    int gridYLength;
    int minimumLengthWord;
    int maximumLengthWord;
    int fourLetterWordsCount;
    int fiveLetterWordsCount;
    int sixLetterWordsCount;
    int sevenLetterWordsCount;
    int eightLetterWordsCount;
    int gameTime;
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



    #region StartUp
    /* main initialise function, will call subsequent StartUp functions */
    public void Initialise()
    {
        /* gamecontroller and initialising variables */
        gameController = GC.Instance;
        trie = gameController.phoenixTrie;
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

        /* hide/unhide table prefabs */
        wordSearchTable.IngameSetup();
        wordSearchTable.clock.GetComponent<Clock>().StartClock(0, 599);

        /* check maximumWordLength against maximum word in dictionary and bounds of grid so everything fits/is a legal word */
        if (maximumLengthWord > 17) maximumLengthWord = 17;
        if (gridXLength < maximumLengthWord)
        {
            maximumLengthWord = gridXLength;
            if (gridYLength < maximumLengthWord)
            {
                maximumLengthWord = gridYLength;
            }
        }

        /* sum weights.Value for 'totalWeight' from start as this wont change */
        foreach (KeyValuePair<string, uint> weight in weights)
        {
            totalWeight = totalWeight + weight.Value;
        }

        /* setup grid and tiles */
        grid = new GameGrid() { dx = gridXLength, dy = gridYLength };
        grid.init();
        grid.directional = true;

        /* place cubes in grid */
        PlaceCubesInGrid();

        /* do initial board animations */
        RunInitialBoardAnimations();

        isInitialised = true;
    }

    /* recursive populate 'unfoundWords' list, called in the Initialise() */
    void PopulateInitialWords(int len, int numberOfWordsCount, bool basedOnSameAnagram)
    {
        Debug.Log("Populating initial words..");
        /* create a random string to the lenth 'len' we want */
        string s = "";
        for (int i = 0; i < len; i++)
        {
            if (Random.value < 0.7) s = s + GetRandomLetter(totalWeight, true);
            else s = s + GetRandomLetter(0, false);
        }
        /* search trie with string 's' and store result with..
         * s = string, anagram = true, exactCompare = false, storeWords = true, lengthOfStoredWords = len, debug = false */
        bool success = trie.SearchString(s, true, false, true, len, false);

        if (trie.lastStoredWords.Count < len) success = false;

        if (success)
        {
            /* grab the amount 'numberOfWordsCount' we need at random if it's not already in the List 'unfoundWords' */
            if (basedOnSameAnagram)
            {
                for (int i = 0; i < numberOfWordsCount; i++)
                {
                    int randIndex = Random.Range(0, trie.lastStoredWords.Count - 1);
                    if (!unfoundWords.Contains(trie.lastStoredWords[randIndex])) unfoundWords.Add(trie.lastStoredWords[randIndex]);
                    /* if the random number made the controller add the same word twice or more, go mental */
                    else
                    {
                        for (int a = 0; a < numberOfWordsCount - i; a++)
                        {
                            if (!unfoundWords.Contains(trie.lastStoredWords[a])) unfoundWords.Add(trie.lastStoredWords[a]);
                        }
                    }
                }
            }
            /* do the same but pick at random for 1 word, not the same anagram for all words */
            else
            {
                PopulateUnfoundWordsList(len, numberOfWordsCount, basedOnSameAnagram);
            }
        }
        /* recursion */
        else
        {
            Debug.Log("Failed to populate words, repopulating");
            PopulateInitialWords(len, numberOfWordsCount, false);
        }
    }

    /* populate unfound words with random word from anagram, and check to see if not already in list, if so recurse */
    void PopulateUnfoundWordsList(int len, int numberOfWordsCount, bool basedOnSameAnagram)
    {
        int randIndex = Random.Range(0, trie.lastStoredWords.Count);
        if (unfoundWords.Contains(trie.lastStoredWords[randIndex]))
        {
            PopulateInitialWords(len, numberOfWordsCount, basedOnSameAnagram);
        }
        else unfoundWords.Add(trie.lastStoredWords[randIndex]);
    }

    /* based on accumulated weight > random number, return string 'Key' in Dictionary 'weights' */
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

    /* place words in 'unfoundWords' into the grid at random start position while checking if it would be legal */
    void InsertWordToGrid(string word)
    {
        Debug.Log("-- Function Start with: " + word + " --");

        List<int[]> precalculatedRoutes = new List<int[]>();
        precalculatedRoutes = PrecalculateRoutes(word, gridXLength, gridYLength);
        int rand = Random.Range(0, precalculatedRoutes.Count);

        // it's possible that there may not be any legal moves left for this word, restart from scratch to be safe
        if (precalculatedRoutes.Count == 0) { Debug.Log("There were no legal paths for " + word + " restarting.."); Restart(); }

        Debug.Log(word + " has " + precalculatedRoutes.Count + " legal routes");

        /* all checks done: choosing a random path, populate each part of the string for each path in the grid we took */
        for (int i = 0; i < precalculatedRoutes[rand].Length; i++)
        {
            grid.PopulateBin(precalculatedRoutes[rand][i], word[i].ToString());
        }
        Debug.Log("Placed word: " + word);
        debugPlacedWords++;
    }

    /* with a word, this function will check all legal possible paths to place the word inside the grid 
     * as this is called word by word, it will also check by char, if it can put it's string over another string */
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

    /* triggers recursive function, places words into the grid where it can, then spawns the actual cube meshes according to strings/random weighted letters */
    public void PlaceCubesInGrid()
    {
        debugPlacedWords = 0;

        /* populate with dummy data so the grid can check 'legals' for placing words in */
        for (int i = 0; i < gridXLength * gridYLength; i++)
        {
            grid.PopulateBin(i, defaultString);
        }

        /* populate with initial words to be placed 'unfoundWords' List */
        for (int i = 0; i < fourLetterWordsCount; i++) { PopulateInitialWords(4, fourLetterWordsCount, false); }
        for (int i = 0; i < fiveLetterWordsCount; i++) { PopulateInitialWords(5, fiveLetterWordsCount, false); }
        for (int i = 0; i < sixLetterWordsCount; i++) { PopulateInitialWords(6, sixLetterWordsCount, false); }
        for (int i = 0; i < sevenLetterWordsCount; i++) { PopulateInitialWords(7, sevenLetterWordsCount, false); }
        for (int i = 0; i < eightLetterWordsCount; i++) { PopulateInitialWords(8, eightLetterWordsCount, false); }

        /* start placing words in the grid and check their legality (possible recursion for each word) */
        foreach (string word in unfoundWords)
        {
            InsertWordToGrid(word);
        }
        Debug.Log("Finished populating words, successfully placed " + debugPlacedWords + " words out of " + unfoundWords.Count + " on the unfoundWords list");

        /* bosh in some random weighted letters where there are only 'defaultLetters' left */
        for (int i = 0; i < gridXLength * gridYLength; i++)
        {
            if (grid.bins[i] == defaultString) grid.PopulateBin(i, GetRandomLetter(totalWeight, true));
        }

        /* now pop the cubes in */
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
                count++;
            }
        }
        diceHolder.transform.localRotation = transform.localRotation;
    }
    #endregion



    #region Update Loop and Functions
    /* update */
    void Update()
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
                ClearCubesAndBoard();
                showingRestart = true;
            }
            if (Input.GetMouseButtonDown(0) && gameController.NewHoverOver == 4442) Restart();
        }

        // DEBUG - REMEMBER TO TAKE THIS OUT FOR BUILD
        if (Input.GetKeyDown(KeyCode.L)) Restart();
    }

    /* input and trie search */
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
                bool isFound = false;
                string res = grid.GetCurrentPath();
                if (res.Length >= minimumLengthWord)
                {
                    if (trie.SearchString(res, false, true, false, 0, false))
                    {
                        /* do not use List.Contains(), it will find 'dog' if 'padog' is searched - better to look and find exact string matching */
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
                                foundWords.Add(res);
                                unfoundWords[i] = "";
                                grid.HighlightCurrentPath();
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
            }
        }
    }

    /* middle man between GC and this class' grid */
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



    #region Restart and TidyUp
    /* restarting fresh / initialising functions*/
    public void Restart()
    {
        wordSearchTable.IngameSetup();
        grid.init();
        ClearCubesAndBoard();
        wordSearchTable.clock.GetComponent<Clock>().StartClock(0, 599);
        diceHolder.transform.localRotation = Quaternion.identity;
        PlaceCubesInGrid();
        RunInitialBoardAnimations();
        showingRestart = false;
    }

    /* clear previous words on the board, delete all dice, change tabledice to view when around table */
    public void TidyUp()
    {
        foundWords.Clear();
        unfoundWords.Clear();
        foreach (ConDice dice in diceHolder.GetComponentsInChildren<ConDice>())
        {
            Destroy(dice.gameObject);
        }
        wordSearchTable.StartSetup();
    }

    /* essentially same as TidyUp, though we do not change any prefabs */
    public void ClearCubesAndBoard()
    {
        foundWords.Clear();
        unfoundWords.Clear();
        foreach (ConDice dice in diceHolder.GetComponentsInChildren<ConDice>())
        {
            Destroy(dice.gameObject);
        }
    }

    /* populate chalk board with words (animation) */
    public void RunInitialBoardAnimations()
    {
        for (int i = 0; i < unfoundWords.Count; i++)
        {
            wordSearchTable.unfoundWordObjects[i].WriteWord(unfoundWords[i], 2f);
            wordSearchTable.foundWordObjects[i].GetComponent<Text>().text = unfoundWords[i];
            wordSearchTable.foundWordObjects[i].ScrubWord(2f);
        }
    }
    #endregion
}