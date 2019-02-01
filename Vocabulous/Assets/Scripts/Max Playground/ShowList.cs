using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Vector3 pos = new Vector3(origin.x + (count * diceScale), origin.y + (diceScale / 2), origin.z + (row * pitch * mod));
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
            count += len;
        }
    }

}
