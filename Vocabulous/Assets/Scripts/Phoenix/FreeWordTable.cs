using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeWordTable : MonoBehaviour
{
    private GC gameController;
    public Tile_Controlller startOverlay, restartOverlay;
    public GameObject startObjects, restartObjects, clock;
    public bool onStartHoverOver, onRestartHoverOver;
    public Color normalColour, hoveredColour, restartHoveredColour;

    Con_Tile2[] tableTiles, restartTiles;
    Vector3[] tileStartPos = new Vector3[8];
    Quaternion[] tileStartRot = new Quaternion[8];

    // Start is called before the first frame update
    void Start()
    {
        gameController = GC.Instance;
        startOverlay.setID(5551);
        restartOverlay.setID(5552);

        tableTiles = startObjects.GetComponentsInChildren<Con_Tile2>();
        restartTiles = restartObjects.GetComponentsInChildren<Con_Tile2>();

        normalColour = tableTiles[0].Body_Material.color;

        for (int i = 0; i < tableTiles.Length; i++)
        {
            tileStartPos[i] = tableTiles[i].gameObject.transform.localPosition;
            tileStartRot[i] = tableTiles[i].gameObject.transform.rotation;
            tableTiles[i].killOverlayTile();
        }
        for (int i = 0; i < restartTiles.Length; i++)
        {
            restartTiles[i].killOverlayTile();
        }

        StartSetup();
    }

    // Update is called once per frame
    void Update()
    {
        // checking hover over
        if (gameController.HoverChange && gameController.NewHoverOver == 5551 && !onStartHoverOver)
        {
            onStartHoverOver = true;
            SetHoverColourOnStartTiles();
            StopAllCoroutines();
            StartCoroutine(ShiftTilesToReadyPosition(3f));
        }
        if (onStartHoverOver && gameController.NewHoverOver != 5551)
        {
            onStartHoverOver = false;
            SetNormalColourOnStartTiles();
            StopAllCoroutines();
            StartCoroutine(ShiftTilesToStartPosition(3f));
        }

        if (gameController.HoverChange && gameController.NewHoverOver == 5552 && !onRestartHoverOver)
        {
            onRestartHoverOver = true;
            SetHoverColourOnRestartTiles();
        }
        if (onRestartHoverOver && gameController.NewHoverOver != 5552)
        {
            onRestartHoverOver = false;
            SetNormalColourOnRestartTiles();
        }
    }

    public void ToggleStartObjects(bool state) { if (startObjects.activeInHierarchy == !state) startObjects.SetActive(state); }
    public void ToggleRestartObjects(bool state) { if (restartObjects.activeInHierarchy == !state) restartObjects.SetActive(state); }
    public void ToggleClock(bool state) { if (clock.activeInHierarchy == !state) clock.SetActive(state); }
    public void HideAll()
    {
        ToggleStartObjects(false);
        ToggleClock(false);
        ToggleRestartObjects(false);
    }

    public void StartSetup()
    {
        ToggleStartObjects(true);
        ToggleClock(false);
        ToggleRestartObjects(false);
    }

    public void IngameSetup()
    {
        ToggleStartObjects(false);
        ToggleClock(true);
        ToggleRestartObjects(false);
    }

    public void RestartSetup()
    {
        ToggleRestartObjects(true);
        ToggleClock(true);
        StartCoroutine(ShiftTilesToRestartPosition(3f));
    }

    public void SetHoverColourOnStartTiles()
    {
        foreach (Con_Tile2 tile in tableTiles)
        {
            tile.ChangeTileColor(hoveredColour);
        }
    }

    public void SetNormalColourOnStartTiles()
    {
        foreach (Con_Tile2 tile in tableTiles)
        {
            tile.ChangeTileColor(normalColour);
        }
    }

    public void SetHoverColourOnRestartTiles()
    {
        foreach (Con_Tile2 tile in restartTiles)
        {
            tile.ChangeTileColor(restartHoveredColour);
        }
    }

    public void SetNormalColourOnRestartTiles()
    {
        foreach (Con_Tile2 tile in restartTiles)
        {
            tile.ChangeTileColor(normalColour);
        }
    }

    IEnumerator ShiftTilesToReadyPosition(float finishTime)
    {
        float t = 0;
        while (t < finishTime)
        {
            foreach (Con_Tile2 tile in tableTiles)
            {
                tile.transform.localPosition = Vector3.Lerp(tile.transform.localPosition, Vector3.zero, t / finishTime);
                tile.transform.rotation = Quaternion.Lerp(tile.transform.rotation, tile.transform.parent.transform.rotation, t / finishTime);
            }
            t += Time.deltaTime;
            if (onStartHoverOver && gameController.NewHoverOver != 5551 || gameController.GameState == 34) yield break;
            yield return null;
        }
        yield break;
    }

    IEnumerator ShiftTilesToStartPosition(float finishTime)
    {
        float t = 0;
        while (t < finishTime)
        {
            for (int i = 0; i < tableTiles.Length; i++)
            {
                tableTiles[i].transform.localPosition = Vector3.Lerp(tableTiles[i].transform.localPosition, tileStartPos[i], t / finishTime);
                tableTiles[i].transform.rotation = Quaternion.Lerp(tableTiles[i].transform.rotation, tileStartRot[i], t / finishTime);
            }
            t += Time.deltaTime;
            if (gameController.HoverChange && gameController.NewHoverOver == 5551 && !onStartHoverOver || gameController.GameState == 34) yield break;
            yield return null;
        }
        yield break;
    }

    IEnumerator ShiftTilesToRestartPosition(float finishTime)
    {
        float t = 0;
        while (t < finishTime)
        {
            foreach (Con_Tile2 tile in restartTiles)
            {
                tile.transform.localPosition = Vector3.Lerp(tile.transform.localPosition, Vector3.zero, t / finishTime);
                tile.transform.rotation = Quaternion.Lerp(tile.transform.rotation, tile.transform.parent.transform.rotation, t / finishTime);
            }
            t += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
