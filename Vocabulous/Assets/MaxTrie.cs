using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MaxTrie : MonoBehaviour
{
    private string DictPath = "/Dictionaries/L3-17 for Q from 65K.txt";
    private File_Reader reader;
    public string WordToCheck = "";
    public bool Loaded = false;
    [SerializeField]
    private int Trie_Word_Count = 0;
    [SerializeField]
    private Node _root;

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
    }

    bool CheckWord(string word)
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

    // Start is called before the first frame update
    void Start()
    {
        _root = new Node() { Length = 1 };
        reader = GetComponent<File_Reader>();
        double Start = Time.realtimeSinceStartup;
        LoadDictionary();
        Debug.Log("Time to load Dictionary (" + DictPath + ") : " + (Time.realtimeSinceStartup - Start).ToString()+ " seconds");
        if (Loaded)
        {
            Debug.Log("dog = " + CheckWord("dog"));
            Debug.Log("god = " + CheckWord("god"));
            Debug.Log("odg = " + CheckWord("odg"));
        }
    }


}
