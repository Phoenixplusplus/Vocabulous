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
    // Presents "Back to table" after game over

    public GameObject Worddice;
    public GameObject StartDice;
    public GameObject TimeUp;
    public GameObject GUIStart;
    public GameObject Restart;
    public GameObject Back;
    public GameObject Box;
    public GameObject Clock;
    private GC gc;
    private bool highlighted;
    private bool restarthighlight;
    //private bool backhighlight;

    public Color TitleNormalColor;
    public Color TitleSelectedColor;
    public Color StartNormalColor;
    public Color StartSelectedColor;
    public Color TimeUpColor;
    public Tile_Controlller GUITile;
    public Tile_Controlller restartGUITile;
    public Tile_Controlller backGUITile;
    public float DiceScaleFactor = 1.4f;


    private ConDice[] title;
    private ConDice[] start;
    private ConDice[] timeup;
    private ConDice[] restart;

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
        restartGUITile.setID(8882);
        backGUITile.setID(8883);

        title = Worddice.GetComponentsInChildren<ConDice>();
        start = StartDice.GetComponentsInChildren<ConDice>();
        timeup = TimeUp.GetComponentsInChildren<ConDice>();
        restart = Restart.GetComponentsInChildren<ConDice>();
        //back = Back.GetComponentsInChildren<ConDice>();
        Back.SetActive(false);
        foreach (var ConDice in title)
        {
            ConDice.killOverlayTile();
        }
        foreach (var ConDice in start)
        {
            ConDice.killOverlayTile();
        }
        foreach (var ConDice in restart)
        {
            ConDice.killOverlayTile();
        }
        setToNormal();

    }

    // Update is called once per frame
    void Update()
    {
        // CHECKING - Start GUI Tile
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
        // CHECKING - Restart GUI Tile
        if (gc.HoverChange && gc.NewHoverOver == 8882 && !restarthighlight)
        {
            restarthighlight = true;
            foreach (ConDice c in restart)
            {
                c.ChangeDiceColor(StartSelectedColor);
            }
        }
        if (restarthighlight && gc.NewHoverOver != 8882)
        {
            restarthighlight = false;
            foreach (ConDice c in restart)
            {
                c.ChangeDiceColor(TitleNormalColor);
            }
        }
    }

    public void OnSceneTable()
    {
        TimeUp.SetActive(false);
        Worddice.SetActive(true);
        StartDice.SetActive(true);
        GUIStart.SetActive(true);
        Restart.SetActive(false);
        Box.SetActive(false);
        Clock.SetActive(false);
        //Back.SetActive(false);
    }

    public void GameRunning()
    {
        TimeUp.SetActive(false);
        Worddice.SetActive(false);
        StartDice.SetActive(false);
        GUIStart.SetActive(false);
        Restart.SetActive(false);
        Box.SetActive(true);
        Clock.SetActive(true);
        //Back.SetActive(false);
    }

    public void GameOver()
    {
        TimeUp.SetActive(true);
        Worddice.SetActive(false);
        StartDice.SetActive(false);
        GUIStart.SetActive(false);
        Restart.SetActive(true);
        Restart.AddComponent<Lerp>();
        Lerp L = Restart.GetComponent<Lerp>();
        L.Configure(Restart.transform.localPosition + new Vector3(10, 0, 0), Restart.transform.localPosition, 1f, true);
        L.Go();
        Box.SetActive(true);
        Clock.SetActive(false);
        //Back.SetActive(true);
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
            die.ChangeDiceColor(TitleNormalColor);
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
