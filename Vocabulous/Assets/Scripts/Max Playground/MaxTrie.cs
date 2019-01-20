using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MaxTrie : MonoBehaviour
{
    private string DictPath = "/Dictionaries/XLGameDictUK.txt";
    private File_Reader reader;
    public string WordToCheck = "";
    public bool Loaded = true;
    public bool Reporting = false;
    public bool Testing = true;
    [SerializeField]
    private int Trie_Word_Count = 0;
    [SerializeField]
    private Node _root;
    [SerializeField]
    private int Trie_Node_Count = 0;

    public Dictionary<char, int> Letter_Dist = new Dictionary<char, int>(); 
    public Dictionary<char, int> Word_Starts = new Dictionary<char, int>();
    public Dictionary<int, int> Word_Lengths = new Dictionary<int, int>();

    private Dictionary<float, char> RandomLetter = new Dictionary<float, char>() {
        {187095.0f/1700190.0f,'e'},{341116.0f/1700190.0f,'i'},{492408.0f/1700190.0f,'s'},
        { 629275.0f/1700190.0f,'a'},{749571.0f/1700190.0f,'r'},{867292.0f/1700190.0f,'n'},
        { 980758.0f/1700190.0f,'t'},{1090113.0f/1700190.0f,'o'},{1179858.0f/1700190.0f,'l'},
        { 1249386.0f/1700190.0f,'c'},{1309461.0f/1700190.0f,'u'},{1368152.0f/1700190.0f,'d'},
        { 1418768.0f/1700190.0f,'p'},{1468342.0f/1700190.0f,'m'},{1516149.0f/1700190.0f,'g'},
        { 1558124.0f/1700190.0f,'h'},{1589258.0f/1700190.0f,'b'},{1617793.0f/1700190.0f,'y'},
        { 1637837.0f/1700190.0f,'f'},{1654167.0f/1700190.0f,'v'},{1668262.0f/1700190.0f,'k'},
        { 1680616.0f/1700190.0f,'w'},{1689227.0f/1700190.0f,'z'},{1693958.0f/1700190.0f,'x'},
        { 1697117.0f/1700190.0f,'q'},{1700190.0f/1700190.0f,'j'}
    };
    private Dictionary<float, char> RandomWordStart = new Dictionary<float, char>() {
        {22310.0f/193655.0f,'s'},{39810.0f/193655.0f,'c'},{56252.0f/193655.0f,'p'},
        { 67846.0f/193655.0f,'d'},{79152.0f/193655.0f,'a'},{90015.0f/193655.0f,'m'},
        { 100558.0f/193655.0f,'t'},{110551.0f/193655.0f,'r'},{120315.0f/193655.0f,'b'},
        { 128382.0f/193655.0f,'u'},{136398.0f/193655.0f,'e'},{143800.0f/193655.0f,'i'},
        { 150847.0f/193655.0f,'f'},{157890.0f/193655.0f,'h'},{164415.0f/193655.0f,'g'},
        { 170143.0f/193655.0f,'l'},{175633.0f/193655.0f,'o'},{179638.0f/193655.0f,'w'},
        { 183446.0f/193655.0f,'n'},{186869.0f/193655.0f,'v'},{188989.0f/193655.0f,'k'},
        { 190748.0f/193655.0f,'j'},{191834.0f/193655.0f,'q'},{192689.0f/193655.0f,'z'},
        { 193418.0f/193655.0f,'y'},{193655.0f/193655.0f,'x'}
    };
    private Dictionary<float, int> RandomWordLength = new Dictionary<float, int>() {
        {29452.0f/193653.0f,8},{58060.0f/193653.0f,9},{84111.0f/193653.0f,7},
        { 108992.0f/193653.0f,10},{127745.0f/193653.0f,6},{146308.0f/193653.0f,11},
        { 159065.0f/193653.0f,12},{170500.0f/193653.0f,5},{178576.0f/193653.0f,13},
        { 183944.0f/193653.0f,4},{188578.0f/193653.0f,14},{191034.0f/193653.0f,15},
        { 192441.0f/193653.0f,3},{193445.0f/193653.0f,16},{193646.0f/193653.0f,2},
        { 193653.0f/193653.0f,17}
    };



    public class Node
    {
        public char Letter { get; set; }
        public int Length { get; set; }
        public Dictionary<char,Node> Kids { get; set;}
        public bool Word { get; set; }
        public bool Profane { get; set; }
    }

    public void AddWord(string str)
    {
        Node curr = _root;
        Node tmp = null;
        str = str.ToLower();
        if (Reporting)
        {
            int len = str.Length;
            if (!Word_Lengths.ContainsKey(len)) Word_Lengths.Add(len, 0);
            Word_Lengths[len]++;
        }
        foreach (char c in str)
        {
            if (curr.Kids == null)
                curr.Kids = new Dictionary<char, Node>();
            if (Reporting)
            {
                if (curr == _root)
                {
                    if (!Word_Starts.ContainsKey(c)) Word_Starts.Add(c, 0);
                    Word_Starts[c]++;
                }
                if (!Letter_Dist.ContainsKey(c)) Letter_Dist.Add(c, 0);
                Letter_Dist[c]++;
            }
            if (!curr.Kids.ContainsKey(c))
            {
                tmp = new Node() { Letter = c, Length = curr.Length + 1 };
                curr.Kids.Add(c, tmp);
                Trie_Node_Count++;
            }
            curr = curr.Kids[c];
        }
        curr.Word = true;
    }


    void LoadDictionary ()
    {
        if (DictPath == "" || DictPath == null)
        {
            Debug.Log("MaxTrie:Load() - Dictionary Path not Specified - returning now");
            return;
        }
        double Start = Time.realtimeSinceStartup;
        reader.open(DictPath);
        bool _streaming = true;
        while (_streaming)
        {
            string str = reader.nextLine();
            if (str == null)
            {
                _streaming = false;
                reader.close();
            }
            else
            {
                AddWord(str);
                Trie_Word_Count++;
            }
        }
        Loaded = true;
        if (Reporting) LogLetters();
        Debug.Log("MaxTrie - Loaded: "+(Time.realtimeSinceStartup - Start).ToString() + " seconds");
    }

 

    // PUBLIC function, feed in a word, returns true if it's in the Trie
    // word {string} - word to be checked 
    public bool CheckWord(string word)
    {
        if (word == null)
        {
            Debug.Log("MaxTrie:CheckWord() - no string submitted - false returned");
            return false;
        }
        word = word.ToLower();
        bool result = true;
        Node curr = _root;
        foreach (char c in word)
        {
            if (curr.Kids != null && curr.Kids.Count >= 1 && curr.Kids.ContainsKey(c))
            {
                curr = curr.Kids[c];
            }
            else
            {
                result = false;
                break;
            }
        }
        if (!curr.Word) result = false;
        //Debug.Log("CheckWord call for: " + word+" : "+result.ToString()+" returned");
        return result;
    }

    // PUBLIC function, checks is the presented string is a POSSIBLE start of a word
    // i.e. it is present in the Trie, AND there are routes to another(any) letter
    public bool CheckWordStart(string letters)
    {
        letters = letters.ToLower();
        bool result = true; // default
        Node curr = _root;
        foreach (char c in letters)
        {
            if (curr.Kids != null && curr.Kids.Count >= 1 && curr.Kids.ContainsKey(c))
            {
                curr = curr.Kids[c];
            }
            else
            {
                result = false;
                break;
            }
        }
        if (curr.Kids == null || curr.Kids.Count == 0) result = false;
        return result;
    }

    // PUBLIC function, provides a list of possible solutions to a partial string (with unknowns marked with "_")
    // Empty List is returned if no solutions found
    public List<string> SolveCrossword(string letters)
    {
        List<string> results = new List<string>();
        letters.ToLower();
        crossMagic(letters, ref results, "", _root);
        return results;
    }

    // PRIVATE function, does the recursive bit for SolveCrossword
    private void crossMagic (string letters, ref List<string> results, string soFar, Node root)
    {
        int len = letters.Length;
        if (len == 0)
        {
            if (root.Word && !results.Contains(soFar)) results.Add(soFar);
        }
        else
        {
            char c = letters[0];
            if (c !='_') // have a character ... see if we have a new path to recurse through
            {
                if (root.Kids != null && root.Kids.Count > 0 && root.Kids.ContainsKey(c))
                {
                    string newSoFar = soFar + letters[0];
                    string newLetters = letters.Remove(0, 1);
                    crossMagic(newLetters, ref results, newSoFar, root.Kids[c]);
                }
            }
            else
            {
                if (root.Kids != null && root.Kids.Count > 0)
                {
                    foreach (KeyValuePair<char,Node> item in root.Kids)
                    {
                        string newSoFar = soFar + item.Key;
                        string newLetters = letters.Remove(0, 1);
                        crossMagic(newLetters, ref results, newSoFar, item.Value);
                    }
                }
            }
        }
    }

    // PUBLIC function, feed in string, returns a Distinct List<string> of anagram solutions (based on paramaters)
    // letters {string} - anagram letters
    // complete {bool} - if true, returns only complete solutions using ALL letters
    // minimum {int} - the minimum length of word to be returned in the list
    public List<string> getAnagram(string letters,bool complete, int minimum)
    {
        List<string> results = new List<string>();
        letters = letters.ToLower();
        optionsMagic(letters, complete, ref results, _root,"",minimum);
        return results;
    }

    // internal Function, does the recursion for getOptions()
    private List<string> optionsMagic(string letters, bool complete, ref List<string> results, Node root, string soFar, int minimum)
    {
        int len = letters.Length;
        if (len == 0)
        {
            if (root.Word)
            {
                if (soFar.Length >= minimum && !results.Contains(soFar))
                {
                    results.Add(soFar);
                }
            }
        }
        else
        {
            if (root.Word && !complete)
            {
                string ret = soFar;
                if (soFar.Length >= minimum && !results.Contains(ret))
                {
                    results.Add(ret);
                }
            }
            for (int i = 0; i < len; i++)
            {
                if (root.Kids != null && root.Kids.ContainsKey(letters[i]))
                {
                    string soFar2 = soFar + letters[i];
                    string letters2 = letters.Remove(i,1);
                    optionsMagic(letters2, complete, ref results, root.Kids[letters[i]], soFar2, minimum);
                }
            }
        }
        return results;
    }

    // PUBLIC function, returns a list of words made using the sequence (N.B. from the START of the sequence ONLY)
    // sequence {string} - the sequence to check
    // minimum {int} the smallest word to include
    public List<string> CheckSequence(string sequence, int minimum)
    {
        List<string> results = new List<string>();
        string test = "";
        int len = sequence.Length;
        for (int i = 0; i < len; i++)
        {
            test = test + sequence[i];
            if (test.Length >= minimum)
            {
                if (CheckWord(test))
                {
                    string r = test;
                    results.Add(r);
                }
            }
        }
        return results;
    }

    // Returns a WEIGHTED random word length
    public int GetRandWordLength()
    {
        float test = Random.Range(0.0f, 1.0f);
        int ret = 0;
        foreach (KeyValuePair<float, int> odds in RandomWordLength)
        {
            if (test <= odds.Key && ret == 0) ret = odds.Value;
        }
        return ret;
    }

    // Returns a WEIGHTED random letter for a word starter
    public string GetRandomWordStart()
    {
        float test = Random.Range(0.0f, 1.0f);
        string ret = "";
        foreach (KeyValuePair<float, char> odds in RandomWordStart)
        {
            if (test <= odds.Key && ret == "") ret = ret + odds.Value;
        }
        return ret;
    }

    // Returns a WEIGHTED randome letter (based upon universal letter distribution in the dictionary)
    public string GetRandomLetter()
    {
        float test = Random.Range(0.0f, 1.0f);
        string ret = "";
        foreach (KeyValuePair<float, char> odds in RandomLetter)
        {
            if (test <= odds.Key && ret == "") ret = ret + odds.Value;
        }
        return ret;
    }

    // Start is called before the first frame update
    // Currently Loads Trie ... and runs some test scripts
    void Start()
    {
        // Trie Loading
        _root = new Node() { Length = 1 };
        reader = GetComponent<File_Reader>();
        LoadDictionary();
        // End Trie Loading
        // TEST SCRIPTS BELOW (if any)
        if (Testing) TestScripts();
    }

    private void TestScripts ()
    {
        var txt1 = "TEST LENGTHS";
        for (int i = 0; i < 50; i++)
        {
            txt1 = txt1 + ", " + GetRandWordLength().ToString();
        }
        Debug.Log(txt1);
        txt1 = "Word Starts";
        for (int i = 0; i < 50; i++)
        {
            txt1 = txt1 + ", " + GetRandomWordStart().ToString();
        }
        Debug.Log(txt1);
        txt1 = "Random Letters";
        for (int i = 0; i < 50; i++)
        {
            txt1 = txt1 + ", " + GetRandomLetter().ToString();
        }
        Debug.Log(txt1);
        txt1 = "ANAGRAM of (dfgtkjauys):";
        List<string> anagram = getAnagram("dfgtkjauys", false, 3);
        foreach (string s in anagram)
        {
            txt1 = txt1 + ", " + s;
        }
        Debug.Log(txt1);

        Debug.Log("Start: ad=" + CheckWordStart("ad").ToString() + ", add=" + CheckWordStart("add").ToString() + ", addx=" + CheckWordStart("addx").ToString());
        Debug.Log("Start: fi=" + CheckWordStart("fi").ToString() + ", fix=" + CheckWordStart("fix").ToString() + ", fixz=" + CheckWordStart("fixz").ToString());
        Debug.Log("Start: CO=" + CheckWordStart("CO").ToString() + ", To=" + CheckWordStart("To").ToString() + ", ruddE=" + CheckWordStart("ruddE").ToString());
        List<string> results = SolveCrossword("_ra_");
        txt1 = "Solving X-Word for (_ra_) "+results.Count.ToString()+" found";
        foreach (string s in results) txt1 = txt1 + ", " + s;
        Debug.Log(txt1);
        results = SolveCrossword("_ac__nat_on_");
        txt1 = "Solving X-Word for (_ac__nat_on_) " + results.Count.ToString() + " found";
        foreach (string s in results) txt1 = txt1 + ", " + s;
        Debug.Log(txt1);

    }

    // Internal function.  Debug logs Dictionary letter distributions, word start letters and length
    // Only called if "Reporting == true"
    private void LogLetters()
    {
        Debug.Log("LETTER DISTRIBUTION");
        foreach (KeyValuePair<char, int> pair in Letter_Dist)
        {
            Debug.Log(pair.Key.ToString() + ": " + pair.Value.ToString());
        }
        Debug.Log("WORD LETTER STARTS");
        foreach (KeyValuePair<char, int> pair in Word_Starts)
        {
            Debug.Log(pair.Key.ToString() + ": " + pair.Value.ToString());
        }
        Debug.Log("WORD LENGTHS");
        foreach (KeyValuePair<int, int> pair in Word_Lengths)
        {
            Debug.Log(pair.Key.ToString() + ": " + pair.Value.ToString());
        }
    }

}
