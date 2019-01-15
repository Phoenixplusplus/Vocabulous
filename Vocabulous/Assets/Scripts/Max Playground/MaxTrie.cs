using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MaxTrie : MonoBehaviour
{
    private string DictPath = "/Dictionaries/XLGameDictUK.txt";
    private File_Reader reader;
    public string WordToCheck = "";
    public bool Loaded = false;
    [SerializeField]
    private int Trie_Word_Count = 0;
    [SerializeField]
    private Node _root;
    [SerializeField]
    private int Trie_Node_Count = 0;

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
        foreach (char c in str)
        {
            if (curr.Kids == null)
                curr.Kids = new Dictionary<char, Node>();

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
        reader.open(DictPath);
        bool _streaming = true;
        while (_streaming)
        {
            string str = reader.nextLine();
            //string str = reader.nextWord();
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
        Debug.Log("MaxTrie - Loaded");
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
            if (curr.Kids.ContainsKey(c))
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
        return result;
    }

    // PUBLIC function, feed in string, returns a Distinct List<string> or anagram solutions (based on paramaters)
    // letters {string} - anagram letters
    // complete {bool} - if true, returns only complete solutions using ALL letters
    // minimum {int} - the minimum length of word to be returned in the list
    public List<string> getOptions(string letters,bool complete, int minimum)
    {
        List<string> results = new List<string>();
        optionsMagic(letters, complete, results, _root,"",minimum);
        return results;
    }

    // internal Function, does the recursion for getOptions()
    private List<string> optionsMagic(string letters, bool complete, List<string> results, Node root, string soFar, int minimum)
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
                    optionsMagic(letters2, complete, results, root.Kids[letters[i]], soFar2, minimum);
                }
            }
        }
        return results;
    }

    // PUBLIC function, returns a list of words made using the sequence (N.B. from the start of the sequence ONLY)
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


    // Start is called before the first frame update
    // Currently Loads Trie ... and runs some test scripts
    void Start()
    {
        // Trie Loading
        _root = new Node() { Length = 1 };
        reader = GetComponent<File_Reader>();
        //double Start = Time.realtimeSinceStartup;
        LoadDictionary();
        //Debug.Log("Time to load Dictionary (" + DictPath + ") : " + (Time.realtimeSinceStartup - Start).ToString()+ " seconds");
        // End Trie Loading

        // Test Scripts

        //Start = Time.realtimeSinceStartup;
        //string tester = "gutsaloadsa";
        //List<string> results = testMyOptions(tester,false,5);
        //Debug.Log("Time to recusively check string (" + tester + ") : " + (Time.realtimeSinceStartup - Start).ToString() + " seconds - "+ results.Count.ToString()+" words found");
        //testMySequence("additions", 2);
    }

    // test script
    private List<string> testMyOptions(string tester,bool complete, int minimum)
    {
        List<string> results = getOptions(tester, complete, minimum);
        foreach (string s in results)
        {
            Debug.Log(s);
        }
        return results;
    }

    // test script
    private List<string> testMySequence(string sequence, int minimum)
    {
        List<string> results = CheckSequence(sequence, minimum);
        if (results.Count == 0) Debug.Log("Nothing found with checkSequence()");
        else
        {
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log(sequence + " has: " + results[i]);
            }
        }
        return results;
        
    }

}
