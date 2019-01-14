﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid  {

    public int dx = 5;
    public int dy = 5;
    public bool directional = false;
    public bool diagonals = true;
    public int currDir = -1;
    public Dictionary<int, string> bins = new Dictionary<int, string>();
    public List<int> legals = new List<int>();
    public List<int> path;
    private string str = "abcdefghijklmnopqrstuvwxyz";
    private bool DEBUG = false;

    // Use this for initialization
    void Start ()
    {

    }

    public void init()
    {
        if (DEBUG) { 
            for (int i = 0; i < dx * dy; i++)
            {
                bins.Add(i, "" + str[Random.Range(0, 27)]);
            }
        }
        else
        {
            currDir = -1;
            bins.Clear();
            legals.Clear();
            path.Clear();
            for (int i = 0; i < dy * dx; i++)
            {
                bins.Add(i, "");
            }
        }
    }

    public void CheckLegals(int a)
    {

        int r = 0;

        int x = a % dx;
        int y = a % dy;

        /* move 0 (up) */
        if (y > 0)
        {
            r = a - dx;
            if (!path.Contains(r) && bins[r] != "") legals.Add(r);
        }
        /* move 1 (up, right) */
        if (y > 0 && x < dx - 2 && diagonals)
        {
            r = a - dx + 1;
            if (!path.Contains(r) && bins[r] != "")
                legals.Add(r);
        }
        /* move 2 (right) */
        if (x < dx - 2)
        {
            r = a + 1;
            if (!path.Contains(r) && bins[r] != "") legals.Add(r);
        }
        /* move 3 (down, right) */
        if (y < dy - 2 && x < dx - 2 && diagonals)
        {
            r = a + dx + 1;
            if (!path.Contains(r) && bins[r] != "") legals.Add(r);
        }
        /* move 4 (down) */
        if (y < dy - 2)
        {
            r = a + dx;
            if (!path.Contains(r) && bins[r] != "") legals.Add(r);
        }
        /* move 5 (down, left) */
        if (y < dy - 2 && x > 0 && diagonals)
        {
            r = a + dx - 1;
            if (!path.Contains(r) && bins[r] != "") legals.Add(r);
        }
        /* move 6 (left) */
        if (x > 0)
        {
            r = a - 1;
            if (!path.Contains(r) && bins[r] != "") legals.Add(r);
        }
        /* move 7 (up, left) */
        if (x > 0 && y > 0 && diagonals)
        {
            r = a - dx - 1;
            if (!path.Contains(r) && bins[r] != "") legals.Add(a - dx - 1);
        }
    }
    
    public void AddToPath(int a)
    {
        int c = path.Count;
        if (c == 0)
        {
            path.Add(a);
            CheckLegals(a);
        }
        else if (path[c - 1] == a)
        {
            path.RemoveAt(c - 1);
            CheckLegals(path[c - 1]);
            if (path.Count < 2)
            {
                currDir = -1;
            }
        }
        else if (legals.Contains(a))
        {
            path.Add(a);
            CheckLegals(a);
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
        }

    }

    public string FinishPath()
    {
        string ret = "";
        int len = path.Count;
        foreach (KeyValuePair<int, string> item in bins)
        {
            ret = ret + item.Value;
        }
        path.Clear();
        legals.Clear();
        currDir = -1;
        return ret;
    }

    public void PopulateGrid(string values)
    {
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
        if (bin >= 0 && bin <= dx * dy)
        {
            bins.Add(bin, value);
        }
    }

    public string GetCurrentPath()
    {
        string ret = "";
        int len = path.Count;
        foreach (KeyValuePair<int, string> item in bins)
        {
            ret = ret + item.Value;
        }
        return ret;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
