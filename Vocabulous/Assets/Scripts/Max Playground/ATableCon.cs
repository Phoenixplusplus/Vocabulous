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
    private Con_Tile2[] nextTiles;
    private Con_Tile2[] hintTiles;
    private Con_Tile2[] shuffleTiles;
    public Color BaseTileColor;
    public Color HighLightTileColor;

    public void Table()
    {
        Main.SetActive(true);
        InGame.SetActive(false);
        Shuffle.SetActive(false);
        Restart.SetActive(false);
    }

    public void GameStart()
    {
        Main.SetActive(false);
        InGame.SetActive(true);
        Shuffle.SetActive(true);
        Restart.SetActive(false);
    }

    public void EndGame()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.HoverChange)
        {
            if (gc.NewHoverOver == 6662)
            {
                ChangeNextColor(HighLightTileColor);
            }
            else if (gc.OldHoverOver == 6662)
            {
                ChangeNextColor(BaseTileColor);
            }

            if (gc.NewHoverOver == 6664)
            {
                ChangeHintColor(HighLightTileColor);
            }
            else if (gc.OldHoverOver == 6664)
            {
                ChangeHintColor(BaseTileColor);
            }

            if (gc.NewHoverOver == 6665)
            {
                ChangeShuffleColor(HighLightTileColor);
            }
            else if (gc.OldHoverOver == 6665)
            {
                ChangeShuffleColor(BaseTileColor);
            }

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

}
