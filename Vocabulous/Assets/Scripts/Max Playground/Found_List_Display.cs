//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

// Table furniture sub-controller used by WordDice
// Seperate class, as it's handy and might want to be used elsewhere (I'm good like that, thanks me laterz)
// Fits words into a set of rows of Inspector determined width
// will fill words into "gaps" to save space
public class Found_List_Display : MonoBehaviour
{
    #region Member Declaration
    List<int> lines;
    private GC gc;
    public int columns;
    public float scale;
    public Vector3 Offset;
    public string Title;
    #endregion

    #region Unity API
    void Awake()
    {
        gc = GC.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gc == null) gc = GC.Instance;
        lines = new List<int>();
    }

    public void init()
    {
        lines.Clear();
        lines.Add(0);
        transform.localRotation = Quaternion.identity;
        GameObject Found = gc.assets.MakeWordFromDiceQU(Title, Offset + transform.position + new Vector3(0,0.25f,0), 1f);
        Found.transform.parent = transform;
        transform.localRotation = transform.parent.localRotation;
    }
    #endregion

    // Public method -  send it a word string ... (n.b. QStatus "Qu" will use that dice)
    // establishes where to put it ... and instantates it
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
    }

}
