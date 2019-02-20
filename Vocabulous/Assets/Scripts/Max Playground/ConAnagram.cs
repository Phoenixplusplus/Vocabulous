using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConAnagram : MonoBehaviour
{
    private GC gc;
    public string Anagram;
    [SerializeField]
    private string CurrWord;
    [SerializeField]
    private List<string> AnswersList;
    private List<ConAnagramWord> ToGets;
    public ATableCon TableCon;
    public Vector3 AnswerListOffset;
    public int AnswersListWidth;
    public float AnswersListPitch;
    private AnagramLevels AL;
    public GameObject AnswersDisplay;
    public GameObject TilesDisplay;
    public LineRenderer LR;
    private List<Vector3> TileSpots;
    [SerializeField]
    private List<string> letters;
    private List<int> selected;
    private List<string> playerAnswers;
    [SerializeField]
    private bool Selecting = false;
    public Vector3 GridCentre;
    public float radius;
    private bool animating = false;
    private float animTimer = 0;

    [SerializeField]
    private int GameState = 0; // 0 = on Big Table, 1 = starting, 2 = playing, 3 = ended, awaiting restart
    private int ToFind = 0;

    // Start is called before the first frame update
    void Start()
    {
        AL = GetComponent<AnagramLevels>();
        gc = GC.Instance;
        AnswersList = new List<string>();
        ToGets = new List<ConAnagramWord>();
        letters = new List<string>();
        selected = new List<int>();
        playerAnswers = new List<string>();
        TableCon.Table();
    }

    void StartGame ()
    {
        AnswersList.Clear();
        ToGets.Clear();
        letters.Clear();
        selected.Clear();
        playerAnswers.Clear();
        Selecting = false;

        AnswersList = AL.GetAnagramLevel(gc.player.ALevel);
        //AnswersList = AL.GetAnagramLevel(20);
        Anagram = AnswersList[0];
        ToFind = AnswersList.Count - 1;
        foreach (char c in Anagram)
        {
            letters.Add("" + c);
        }
        shuffle(letters);
        DisplayHand();
        DisplayToGets();
        GameState = 2;
    }

    void DisplayHand ()
    {
        TileSpots = new List<Vector3>();
        int count = letters.Count;
        float angle = 360.0f / (float)count;
        Vector3 offset = new Vector3(0, 0, radius);
        for (int i = 0; i < count; i++)
        {
            // thanks to https://forum.unity.com/threads/rotating-a-vector-by-an-eular-angle.18485/ (Feb 2019)
            offset = Quaternion.AngleAxis(angle, Vector3.up) * offset;
            string IWant = letters[i] + "_";
            GameObject tile = gc.assets.SpawnTile(IWant, Vector3.zero, false, true);
            tile.transform.parent = TilesDisplay.transform;
            tile.transform.localRotation = TilesDisplay.transform.localRotation;
            tile.transform.localPosition = GridCentre;
            TileSpots.Add( tile.transform.position + ( (Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * offset) ) );
            Con_Tile2 con = tile.GetComponent<Con_Tile2>();
            con.SetFullFaceID(i, i);

            tile.AddComponent<Lerp>();
            Lerp L = tile.GetComponent<Lerp>();
            L.Configure(tile.transform.localPosition, tile.transform.localPosition + offset, 0.5f, true);
            L.Go();
            animTimer = 0.5f;

        }
    }

    void ShuffleHand()
    {
        foreach (Transform child in TilesDisplay.transform)
        {
            Destroy(child.gameObject);
        }
        shuffle(letters);
        DisplayHand();
    }

    void DisplayToGets()
    {
        int row = 0;
        int count = 0;
        List<float> rowoffset = new List<float>();
        rowoffset.Add(0f);
        for (int i = 1; i < AnswersList.Count; i++)
        {
            int len = AnswersList[i].Length;
            if (count + len >= AnswersListWidth)
            {
                rowoffset.Add(0f);
                row++;
                count = 0;
            }
            rowoffset[row] = (((float)AnswersListWidth - (float)count)/2f);
            Debug.Log("Row offset " + rowoffset[row].ToString());
            count += len + 1;
        }
        row = 0;
        count = 0;
        for (int i = 1; i < AnswersList.Count; i++)
        {
            int len = AnswersList[i].Length;
            if (count + len >= AnswersListWidth)
            {
                row++;
                count = 0;
            }
            GameObject ToGet = gc.assets.MakeWordFromTiles(AnswersList[i], Vector3.zero, 1f, true, false, false);
            ToGet.transform.parent = AnswersDisplay.transform;
            ToGet.transform.localRotation = AnswersDisplay.transform.localRotation;
            ToGet.transform.localPosition = AnswerListOffset + new Vector3(count + rowoffset[row], 0, row * AnswersListPitch);
            ToGet.AddComponent<ConAnagramWord>();
            ToGet.GetComponent<ConAnagramWord>().myWord = AnswersList[i];
            ToGets.Add(ToGet.GetComponent<ConAnagramWord>());
            count += len + 1;
        }
    }

    private List<string> shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string tmp = list[i];
            int r = Random.Range(i, list.Count-1);
            list[i] = list[r];
            list[r] = tmp;
        }
        return list;
    }

    public void KickOff()
    {
        TableCon.GameStart();
        StartGame();
    }
    
    public void TidyUp()
    {
        AnswersList.Clear();
        ToGets.Clear();
        letters.Clear();
        selected.Clear();
        playerAnswers.Clear();
        Selecting = false;
        killDisplays();
        TableCon.Table();
        GameState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Selecting)
        {
            CurrWord = "";
            foreach (int i in selected)
            {
                CurrWord += letters[i];
            }
        }

        CheckMouseClicks();
        CheckHoverOver();
        SetLineRenderer();
        if (animTimer !=0)
        {
            animTimer -= Time.deltaTime;
            if (animTimer <=0)
            {
                animTimer = 0;
                animating = false;
            }
        }
    }

    void SetLineRenderer()
    {
        if (!Selecting) LR.enabled = false;
        else
        {
            LR.enabled = true;
            LR.positionCount = selected.Count;
            for (int i = 0; i < selected.Count; i++)
            {
                LR.SetPosition(i, TileSpots[selected[i]]);
            }
        }

    }

    void GiveHint ()
    {
        // find "unsolved words" ...
        List<int> pos = new List<int>();
        for (int i = 0; i < ToGets.Count; i++)
        {
            if (ToGets[i].IsHintable()) pos.Add(i);
        }
        if (pos.Count == 0)
        {
            Debug.Log("Sorry no hints possible !!");
        }
        else
        {
            ToGets[pos[Random.Range(0, pos.Count)]].RevealHint();
        }
    }

    void CheckMouseClicks()
    {
        if (GameState == 2 && !animating) // game running
        {
            if (!Selecting) // not currently selecting anything
            {
                // Mouse goes down over a non -1 or 9999 IisOverlayTile object (i.e. a valid letter)
                if (Input.GetMouseButtonDown(0))
                {
                    if  (gc.NewHoverOver != -1 && gc.NewHoverOver <= 999)
                    {
                        Selecting = true;
                        selected.Add(gc.NewHoverOver);
                    }
                    else if (gc.NewHoverOver == 6664)
                    {
                        GiveHint();
                    }
                    else if (gc.NewHoverOver == 6665)
                    {
                        ShuffleHand();
                    }
                }
            }
            else // are selecting
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Selecting = false;
                    string res = "";
                    foreach (int i in selected)
                    {
                        res = res + letters[i];
                    }
                    selected.Clear();
                    if (AnswersList.Contains(res) && !playerAnswers.Contains(res)) // found standard answer
                    {
                        // ANIMATE - Gratz found one !!
                        // ANIMATE reveal answer
                        Debug.Log("You found: " + res);
                        playerAnswers.Add(res);
                        ToFind--;
                        foreach (ConAnagramWord t in ToGets)
                        {
                            if (t.myWord == res)
                            {
                                t.Roll(0.1f);
                            }
                        }
                        // check for game end
                        if (ToFind == 0)
                        {
                            EndGame();
                        }
                    }
                    else if (gc.maxTrie.CheckWord(res)) // NEW non-standard word
                    {
                        if (!playerAnswers.Contains(res)) // and not already found
                        {
                            // ANIMATE GREAT WORD, new for us
                            Debug.Log("GREAT new word");
                            playerAnswers.Add(res);
                        }
                        else
                        {
                            // ANIMATE - You've already got that one
                            Debug.Log("Already have that one");
                           // gc.FM.Flash(Flashes.AlreadyGot);
                        }
                    }
                    else
                    {
                        // ANIMATE - sorry word not recognised
                        Debug.Log("Not a word");
                    }
                }
            }
        }
        if (GameState == 3) // looking for restart
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gc.NewHoverOver == 6662) // restart game
                {
                    Debug.Log("Attempting restart");
                    ResetGame();
                    KickOff();
                }
                if (gc.NewHoverOver == 6663) // back to menu
                {
                    Debug.Log("Want to Return to main table ... but not connected yet");
                    TidyUp();
                }
            }
        }
    }

    void CheckHoverOver()
    {
        if (gc.HoverChange && Selecting && GameState == 2) // game running
        {
            if (gc.NewHoverOver == -1) // off grid - reset
            {
                selected.Clear();
                Selecting = false;
            }
            else
            {
                if (gc.NewHoverOver == selected[selected.Count -1]) // back track
                {
                    selected.RemoveAt(selected.Count - 1);
                }
                else
                {
                    if (gc.NewHoverOver < Anagram.Length && !selected.Contains(gc.NewHoverOver))
                    selected.Add(gc.NewHoverOver);
                }
            }

        }
    }

    void EndGame()
    {
        Debug.Log("Game Over .. you got them all");
        //gc.FM.Flash("Well Done !!", 4);
        gc.player.ALevel++;
        gc.SaveStats();
        GameState = 3;
        TableCon.EndGame();
    }

    void ResetGame()
    {
        killDisplays();
        AnswersList = new List<string>();
        ToGets = new List<ConAnagramWord>();
        letters = new List<string>();
        selected = new List<int>();
        playerAnswers = new List<string>();
    }

    void killDisplays ()
    {
        foreach (Transform child in AnswersDisplay.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in TilesDisplay.transform)
        {
            Destroy(child.gameObject);
        }
    }




    // Legacy script, used to determine Anagram candidates
    void ExamineWords()
    {
        double start = Time.realtimeSinceStartup;
        File_Reader fr = new File_Reader();
        File_Writer fw = new File_Writer();
        List<string> miniDict = new List<string>();
        fr.open("/Dictionaries/anagram_candidates.txt");
        bool reading = true;
        while (reading)
        {
            string word = fr.nextLine();
            if (word == null)
            {
                reading = false;
                fr.close();
            }
            else
            {
                miniDict.Add(word);
            }
        }
        Debug.Log("MiniDict loaded : " + miniDict.Count.ToString() + " words");

        fr.open("/Dictionaries/anagram_candidates.txt");
        fw.open("/Dictionaries/answers.txt");
        reading = true;
        int con = 0;
        while (reading)
        {
            string word = fr.nextLine();
            if (word == null)
            {
                reading = false;
                fr.close();
            }
            else
            {
                if (word.Length >= 4)
                {
                    List<string> sub = gc.assets.SortList(gc.maxTrie.getAnagram(word, false, 3));
                    int num = 0;
                    foreach (string s in sub)
                    {
                        if (miniDict.Contains(s)) num++;
                    }
                    if (num >= word.Length)
                    {
                        string ret = word;
                        foreach (string s in sub)
                        {
                            if (miniDict.Contains(s)) ret = ret + " " + s;
                        }
                        fw.writeLine(ret);
                        con++;
                    }
                }
            }
        }
        fw.close();
        Debug.Log("Words examinied : " + con.ToString() + " candidates in " + (Time.realtimeSinceStartup - start).ToString() + " secs");
    }



}
