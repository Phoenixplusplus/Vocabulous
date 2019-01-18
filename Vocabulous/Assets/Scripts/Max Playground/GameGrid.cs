﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid  {
    // GAMEGRID CLASS
    // Created and initialised by an "game"
    // is populated with the game "letters" (held in Dictionary bins by number and String Value)
    // Grids numbered from 0 - whatever, starting top left to right and down, e.g.
    // -----------------
    // | 0 | 1 | 2 | 3 |
    // |---|---|---|---|
    // | 4 | 5 | 6 | 7 |
    // |---|---|---|---|
    // | 8 | 9 |10 |11 |
    // |---|---|---|---|
    // Tracks player "path" through the grid
    // Establishes "legal" moves from end of path to next cell (based upon rules set at Initialisation)
    // Returns data to the "game" upon request

    public int dx = 5;
    public int dy = 5;
    public bool directional = false;
    public bool diagonals = true;
    public int currDir = -1;
    public Dictionary<int, string> bins = new Dictionary<int, string>();
    public List<int> legals = new List<int>();
    public List<int> path = new List<int>();
    private string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public bool DEBUG = false;

    // Use this for initialization ... not used, retained "just in case"
    // void Start() { }

    public void init()
    {
        // Call this to initialise (and subsequently reset) the GameGrid
        currDir = -1;
        bins.Clear();
        legals.Clear();
        path.Clear();
        for (int i = 0; i < dy * dx; i++) // needs to be "full" even if with empty strings, else get reference errors
        {
            if (DEBUG) // for test purposes, populates the bins (all of them) with a random letter
            {
                bins.Add(i, "" + str[Random.Range(0, 26)]);
            }
            else
            {
                bins.Add(i, "");
            }
        }
    }

    public void CheckLegals(int a)
    {
        // populates the legals<int> list with legal moves from the passed (a) cell
        // based upon cells already on the path and the game rules 
        // Uses a "move" system defined as
        //  7  0  1
        //   \ | /
        //  6- a -2
        //   / | \
        //  5  4  3

        legals.Clear();
        int r = 0;
        int x = a % dx;
        int y = (a - x)/dy;

        /* move 0 (up) */
        if (y > 0)
        {
            r = a - dx;
            if (!path.Contains(r) && bins[r] != "" && (!directional || currDir == -1 || currDir == 0) )
                legals.Add(r); 
        }
        /* move 1 (up, right) */
        if (y > 0 && x < dx - 1 && diagonals)
        {
            r = a - dx + 1;
            if (!path.Contains(r) && bins[r] != "" && (!directional || currDir == -1 || currDir == 1))
                legals.Add(r);
        }
        /* move 2 (right) */
        if (x < dx - 1)
        {
            r = a + 1;
            if (!path.Contains(r) && bins[r] != "" && (!directional || currDir == -1 || currDir == 2))
                legals.Add(r);
        }
        /* move 3 (down, right) */
        if (y < dy - 1 && x < dx - 1 && diagonals)
        {
            r = a + dx + 1;
            if (!path.Contains(r) && bins[r] != "" && (!directional || currDir == -1 || currDir == 3))
                legals.Add(r);
        }
        /* move 4 (down) */
        if (y < dy - 1)
        {
            r = a + dx;
            if (!path.Contains(r) && bins[r] != "" && (!directional || currDir == -1 || currDir == 4))
                legals.Add(r);
        }
        /* move 5 (down, left) */
        if (y < dy - 1 && x > 0 && diagonals)
        {
            r = a + dx - 1;
            if (!path.Contains(r) && bins[r] != "" && (!directional || currDir == -1 || currDir == 5))
                legals.Add(r);
        }
        /* move 6 (left) */
        if (x > 0)
        {
            r = a - 1;
            if (!path.Contains(r) && bins[r] != "" && (!directional || currDir == -1 || currDir == 6))
                legals.Add(r);
        }
        /* move 7 (up, left) */
        if (x > 0 && y > 0 && diagonals)
        {
            r = a - dx - 1;
            if (!path.Contains(r) && bins[r] != "" && (!directional || currDir == -1 || currDir == 7))
                legals.Add(r);
        }
    }
    
    public void AddToPath(int a)
    {
        // Called by any "game" to add a cell to the path

        // of "out-of bounds" ... not getting in
        if (a < 0 || a > dx * dy) 
        {
            Debug.Log("GameGrid:AddToPath() - illegal for: " + a);
            return;
        }

        int c = path.Count;
        // First node on path ... add and populate legals
        if (c == 0)
        {
            path.Add(a);
            CheckLegals(a);
        }
        // "rollback" functionality, if a == second from end, delete path AND a from it
        // "game" needs to check if it wants to do this
        else if (c > 1 && GetPathSecondFromEnd() == a)
        {
            path.RemoveAt(c - 1);
            if (path.Count < 2)
            {
                currDir = -1;
            }
            CheckLegals(GetPathEnd());
        }
        // Only adds a path node if it's on the legals list
        else if (legals.Contains(a))
        {
            path.Add(a);
            // establish the current direction 
            if (path.Count == 2 && directional)
            {
                int d = path[0] - path[1];
                if (d == dx) currDir = 0;
                else if (d == dx -1) currDir = 1;
                else if (d == - 1) currDir = 2;
                else if (d == -dx - 1) currDir = 3;
                else if (d == -dx ) currDir = 4;
                else if (d == -dx + 1) currDir = 5;
                else if (d ==  1) currDir = 6;
                else if (d == dx + 1) currDir = 7;
            }
            CheckLegals(a);
        }
    }

    public string FinishPath()
    {
        // To be called by "game"
        // returns a string of the selected characters
        // game needs to "length check" it
        // current path (and direction) is then cleared
        string ret = "";
        foreach (int item in path)
        {
            ret = ret + bins[item];
        }
        path.Clear();
        legals.Clear();
        currDir = -1;
        return ret;
    }

    public void PopulateGrid(string values)
    {
        // FULLY populates a grid with a string sequence of character
        // length of which MUST match the size of the grid
        int len = dx * dy;
        if (values.Length != len) Debug.Log("Grid:Populate() -> Wrong length of values string");
        else
        {
            for (int i = 0; i < len; i++)
            {
                bins.Add(i, "" + values[i]);
            }
        }
    }

    public void PopulateBin(int bin, string value)
    {
        // Populates a particular "bin" witha string (i.e. letter or "")
        if (bin >= 0 && bin <= dx * dy)
        {
            bins[bin] = value;
        }
    }

    public string GetCurrentPath()
    {
        // returns a String reflecting the letters (words) on the current path
        // path itself is not modified
        string ret = "";
        foreach (int item in path)
        {
            ret = ret + bins[item];
        }
        return ret;
    }

    public void ClearPath()
    {
        // Clears the current path (and resets legals and direction)
        path.Clear();
        legals.Clear();
        currDir = -1;
    }

    public int GetPathEnd()
    {
        // returns the LAST cell ID of the path
        int len = path.Count;
        if (len == 0) return -1;
        else
        {
            return path[len - 1];
        }
    }

    public int GetPathSecondFromEnd()
    {
        // returns the SECOND LAST cell ID of the path
        int len = path.Count;
        if (len < 2 ) return -1;
        else
        {
            return path[len - 2];
        }
    }

}
