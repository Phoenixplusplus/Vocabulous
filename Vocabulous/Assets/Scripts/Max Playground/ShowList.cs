//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;


// Helper script (used by WordDice for final "could have had" display)
// instantates words made up of dice and displays them
// populate by feeding a List<string> into "print()" (n.b. List is justified-middle)
// destroy with "clear()"
public class ShowList : MonoBehaviour
{
    private GC gc;
    public string EmptyListReturn;
    private List<string> myEmptyWarning = new List<string>();
    public int lineLength;
    public float diceScale;
    public float pitch;
    public Vector3 origin;
    public bool up;
    [SerializeField]
    private List<float> rowoffset;

    void Awake()
    {
        gc = GC.Instance;
        if (EmptyListReturn != null)
        {
            string[] split = EmptyListReturn.Split(" "[0]);
            myEmptyWarning.Clear();
            foreach (string s in split)
            {
                myEmptyWarning.Add(s);
            }
        }
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Print(List<string> list, string Qstatus)
    {
        Clear();
        if (gc == null) gc = GC.Instance;

        if (list.Count == 0) list = myEmptyWarning;

        int mod = 1;
        if (!up) mod = -1;
        int count = 0;
        int row = 0;

        rowoffset = new List<float>();
        rowoffset.Add(0f);
        for (int i = 0; i < list.Count; i++)
        {
            int len = list[i].Length;
            if (count + len >= lineLength)
            {
                rowoffset.Add(0f);
                row++;
                count = 0;
            }
            count += len + 1;
            rowoffset[row] = (((float)lineLength - (float)count-1) / 2f);
        }

        count = 0;
        row = 0;

        foreach (string s in list)
        {
            int len = s.Length;
            if (count + len + 1 > lineLength)
            {
                count = 0;
                row++;
            }
            else if (count != 0)
            {
                count += 1;
            }
            Vector3 pos = new Vector3(origin.x + (count * diceScale) + (rowoffset[row] * diceScale), origin.y - (diceScale / 2), origin.z + (row * pitch * mod));
            GameObject dice;
            if (Qstatus == "q")
            {
                dice = gc.assets.MakeWordFromDiceQ(s, Vector3.zero, diceScale);
            }
            else
            {
                dice = gc.assets.MakeWordFromDiceQU(s, Vector3.zero, diceScale);
            }
            dice.transform.parent = transform;
            dice.transform.localPosition = pos;
            dice.transform.localRotation = Quaternion.identity;
            count += len;
        }
    }

}
