using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATableCon : MonoBehaviour
{
    private GC gc;
    public GameObject Main;
    public GameObject InGame;
    public GameObject Restart;
    public GameObject Shuffle;
    public GameObject TableTiles;
    private Con_Tile2[] nextTiles;
    private Con_Tile2[] hintTiles;
    private Con_Tile2[] shuffleTiles;
    private Con_Tile2[] tableTiles;
    private TileLerper[] lerpers;
    public Color NormalColor;
    public Color BaseTileColor;
    public Color HighLightTileColor;

    public void Table()
    {
        Main.SetActive(true);
        ResetMainTable();
        InGame.SetActive(false);
        Shuffle.SetActive(false);
        Restart.SetActive(false);
    }

    public void GameStart()
    {
        Debug.Log("Calling Game Start in Table Controller");
        //if (Main.active) ResetMainTable();
        Main.SetActive(false);
        InGame.SetActive(true);
        Shuffle.SetActive(true);
        Restart.SetActive(false);
    }

    public void EndGame()
    {
        //if (Main.active) ResetMainTable();
        Main.SetActive(false);
        InGame.SetActive(false);
        Shuffle.SetActive(false);
        Restart.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        gc = GC.Instance;
        nextTiles = Restart.GetComponentsInChildren<Con_Tile2>();
        ChangeNextColor(BaseTileColor);
        hintTiles = InGame.GetComponentsInChildren<Con_Tile2>();
        ChangeHintColor(BaseTileColor);
        shuffleTiles = Shuffle.GetComponentsInChildren<Con_Tile2>();
        ChangeShuffleColor(BaseTileColor);
        tableTiles = TableTiles.GetComponentsInChildren<Con_Tile2>();
        ChangeTableColor(HighLightTileColor);
        lerpers = TableTiles.GetComponentsInChildren<TileLerper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.HoverChange)
        {
            if (gc.NewHoverOver == 6661 && Main.active) // MainTable Display
            {
                AnimateMain();
            }
            else if (gc.OldHoverOver == 6661 && Main.active)
            {
                StopAnimateMain();
            }

            if (gc.NewHoverOver == 6662) // Restart / "Next"
            {
                ChangeNextColor(HighLightTileColor);
            }
            else if (gc.OldHoverOver == 6662)
            {
                ChangeNextColor(BaseTileColor);
            }

            if (gc.NewHoverOver == 6664) // Hint
            {
                ChangeHintColor(HighLightTileColor);
            }
            else if (gc.OldHoverOver == 6664)
            {
                ChangeHintColor(BaseTileColor);
            }

            if (gc.NewHoverOver == 6665) // Shuffle
            {
                ChangeShuffleColor(HighLightTileColor);
            }
            else if (gc.OldHoverOver == 6665)
            {
                ChangeShuffleColor(BaseTileColor);
            }

        }
    }

    void AnimateMain()
    {
        ChangeTableColor(HighLightTileColor);
        foreach (Con_Tile2 tile in tableTiles)
        {
            tile.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        foreach (TileLerper t in lerpers)
        {
            t.LerpForward();
        }
    }

    void StopAnimateMain()
    {
        ChangeTableColor(NormalColor);
        foreach (Con_Tile2 tile in tableTiles)
        {
            tile.transform.localScale = new Vector3(1, 1, 1);
        }
        foreach (TileLerper t in lerpers)
        {
            t.LerpBackwards();
        }
    }

    void ResetMainTable()
    {
        ChangeTableColor(NormalColor);
        foreach (Con_Tile2 tile in tableTiles)
        {
            tile.transform.localScale = new Vector3(1, 1, 1);
        }
        foreach (TileLerper t in lerpers)
        {
            t.SetToFinish();
        }
    }

    void ChangeNextColor (Color color)
    {
        foreach (Con_Tile2 tile in nextTiles)
        {
            tile.ChangeTileColor(color);
        }
    }

    void ChangeHintColor(Color color)
    {
        foreach (Con_Tile2 tile in hintTiles)
        {
            tile.ChangeTileColor(color);
        }
    }

    void ChangeShuffleColor(Color color)
    {
        foreach (Con_Tile2 tile in shuffleTiles)
        {
            tile.ChangeTileColor(color);
        }
    }

    void ChangeTableColor (Color color)
    {
        foreach (Con_Tile2 tile in tableTiles)
        {
            tile.ChangeTileColor(color);
        }
    }


}
