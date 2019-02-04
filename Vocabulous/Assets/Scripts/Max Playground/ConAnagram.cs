using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConAnagram : MonoBehaviour
{
    private GC gc;
    public string Anagram;
    [SerializeField]
    private List<string> AnswersList;
    private AnagramLevels AL;
    public GameObject AnswersDisplay;
    public GameObject TilesDisplay;
    [SerializeField]
    private List<string> letters;

    // Start is called before the first frame update
    void Start()
    {
        AL = GetComponent<AnagramLevels>();
        gc = GC.Instance;
        AnswersList = new List<string>();
        letters = new List<string>();
        StartGame();
    }

    void StartGame ()
    {
        AnswersList = AL.GetAnagramLevel(gc.player.ALevel);
        Anagram = AnswersList[0];
        foreach (char c in Anagram)
        {
            letters.Add("" + c);
        }
        shuffle(letters);
        Debug.Log(Anagram);
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

    }
    
    public void TidyUp()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
