using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFiller : MonoBehaviour
{
    public List<int> GetWSPaths(int wordLength, int GridX, int GridY)
    {
        // Uses a "move" system defined as
        //  7  0  1
        //   \ | /
        //  6- a -2
        //   / | \
        //  5  4  3
        // any particular "cell" at position X,Y is defined as (Y * GridX) + X

        List<int> ret = new List<int>();
        for (int y = 0; y < GridY; y++)
        {
            for (int x = 0; x < GridX; x++)
            {
                int cell = (y * GridX) + x;
                // dir 0
                if (y > wordLength) { ret.Add(cell); ret.Add(0); }
                // dir 1
                if (y > wordLength && (x + wordLength) < GridX) { ret.Add(cell);ret.Add(1); }
                // etc etc for dir 2 - 7
            }
        }
        return ret;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
