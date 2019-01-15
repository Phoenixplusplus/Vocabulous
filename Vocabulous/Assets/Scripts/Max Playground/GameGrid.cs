using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid  {

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

    // Use this for initialization
    void Start ()
    {
        
    }

    public void init()
    {
        currDir = -1;
        bins.Clear();
        legals.Clear();
        path.Clear();
        for (int i = 0; i < dy * dx; i++)
        {
            if (DEBUG)
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
        if (a < 0 || a > dx * dy)
        {
            Debug.Log("GameGrid:AddToPath() - illegal for: " + a);
            return;
        }
        // Debug.Log("GameGrid:AddToPath() - attempt for: " + a);
        int c = path.Count;
        if (c == 0)
        {
            path.Add(a);
            CheckLegals(a);
        }
        else if (c > 1 && GetPathSecondFromEnd() == a)
        {
            path.RemoveAt(c - 1);
            CheckLegals(GetPathEnd());
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
            bins[bin] = value;
            //bins.Add(bin, value);
        }
    }

    public string GetCurrentPath()
    {
        string ret = "";
        foreach (int item in path)
        {
            ret = ret + bins[item];
        }
        return ret;
    }

    public void ClearPath()
    {
        path.Clear();
        legals.Clear();
    }

    public int GetPathEnd()
    {
        int len = path.Count;
        if (len == 0) return -1;
        else
        {
            return path[len - 1];
        }
    }

    public int GetPathSecondFromEnd()
    {
        int len = path.Count;
        if (len < 2 ) return -1;
        else
        {
            return path[len - 2];
        }
    }

    // Update is called once per frame
    //void Update ()
    //{ }
}
