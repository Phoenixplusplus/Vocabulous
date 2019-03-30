//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

// Move to legacy ??
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

}
