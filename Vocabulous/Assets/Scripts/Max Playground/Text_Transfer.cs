using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Transfer : MonoBehaviour
{
    public File_Reader reader;
    public File_Writer writer;
    private bool _running = false;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        reader.open("/Dictionaries/UK English 65K words.txt");
        writer.open("/Dictionaries/L2-6 from 65K.txt");
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
            if (word.Length >= 2 && word.Length <=6)
            {
                writer.writeLine(word);
                count++;
            }
        }
    }

    void closeAll()
    {
        reader.close();
        writer.close();
        Debug.Log("L2-6 from 65K.txt - Word Count: " + count.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
