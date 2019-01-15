using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Game_Controller : MonoBehaviour
{
    public GameObject OverlayPrefab;
    private GameGrid grid;
    private MaxTrie trie;
    public GameObject tiles;
    public int HoverOver = -1;
    public bool Selecting = false;
    public string CurrentWord = "";
    public List<string> FoundWords = new List<string>();
    public List<int> GridLegals;
    public int currDirection;
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
        trie = GetComponent<MaxTrie>();
        grid = new GameGrid() { dx = 4, dy = 4 };
        grid.init();
        GridLegals = grid.legals;
        currDirection = grid.currDir;
        PopulateGrid();
        Debug.Log("New Grid x: " + grid.dx.ToString() + " y: " + grid.dy.ToString());
        int count = 0;
        for (int y = 4; y > 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                GameObject tile = Instantiate(OverlayPrefab, new Vector3(x, y, 0),Quaternion.identity);
                tile.transform.parent = tiles.transform;
                tile.GetComponent<Tile_Controlller>().setID(count);
                count++;
                tile.GetComponent<Tile_Controlller>().myGrid = grid;
            }
        }
    }

    void PopulateGrid()
    {
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
        if (Selecting)
        {
            CurrentWord = grid.GetCurrentPath();
        }
        else
        {
            CurrentWord = "";
        }
        CheckHoverOver();
        CheckMouseClicks();
    }

    void CheckMouseClicks()
    {
        if (!Selecting)
        {
            if (Input.GetMouseButtonDown(0) && HoverOver != -1)
            {
                Selecting = true;
                // Debug.Log("Test_Game_Controller:CheckMouseClicks() : Attempting to add " + HoverOver.ToString() + " to path");
                grid.AddToPath(HoverOver);
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
        int OldHover = HoverOver;
        HoverOver = -1;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider != null)
            {
                IisOverlayTile tile = hit.collider.GetComponent<IisOverlayTile>();              
                if (tile != null)
                {
                    HoverOver = tile.getID();
                }
            }
        }
        if (HoverOver != OldHover && Selecting) // We have a change (whilst selecting
        {
            if (HoverOver == -1)
            {
                grid.ClearPath();
                Selecting = false;
            }
            if (grid.legals.Contains(HoverOver) || grid.GetPathSecondFromEnd() == HoverOver) grid.AddToPath(HoverOver);
        }
    }

}
