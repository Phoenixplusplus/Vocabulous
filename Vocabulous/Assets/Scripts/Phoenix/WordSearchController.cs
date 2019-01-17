/* random letter based on weighting: http://pi.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html?fbclid=IwAR3gpj-HzjT6s2GQ2wBlYq4eZbdJ7uA6SjFhSrcDYb-CXHBtpaB3cdCjyr0 */

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WordSearchController : MonoBehaviour
{
    private GC gameController;
    private GameGrid grid;
    public GameObject tiles;
    public GameObject OverlayPrefab;
    public TrieTest trie;
    public int gridXLength = 20;
    public int gridYLength = 20;
    [SerializeField]
    private int minimumLengthWord = 3;
    [SerializeField]
    private int maximumLengthWord = 8;
    public int numberOfWords = 15;
    //                                  will longer length/default these off some research when less tierd
    public int threeLetterWordsCount = 2;
    public int fourLetterWordsCount = 3;
    public int fiveLetterWordsCount = 4;
    public int sixLetterWordsCount = 2;
    public int sevenLetterWordsCount = 2;
    public int eightLetterWordsCount = 2;
    //
    public List<string> foundWords = new List<string>();
    public List<string> unfoundWords = new List<string>();
    public Dictionary<string, float> weights = new Dictionary<string, float>
    {
        { "a", 8.12f }, { "b", 1.49f }, { "c", 2.71f }, { "d", 4.32f }, { "e", 12.02f },{ "f", 2.30f },
        { "g", 2.03f }, { "h", 5.92f }, { "i", 7.31f }, { "j", 0.10f }, { "k", 0.69f },{ "l", 3.98f },
        { "m", 2.61f }, { "n", 6.95f }, { "o", 7.68f }, { "p", 1.82f }, { "q", 0.11f },{ "r", 6.02f },
        { "s", 6.82f }, { "t", 9.10f }, { "u", 2.88f }, { "v", 1.11f }, { "w", 2.09f },{ "x", 0.17f },
        { "y", 2.11f }, { "z", 0.07f }
    };
    private float totalWeight = 0f;
    [SerializeField]
    private bool selecting = false;

    void Start()
    {

        /* gc */
        gameController = GC.Instance;

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
        foreach (KeyValuePair<string, float> weight in weights)
        {
            totalWeight = totalWeight + weight.Value;
        }

        /* setup grid and tiles */
        grid = new GameGrid() { dx = gridXLength, dy = gridYLength };
        grid.init();

        int count = 0;
        for (int y = gridYLength; y > 0; y--)
        {
            for (int x = 0; x < gridXLength; x++)
            {
                GameObject tile = Instantiate(OverlayPrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.transform.parent = tiles.transform;
                Tile_Controlller tilecon = tile.GetComponent<Tile_Controlller>();
                tilecon.setID(count);
                count++;
                tilecon.myGrid = grid;
                tilecon.SetVisible(true);
            }
        }

        /* populate with dummy data so the grid can check 'legals' for placing words in */
        for (int i = 0; i < gridXLength * gridYLength; i++)
        {
            grid.PopulateBin(i, "a");
        }

        /* populate with initial words to be placed 'unfoundWords' List */ // NTS: shorten this up, maybe convert to array and add a loop
        PopulateInitialWords(3, threeLetterWordsCount);
        PopulateInitialWords(4, fourLetterWordsCount);
        PopulateInitialWords(5, fiveLetterWordsCount);
        PopulateInitialWords(6, sixLetterWordsCount);
        PopulateInitialWords(7, sevenLetterWordsCount);
        PopulateInitialWords(8, eightLetterWordsCount);

        /* bosh in some random weighted letters */
        //for (int i = 0; i < gridXLength * gridYLength; i++)
        //{
        //    grid.PopulateBin(i, GetRandomLetter(totalWeight));
        //}
    }

    void Update()
    {
        if (selecting) grid.GetCurrentPath();

        CheckGCHoverValue();
        InputAndSearch();
    }

    /* recursive populate 'unfoundWords' list */
    void PopulateInitialWords(int len, int numberOfWordsCount)
    {
        Debug.Log("Populating initial words..");
        /* create a random string to the lenth 'len' we want */
        string s = "";
        for (int i = 0; i < len; i++)
        {
            s = s + GetRandomLetter(totalWeight);
        }
        /* search trie with string 's' and store result with..
         * s = string, anagram = true, exactCompare = false, storeWords = true, lengthOfStoredWords = len, debug = false */
        bool success = trie.SearchString(s, true, false, true, len, false);
        /* recursion */
        if (!success)
        {
            Debug.Log("Failed to populate words..");
            PopulateInitialWords(len, numberOfWordsCount);
        }
        if (trie.lastStoredWords.Count < len)
        {
            Debug.Log("Failed to populate words..");
            PopulateInitialWords(len, numberOfWordsCount);
        }
        /* grab the amount 'numberOfWordsCount' we need at random if it's not already in the List 'unfoundWords' */
        else
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
    }

    /* based on accumulated weight > random number, return string 'Key' in Dictionary 'weights' */
    string GetRandomLetter(float tw)
    {
        float rand = Random.Range(0f, tw);
        float accumWeight = 0;
        int count;

        for (count = 0; count < weights.Count; count++)
        {
            accumWeight = accumWeight + weights.Values.ElementAt(count);
            if (accumWeight > rand) break;
        }
        return weights.ElementAt(count).Key;
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
                string res = grid.FinishPath();
                if (res.Length >= minimumLengthWord)
                {
                    if (trie.SearchString(res, false, true, false, 0, false))
                    {
                        if (foundWords.Contains(res))
                        {
                            Debug.Log("You already got that one!");
                        }
                        else
                        {
                            Debug.Log("You got " + res);
                            foundWords.Add(res);
                            unfoundWords.Remove(res);
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
}
