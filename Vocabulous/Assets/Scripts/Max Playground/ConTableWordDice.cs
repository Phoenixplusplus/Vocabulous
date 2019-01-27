using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConTableWordDice : MonoBehaviour
{
    // Controller for the Tabletop representation of WordDice
    // responds to hoverover
    // On table ... shakes and colour changes to highlight it can be clicked to "start"
    // Disappears "in-game"
    // Presents "Restart" after game over
    // maybe asks "see what you could have had?"

    public GameObject Worddice;
    public GameObject StartDice;
    public GameObject TimeUp;
    public GameObject GUIStart;
    private GC gc;
    private bool highlighted;

    public Color TitleNormalColor;
    public Color TitleSelectedColor;
    public Color StartNormalColor;
    public Color StartSelectedColor;
    public Color TimeUpColor;
    public Tile_Controlller GUITile;
    public float DiceScaleFactor = 1.4f;

    private ConDice[] title;
    private ConDice[] start;
    private ConDice[] timeup;

    void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        gc = GC.Instance;

        GUITile.SetVisible(false);
        //GUITile.SetLetter("");
        GUITile.setID(8881);

        title = Worddice.GetComponentsInChildren<ConDice>();
        start = StartDice.GetComponentsInChildren<ConDice>();
        timeup = TimeUp.GetComponentsInChildren<ConDice>();
        foreach (var ConDice in title)
        {
            ConDice.killOverlayTile();
        }
        foreach (var ConDice in start)
        {
            ConDice.killOverlayTile();
        }
        setToNormal();

    }

    // Update is called once per frame
    void Update()
    {
        if (gc.HoverChange && gc.NewHoverOver == 8881 && !highlighted) // have just moved over me ... best do something
        {
            highlighted = true;
            setToHighlight();
            ScaleDiceUp();
        }
        if (highlighted && gc.NewHoverOver != 8881)
        {
            highlighted = false;
            setToNormal();
            ScaleDiceDown();
        }

    }

    public void OnSceneTable()
    {
        TimeUp.SetActive(false);
        Worddice.SetActive(true);
        StartDice.SetActive(true);
        GUIStart.SetActive(true);
    }

    public void GameRunning()
    {
        TimeUp.SetActive(false);
        Worddice.SetActive(false);
        StartDice.SetActive(false);
        GUIStart.SetActive(false);
    }

    public void GameOver()
    {
        TimeUp.SetActive(true);
        Worddice.SetActive(false);
        StartDice.SetActive(false);
        GUIStart.SetActive(false);
    }


    private void setToNormal()
    {
        foreach (ConDice die in title)
        {
            die.ChangeDiceColor(TitleNormalColor);
            die.shaking = false; // needs work
        }
        foreach (ConDice die in start)
        {
            die.ChangeDiceColor(StartNormalColor);
        }
        foreach (ConDice die in timeup)
        {
            die.ChangeDiceColor(TimeUpColor);
        }
    }

    private void setToHighlight()
    {
        foreach (ConDice die in title)
        {
            die.ChangeDiceColor(TitleSelectedColor);
            die.shaking = true; // needs work
        }
        foreach (ConDice die in start)
        {
            die.ChangeDiceColor(StartSelectedColor);
        }
    }

    private void ScaleDiceUp()
    {
        foreach (ConDice die in title)
        {
            die.Scale(DiceScaleFactor);

        }
        foreach (ConDice die in start)
        {
            die.Scale(DiceScaleFactor);
        }
    }

    
    private void ScaleDiceDown()
    {
        foreach (ConDice die in title)
        {
            die.Scale(1.0f / DiceScaleFactor);

        }
        foreach (ConDice die in start)
        {
            die.Scale(1.0f / DiceScaleFactor);
        }
    }


}
