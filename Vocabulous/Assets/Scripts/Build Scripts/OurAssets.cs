using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurAssets : MonoBehaviour
{
    // Purely a Place holder for all pre-fabs, textures etc assets that might need to be called by other items
    [Header("Old School Overlay Tile")]
    public GameObject OLayTile;
    [Header("Dice in format Dx")]
    public GameObject D_;
    public GameObject DQMark;
    public GameObject DBlank;
    public GameObject Da;
    public GameObject Db;
    public GameObject Dc;
    public GameObject Dd;
    public GameObject De;
    public GameObject Df;
    public GameObject Dg;
    public GameObject Dh;
    public GameObject Di;
    public GameObject Dj;
    public GameObject Dk;
    public GameObject Dl;
    public GameObject Dm;
    public GameObject Dn;
    public GameObject Do;
    public GameObject Dp;
    public GameObject Dq;
    public GameObject Dqu;
    public GameObject Dr;
    public GameObject Ds;
    public GameObject Dt;
    public GameObject Du;
    public GameObject Dv;
    public GameObject Dw;
    public GameObject Dx;
    public GameObject Dy;
    public GameObject Dz;
    [Header("Tiles in format Tx")]
    public GameObject Ta;
    public GameObject Tb;
    public GameObject Tc;
    public GameObject Td;
    public GameObject Te;
    public GameObject Tf;
    public GameObject Tg;
    public GameObject Th;
    public GameObject Ti;
    public GameObject Tj;
    public GameObject Tk;
    public GameObject Tl;
    public GameObject Tm;
    public GameObject Tn;
    public GameObject To;
    public GameObject Tp;
    public GameObject Tq;
    public GameObject Tqu;
    public GameObject Tr;
    public GameObject Ts;
    public GameObject Tt;
    public GameObject Tu;
    public GameObject Tv;
    public GameObject Tw;
    public GameObject Tx;
    public GameObject Ty;
    public GameObject Tz;

    public GameObject MakeWordFromDiceQ (string word, Vector3 Position, float Scale)
    {
        GameObject myWord = new GameObject();
        myWord.transform.position = Position;
        for (var i = 0; i < word.Length; i++)
        {
            GameObject letter = SpawnDice(""+word[i], new Vector3(Position.x + i, Position.y, Position.z));
            letter.transform.parent = myWord.transform;
        }
        myWord.transform.localScale = new Vector3(Scale, Scale, Scale);
        return myWord;
    }


    public GameObject SpawnDice (string face, Vector3 position)
    {
        string myFace = face.ToLower();
        GameObject dice;
        switch (myFace)
        {
            case "?":
                dice = Instantiate(DQMark, position, Quaternion.identity); break;
            case "_":
                dice = Instantiate(D_, position, Quaternion.identity); break;
            case "a":
                dice = Instantiate(Da, position, Quaternion.identity); break;
            case "b":
                dice = Instantiate(Db, position, Quaternion.identity); break;
            case "c":
                dice = Instantiate(Dc, position, Quaternion.identity); break;
            case "d":
                dice = Instantiate(Dd, position, Quaternion.identity); break;
            case "e":
                dice = Instantiate(De, position, Quaternion.identity); break;
            case "f":
                dice = Instantiate(Df, position, Quaternion.identity); break;
            case "g":
                dice = Instantiate(Dg, position, Quaternion.identity); break;
            case "h":
                dice = Instantiate(Dh, position, Quaternion.identity); break;
            case "i":
                dice = Instantiate(Di, position, Quaternion.identity); break;
            case "j":
                dice = Instantiate(Dj, position, Quaternion.identity); break;
            case "k":
                dice = Instantiate(Dk, position, Quaternion.identity); break;
            case "l":
                dice = Instantiate(Dl, position, Quaternion.identity); break;
            case "m":
                dice = Instantiate(Dm, position, Quaternion.identity); break;
            case "n":
                dice = Instantiate(Dn, position, Quaternion.identity); break;
            case "o":
                dice = Instantiate(Do, position, Quaternion.identity); break;
            case "p":
                dice = Instantiate(Dp, position, Quaternion.identity); break;
            case "q":
                dice = Instantiate(Dq, position, Quaternion.identity); break;
            case "qu":
                dice = Instantiate(Dqu, position, Quaternion.identity); break;
            case "r":
                dice = Instantiate(Dr, position, Quaternion.identity); break;
            case "s":
                dice = Instantiate(Ds, position, Quaternion.identity); break;
            case "t":
                dice = Instantiate(Dt, position, Quaternion.identity); break;
            case "u":
                dice = Instantiate(Du, position, Quaternion.identity); break;
            case "v":
                dice = Instantiate(Dv, position, Quaternion.identity); break;
            case "w":
                dice = Instantiate(Dw, position, Quaternion.identity); break;
            case "x":
                dice = Instantiate(Dx, position, Quaternion.identity); break;
            case "y":
                dice = Instantiate(Dy, position, Quaternion.identity); break;
            case "z":
                dice = Instantiate(Dz, position, Quaternion.identity); break;
            default: dice = Instantiate(DBlank, position, Quaternion.identity); break;
        }
        if (dice == null) Debug.Log("OurAssets::Spawn Dice() - Improper String parameter");
        return dice;
    }

    // returns a random opaque color
    public Color GetRandomColor()
    {
        Color ret = new Color();
        ret.r = Random.Range(0, 256);
        ret.g = Random.Range(0, 256);
        ret.b = Random.Range(0, 256);
        ret.a = 1;
        return ret;
    }

    public List<string> SortList(List<string> list)
    {
        List<string> ret = new List<string>();
        int min = 99;
        int max = 0;
        foreach (string s in list)
        {
            int len = s.Length;
            if (len < min) min = len;
            if (len > max) max = len;
        }
        for (int i = max; i >= min; i--)
        {
            foreach (string s in list)
            {
                if (s.Length == i) ret.Add(s);
            }
        }
        return ret;
    }


}
