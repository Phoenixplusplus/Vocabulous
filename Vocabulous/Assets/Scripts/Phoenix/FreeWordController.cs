/* random letter based on weighting: http://pi.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html?fbclid=IwAR3gpj-HzjT6s2GQ2wBlYq4eZbdJ7uA6SjFhSrcDYb-CXHBtpaB3cdCjyr0 */

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeWordController : MonoBehaviour
{
    GC gameController;
    GameGrid grid;
    public GameObject tileHolder;
    public FreeWordTable freeWordTable;
    public TrieTest trie;
    int numberOfWords = 15;
    int minimumLengthWord = 2;
    int gridXLength = 10;
    int gridYLength = 10;
    int gameTime = 120;
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



    #region StartUp
    // main initialise function, will call subsequent StartUp functions
    public void Initialise()
    {
        // gamecontroller and initialising variables
        gameController = GC.Instance;
        trie = gameController.phoenixTrie;

        freeWordTable.IngameSetup();
        freeWordTable.clock.GetComponent<Clock>().StartClock(gameTime, 0);

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
        }

        if (timeUp)
        {
            if (!showingRestart)
            {
                freeWordTable.RestartSetup();
                ClearTilesAndFoundList();
                showingRestart = true;
            }
            if (Input.GetMouseButtonDown(0) && gameController.NewHoverOver == 5552) Restart();
        }

        // DEBUG - REMEMBER TO TAKE THIS OUT FOR BUILD
        if (Input.GetKeyDown(KeyCode.L)) Restart();
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
                            grid.FinishPath();
                        }
                        else
                        {
                            Debug.Log("You already found " + res + "!");
                            grid.FinishPath();
                        }
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



    #region Restart and TidyUp
    // restarting fresh / initialising functions
    public void Restart()
    {
        freeWordTable.IngameSetup();
        grid.init();
        ClearTilesAndFoundList();
        freeWordTable.clock.GetComponent<Clock>().StartClock(gameTime, 0);
        tileHolder.transform.localRotation = Quaternion.identity;
        PlaceTilesInGrid();
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
        freeWordTable.StartSetup();
    }

    // essentially same as TidyUp, though we do not change any prefabs
    public void ClearTilesAndFoundList()
    {
        foundWords.Clear();
        foreach (Con_Tile2 tile in tileHolder.GetComponentsInChildren<Con_Tile2>())
        {
            Destroy(tile.gameObject);
        }
    }
    #endregion
}