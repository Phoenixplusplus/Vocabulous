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
    public int minimumLengthWord = 3;
    public int maximumLengthWord = 17;
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
    [SerializeField]
    private bool selecting = false;
    private float totalWeight;

    void Start()
    {
        /* gc */
        gameController = GC.Instance;

        /* sum weights.Value for 'totalWeight' from start as this wont change */
        float totalWeight = 0f;
        foreach (KeyValuePair<string, float> weight in weights)
        {
            totalWeight = totalWeight + weight.Value;
        }

        /* setup grid and tiles */
        grid = new GameGrid() { dx = gridXLength, dy = gridYLength };
        grid.init();

        for (int i = 0; i < gridXLength * gridYLength; i++)
        {
            grid.PopulateBin(i, GetRandomLetter(totalWeight));
        }

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
    }

    void Update()
    {
        if (selecting) grid.GetCurrentPath();

        CheckGCHoverValue();
        InputAndSearch();
    }

    /* return key in 'weights' dictionary based on accumulated weight > random number */
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

    void InputAndSearch()
    {
        if (!selecting)
        {
            /* hit a tile, mouse down */
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
                    if (trie.SearchString(res, false, true, false))
                    {
                        if (foundWords.Contains(res))
                        {
                            Debug.Log("You already got that one!");
                        }
                        else
                        {
                            Debug.Log("You got " + res);
                            foundWords.Add(res);
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
