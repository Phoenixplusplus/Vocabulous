//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDropController : MonoBehaviour
{
    [SerializeField]
    GC gameController;
    GameGrid grid;
    public TrieTest trie;
    public GameObject tileHolder, ingameModels, wordDropTable;

    int gameSize = 8;
    float tileXOffset = 0.2f;
    Vector3 tileOverlayScale = new Vector3(1.15f, 1.15f, 1.15f);

    void Start()
    {
        Initialise();
    }

    void Initialise()
    {
        if (gameController == null)
        {
            gameController = GC.Instance;
            trie = gameController.phoenixTrie;
        }

        // setup grid and tiles
        grid = new GameGrid() { dx = 8, dy = 8, DEBUG = true }; // debug spawns with random letters in bins
        grid.init();
        grid.directional = true;

        // just doing this for reference of positions
        // pop tiles in
        int count = 0;
        for (int x = 0; x < gameSize; x++)
        {
            for (int y = gameSize; y > 0; y--)
            {
                GameObject tile = gameController.assets.SpawnTile(grid.bins[count], new Vector3(tileHolder.transform.position.x + (x + (tileXOffset * x)), tileHolder.transform.position.y + y, tileHolder.transform.position.z), true, false);
                tile.transform.parent = tileHolder.transform;
                Con_Tile2 tilecon = tile.GetComponent<Con_Tile2>();
                tilecon.SetID(count, count);
                tilecon.myGrid = grid;
                Tile_Controlller[] tileOverlays = tile.GetComponentsInChildren<Tile_Controlller>();
                foreach (Tile_Controlller tc in tileOverlays)
                {
                    tc.transform.localScale = tileOverlayScale;
                }
                count++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
