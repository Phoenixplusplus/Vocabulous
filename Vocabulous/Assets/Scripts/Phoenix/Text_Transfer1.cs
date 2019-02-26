using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Transfer1 : MonoBehaviour
{
    public File_Reader1 reader;
    public TrieTest Trie;
    private bool _running = false;

    // Start is called before the first frame update
    void Start()
    {
        //reader.open("/Dictionaries/XLGameDictUK.txt");
        reader.open("/StreamingAssets/Game_Dictionary.txt");
        
        _running = true;
        while (_running)
        {
            check(reader.nextLine());
        }

    }

    void check(string word)
    {
        if (word == null)
        {
            _running = false;
            closeAll();
        }
        else
        {
            Trie.allWordsList.Add(word);
        }
    }

    void closeAll()
    {
        reader.close();
        Trie.Initialise();
    }
}
