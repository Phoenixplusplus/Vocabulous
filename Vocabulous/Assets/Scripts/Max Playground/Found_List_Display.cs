using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Found_List_Display : MonoBehaviour
{
    List<int> lines;
    private GC gc;
    public int columns;
    public float scale;
    public Vector3 Offset;
    public string Title;

    void Awake()
    {
        gc = GC.Instance;
    }


    public void init()
    {
        lines.Clear();
        lines.Add(0);
        transform.localRotation = Quaternion.identity;
        GameObject Found = gc.assets.MakeWordFromDiceQU(Title, Offset + transform.position, 1f);
        Found.transform.parent = transform;
        transform.localRotation = transform.parent.localRotation;
    }
    
    public void addWord(string word, string QStatus)
    {
        word = word.ToLower();
        int len = word.Length;
        if (QStatus == "qu" && word.Contains("qu")) len -= 1;
        bool found = false;
        int myRow = 0;
        int myIndex = 0;
        foreach (int ind in lines)
        {
            if (ind + len <= columns)
            {
                found = true;
                myIndex = ind;
                break;
            }
            myRow++;
        }
        if (!found)
        {
            lines.Add(len + 1);
            myRow = lines.Count-1;
            myIndex = 0;
        }
        else
        {
            lines[myRow] += len + 1;
        }
        Vector3 pos = Offset - new Vector3(0, 0, 1);
        pos += new Vector3(myIndex * scale, 0, -myRow * scale) + transform.position;
        GameObject myWord;
        if (QStatus == "qu") myWord = gc.assets.MakeWordFromDiceQU(word, pos, scale);
        else { myWord = gc.assets.MakeWordFromDiceQ(word, pos, scale); }
        transform.localRotation = Quaternion.identity;
        myWord.transform.parent = transform;
        transform.localRotation = transform.parent.localRotation;

        //GameObject newWord = gc.assets.MakeWordFromDiceQU(res, new Vector3(4.5f, 0, 5.6f - (FoundWords.Count * 0.6f)) + transform.position, 0.5f);
        //FoundList.transform.localRotation = Quaternion.identity;
        //newWord.transform.parent = FoundList.transform;
        //FoundList.transform.localRotation = transform.localRotation; // phoenix edi


    }


    // Start is called before the first frame update
    void Start()
    {
        if (gc == null) gc = GC.Instance;
        lines = new List<int>();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
