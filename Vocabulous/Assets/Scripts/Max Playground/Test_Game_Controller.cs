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


    // Start is called before the first frame update
    void Start()
    {
        trie = GetComponent<MaxTrie>();
        grid = new GameGrid() { dx = 4, dy = 4, DEBUG = true };
        grid.init();
        Debug.Log("New Grid x: " + grid.dx.ToString() + " y: " + grid.dy.ToString());
        int count = 0;
        for (int y = 4; y > 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                GameObject tile = Instantiate(OverlayPrefab, new Vector3(x, y, 0),Quaternion.identity);
                tile.transform.parent = tiles.transform;
                tile.GetComponent<Tile_Controlller>().ID = count;
                count++;
                tile.GetComponent<Tile_Controlller>().myGrid = grid;
            }
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
                Debug.Log("Test_Game_Controller:CheckMouseClicks() : Attempting to add " + HoverOver.ToString() + " to path");
                grid.AddToPath(HoverOver);
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Selecting = false;
                string res = grid.FinishPath();
                if (trie.CheckWord(res))
                {
                    Debug.Log("You got " + res);
                    FoundWords.Add(res);
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
                IisOverlayTile tile = hit.collider.GetComponentInParent<IisOverlayTile>();
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
            if (grid.legals.Contains(HoverOver)) grid.AddToPath(HoverOver);
        }
    }

}
