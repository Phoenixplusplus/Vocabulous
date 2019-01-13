using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Transfer1 : MonoBehaviour
{
    public File_Reader1 reader;
    public TrieTest Trie;
    //public File_Writer writer;
    private bool _running = false;

    // Start is called before the first frame update
    void Start()
    {
        reader.open("/Dictionaries/L3-4 from 65K.txt");
        //writer.open("/Dictionaries/testOutput.txt");
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
        //else
        //{
        //    writer.writeLine(word);
        //}
    }

    void closeAll()
    {
        reader.close();
        //writer.close();
        Debug.Log("Read finished");
        Trie.Initialise();
    }
}
