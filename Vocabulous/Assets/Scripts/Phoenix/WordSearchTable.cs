using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSearchTable : MonoBehaviour
{
    private GC gameController;
    public Tile_Controlller startOverlay, restartOverlay;
    public GameObject startObjects, restartObjects, boards, scrubber, chalk, clock;
    public bool onStartHoverOver, onRestartHoverOver;
    public Color normalColour, hoveredColour;

    ConDice[] tableDice, restartDice;
    Vector3[] diceStartPos = new Vector3[9];
    Vector3[] restartDiceStartPos = new Vector3[8];
    Quaternion[] diceStartRot = new Quaternion[9];
    Quaternion[] restartDiceStartRot = new Quaternion[8];
    public BoardAnimator[] unfoundWordObjects = new BoardAnimator[9];
    public BoardAnimator[] foundWordObjects = new BoardAnimator[9];

    // Start is called before the first frame update
    void Start()
    {
        gameController = GC.Instance;
        startOverlay.setID(4441);
        restartOverlay.setID(4442);

        tableDice = startObjects.GetComponentsInChildren<ConDice>();
        restartDice = restartObjects.GetComponentsInChildren<ConDice>();

        normalColour = tableDice[0].DiceBody.GetComponent<Renderer>().material.color;

        for (int i = 0; i < tableDice.Length; i++)
        {
            diceStartPos[i] = tableDice[i].gameObject.transform.position + new Vector3(0, 0.5f, 0);
            diceStartRot[i] = tableDice[i].gameObject.transform.rotation;
            tableDice[i].killOverlayTile();
        }
        for (int i = 0; i < restartDice.Length; i++)
        {
            restartDiceStartPos[i] = restartDice[i].gameObject.transform.position + new Vector3(0, 0.5f, 0);
            restartDiceStartRot[i] = restartDice[i].gameObject.transform.rotation;
            restartDice[i].killOverlayTile();
        }

        StartSetup();

        unfoundWordObjects = boards.transform.GetChild(1).GetComponentsInChildren<BoardAnimator>();
        foundWordObjects = boards.transform.GetChild(0).GetComponentsInChildren<BoardAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        // checking hover over
        if (gameController.HoverChange && gameController.NewHoverOver == 4441 && !onStartHoverOver)
        {
            onStartHoverOver = true;
            SetHoverColourOnStartDice();
            StartCoroutine(ShiftDiceToReadyPosition(3f));
        }
        if (onStartHoverOver && gameController.NewHoverOver != 4441)
        {
            onStartHoverOver = false;
            SetNormalColourOnStartDice();
            StartCoroutine(ShiftDiceToStartPosition(3f));
        }

        if (gameController.HoverChange && gameController.NewHoverOver == 4442 && !onRestartHoverOver)
        {
            onRestartHoverOver = true;
            SetHoverColourOnRestartDice();
        }
        if (onRestartHoverOver && gameController.NewHoverOver != 4442)
        {
            onRestartHoverOver = false;
            SetNormalColourOnRestartDice();
        }
    }

    public void ToggleStartObjects(bool state) { if (startObjects.activeInHierarchy == !state) startObjects.SetActive(state); }
    public void ToggleRestartObjects(bool state) { if (restartObjects.activeInHierarchy == !state) restartObjects.SetActive(state); }
    public void ToggleBoards(bool state) { if (boards.activeInHierarchy == !state) boards.SetActive(state); }
    public void ToggleScrubberChalk(bool state) { if (scrubber.activeInHierarchy == !state) { scrubber.SetActive(state); chalk.SetActive(state); } }
    public void ToggleClock(bool state) { if (clock.activeInHierarchy == !state) clock.SetActive(state); }
    public void HideAll()
    {
        ToggleStartObjects(false);
        ToggleBoards(false);
        ToggleScrubberChalk(false);
        ToggleClock(false);
        ToggleRestartObjects(false);
        ResetRestartDiceOrientation();
    }

    public void StartSetup()
    {
        ToggleStartObjects(true);
        ToggleBoards(false);
        ToggleScrubberChalk(false);
        ToggleClock(false);
        ToggleRestartObjects(false);
        ResetRestartDiceOrientation();
    }

    public void IngameSetup()
    {
        ToggleStartObjects(false);
        ToggleBoards(true);
        ToggleScrubberChalk(true);
        ToggleClock(true);
        ToggleRestartObjects(false);
        ResetRestartDiceOrientation();
    }

    public void RestartSetup()
    {
        ToggleRestartObjects(true);
        ToggleBoards(true);
        ToggleClock(true);
        ToggleScrubberChalk(true);
        ResetRestartDiceOrientation();
        StartCoroutine(ShiftDiceToRestartPosition(3f));
    }

    public void ResetRestartDiceOrientation()
    {
        for (int i = 0; i < restartDice.Length; i++)
        {
            restartDice[i].transform.position = restartDiceStartPos[i];
            restartDice[i].transform.rotation = restartDiceStartRot[i];
        }
    }

    public void SetHoverColourOnStartDice()
    {
        foreach(ConDice dice in tableDice)
        {
            dice.ChangeDiceColor(hoveredColour);
        }
    }

    public void SetNormalColourOnStartDice()
    {
        foreach (ConDice dice in tableDice)
        {
            dice.ChangeDiceColor(normalColour);
        }
    }

    public void SetHoverColourOnRestartDice()
    {
        foreach (ConDice dice in restartDice)
        {
            dice.ChangeDiceColor(hoveredColour);
        }
    }

    public void SetNormalColourOnRestartDice()
    {
        foreach (ConDice dice in restartDice)
        {
            dice.ChangeDiceColor(normalColour);
        }
    }

    IEnumerator ShiftDiceToReadyPosition(float finishTime)
    {
        float t = 0;
        while (t < finishTime)
        {
            foreach (ConDice dice in tableDice)
            {
                dice.transform.position = Vector3.Lerp(dice.transform.position, dice.transform.parent.transform.position, t / finishTime);
                dice.transform.rotation = Quaternion.Lerp(dice.transform.rotation, dice.transform.parent.transform.rotation, t / finishTime);
            }
            t += Time.deltaTime;
            if (onStartHoverOver && gameController.NewHoverOver != 4441 || gameController.GameState == 35) yield break;
            yield return null;
        }
        yield break;
    }

    IEnumerator ShiftDiceToStartPosition(float finishTime)
    {
        float t = 0;
        while (t < finishTime)
        {
            for (int i = 0; i < tableDice.Length; i++)
            {
                tableDice[i].transform.position = Vector3.Lerp(tableDice[i].transform.position, diceStartPos[i], t / finishTime);
                tableDice[i].transform.rotation = Quaternion.Lerp(tableDice[i].transform.rotation, diceStartRot[i], t / finishTime);
            }
            t += Time.deltaTime;
            if (gameController.HoverChange && gameController.NewHoverOver == 4441 && !onStartHoverOver || gameController.GameState == 35) yield break;
            yield return null;
        }
        yield break;
    }

    IEnumerator ShiftDiceToRestartPosition(float finishTime)
    {
        float t = 0;
        while (t < finishTime)
        {
            foreach (ConDice dice in restartDice)
            {
                dice.transform.position = Vector3.Lerp(dice.transform.position, dice.transform.parent.transform.position, t / finishTime);
                dice.transform.rotation = Quaternion.Lerp(dice.transform.rotation, dice.transform.parent.transform.rotation, t / finishTime);
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
