using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurAssets : MonoBehaviour
{
    // Purely a Place holder for all pre-fabs, textures etc assets that might need to be called by other items
    [Header("Old School Overlay Tile")]
    public GameObject OLayTile;
    [Header("Dice in format Dx (n.b. _ = QMark)")]
    public GameObject DArrow;
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
    [Header("Tiles in format Tx (n.b. _ = QMark)")]
    public GameObject Taa;
    public GameObject Tbb;
    public GameObject Tcc;
    public GameObject Tdd;
    public GameObject Tee;
    public GameObject Tff;
    public GameObject Tgg;
    public GameObject Thh;
    public GameObject Tii;
    public GameObject Tjj;
    public GameObject Tkk;
    public GameObject Tll;
    public GameObject Tmm;
    public GameObject Tnn;
    public GameObject Too;
    public GameObject Tpp;
    public GameObject Tqq;
    public GameObject Tququ;
    public GameObject Trr;
    public GameObject Tss;
    public GameObject Ttt;
    public GameObject Tuu;
    public GameObject Tvv;
    public GameObject Tww;
    public GameObject Txx;
    public GameObject Tyy;
    public GameObject Tzz;
    public GameObject Ta_;
    public GameObject Tb_;
    public GameObject Tc_;
    public GameObject Td_;
    public GameObject Te_;
    public GameObject Tf_;
    public GameObject Tg_;
    public GameObject Th_;
    public GameObject Ti_;
    public GameObject Tj_;
    public GameObject Tk_;
    public GameObject Tl_;
    public GameObject Tm_;
    public GameObject Tn_;
    public GameObject To_;
    public GameObject Tp_;
    public GameObject Tq_;
    public GameObject Tqu_;
    public GameObject Tr_;
    public GameObject Ts_;
    public GameObject Tt_;
    public GameObject Tu_;
    public GameObject Tv_;
    public GameObject Tw_;
    public GameObject Tx_;
    public GameObject Ty_;
    public GameObject Tz_;
    public GameObject TArrow;
    public GameObject TBlank_;
    public GameObject QuestQuest;

    // makes a "word" from a string ... USES seperate Q and U tiles
    public GameObject MakeWordFromDiceQ (string word, Vector3 Position, float Scale)
    {
        word = word.ToLower();
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

    public GameObject MakeWordFromDiceQU(string word, Vector3 Position, float Scale)
    {
        word = word.ToLower();
        GameObject myWord = new GameObject();
        myWord.transform.position = Position;
        int q = 0;
        for (var i = 0; i < word.Length; i++)
        {
            GameObject letter;
            if (word[i] == 'q' && i < word.Length-1 && word[i+1] == 'u')
            {
                letter = SpawnDice("qu", new Vector3(Position.x + q, Position.y, Position.z));
                i++;
            }
            else
            {
                letter = SpawnDice("" + word[i], new Vector3(Position.x + q, Position.y, Position.z));
            }
           letter.transform.parent = myWord.transform;
            q++;
        }
        myWord.transform.localScale = new Vector3(Scale, Scale, Scale);
        return myWord;
    }

    public GameObject MakeWordFromTiles (string word, Vector3 Position, float Scale, bool QuestionSet, bool vertical, bool forward)
    {
        string myWord = word.ToLower();
        GameObject ret = new GameObject();
        ret.transform.position = Position;
        for (int i = 0; i < myWord.Length; i++)
        {
            GameObject letter;
            string add = ""+ myWord[i];
            if (QuestionSet) add = "_";
            letter = SpawnTile(myWord[i] + add, new Vector3(i, 0, 0), vertical, forward);
            letter.transform.parent = ret.transform;
        }
        ret.transform.localScale = new Vector3(Scale, Scale, Scale);
        return ret;
    }


    public GameObject SpawnDice (string face, Vector3 position)
    {
        string myFace = face.ToLower();
        GameObject dice;
        switch (myFace)
        {
            case "arrow":
                dice = Instantiate(DArrow, position, Quaternion.identity); break;
            case "blank":
                dice = Instantiate(DBlank, position, Quaternion.identity); break;
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

    // Spawns a game Tile (based on string)
    // these default to vertical and forward facing ... so set vertical and forward to re-orientate
    public GameObject SpawnTile(string face, Vector3 position, bool vertical, bool forward)
    {
        string myFace = face.ToLower();
        GameObject tile;
        switch (myFace)
        {
            case "questquest":
                tile = Instantiate(QuestQuest, position, Quaternion.identity); break;
            case "arrow":
                tile = Instantiate(TArrow, position, Quaternion.identity); break;
            case "aa": 
                tile = Instantiate(Taa, position, Quaternion.identity); break;
            case "bb":
                tile = Instantiate(Tbb, position, Quaternion.identity); break;
            case "cc":
                tile = Instantiate(Tcc, position, Quaternion.identity); break;
            case "dd":
                tile = Instantiate(Tdd, position, Quaternion.identity); break;
            case "ee":
                tile = Instantiate(Tee, position, Quaternion.identity); break;
            case "ff":
                tile = Instantiate(Tff, position, Quaternion.identity); break;
            case "gg":
                tile = Instantiate(Tgg, position, Quaternion.identity); break;
            case "hh":
                tile = Instantiate(Thh, position, Quaternion.identity); break;
            case "ii":
                tile = Instantiate(Tii, position, Quaternion.identity); break;
            case "jj":
                tile = Instantiate(Tjj, position, Quaternion.identity); break;
            case "kk":
                tile = Instantiate(Tkk, position, Quaternion.identity); break;
            case "ll":
                tile = Instantiate(Tll, position, Quaternion.identity); break;
            case "mm":
                tile = Instantiate(Tmm, position, Quaternion.identity); break;
            case "nn":
                tile = Instantiate(Tnn, position, Quaternion.identity); break;
            case "oo":
                tile = Instantiate(Too, position, Quaternion.identity); break;
            case "pp":
                tile = Instantiate(Tpp, position, Quaternion.identity); break;
            case "qq":
                tile = Instantiate(Tqq, position, Quaternion.identity); break;
            case "ququ":
                tile = Instantiate(Tququ, position, Quaternion.identity); break;
            case "rr":
                tile = Instantiate(Trr, position, Quaternion.identity); break;
            case "ss":
                tile = Instantiate(Tss, position, Quaternion.identity); break;
            case "tt":
                tile = Instantiate(Ttt, position, Quaternion.identity); break;
            case "uu":
                tile = Instantiate(Tuu, position, Quaternion.identity); break;
            case "vv":
                tile = Instantiate(Tvv, position, Quaternion.identity); break;
            case "ww":
                tile = Instantiate(Tww, position, Quaternion.identity); break;
            case "xx":
                tile = Instantiate(Txx, position, Quaternion.identity); break;
            case "yy":
                tile = Instantiate(Tyy, position, Quaternion.identity); break;
            case "zz":
                tile = Instantiate(Tzz, position, Quaternion.identity); break;

            case "a_":
                tile = Instantiate(Ta_, position, Quaternion.identity); break;
            case "b_":
                tile = Instantiate(Tb_, position, Quaternion.identity); break;
            case "c_":
                tile = Instantiate(Tc_, position, Quaternion.identity); break;
            case "d_":
                tile = Instantiate(Td_, position, Quaternion.identity); break;
            case "e_":
                tile = Instantiate(Te_, position, Quaternion.identity); break;
            case "f_":
                tile = Instantiate(Tf_, position, Quaternion.identity); break;
            case "g_":
                tile = Instantiate(Tg_, position, Quaternion.identity); break;
            case "h_":
                tile = Instantiate(Th_, position, Quaternion.identity); break;
            case "i_":
                tile = Instantiate(Ti_, position, Quaternion.identity); break;
            case "j_":
                tile = Instantiate(Tj_, position, Quaternion.identity); break;
            case "k_":
                tile = Instantiate(Tk_, position, Quaternion.identity); break;
            case "l_":
                tile = Instantiate(Tl_, position, Quaternion.identity); break;
            case "m_":
                tile = Instantiate(Tm_, position, Quaternion.identity); break;
            case "n_":
                tile = Instantiate(Tn_, position, Quaternion.identity); break;
            case "o_":
                tile = Instantiate(To_, position, Quaternion.identity); break;
            case "p_":
                tile = Instantiate(Tp_, position, Quaternion.identity); break;
            case "q_":
                tile = Instantiate(Tq_, position, Quaternion.identity); break;
            case "qu_":
                tile = Instantiate(Tqu_, position, Quaternion.identity); break;
            case "r_":
                tile = Instantiate(Tr_, position, Quaternion.identity); break;
            case "s_":
                tile = Instantiate(Ts_, position, Quaternion.identity); break;
            case "t_":
                tile = Instantiate(Tt_, position, Quaternion.identity); break;
            case "u_":
                tile = Instantiate(Tu_, position, Quaternion.identity); break;
            case "v_":
                tile = Instantiate(Tv_, position, Quaternion.identity); break;
            case "w_":
                tile = Instantiate(Tw_, position, Quaternion.identity); break;
            case "x_":
                tile = Instantiate(Tx_, position, Quaternion.identity); break;
            case "y_":
                tile = Instantiate(Ty_, position, Quaternion.identity); break;
            case "z_":
                tile = Instantiate(Tz_, position, Quaternion.identity); break;

            default: tile = Instantiate(TBlank_, position, Quaternion.identity); break;
        }
        if (tile == null) Debug.Log("OurAssets::Spawn Tile() - Improper String parameter");
        Con_Tile2 tc = tile.GetComponent<Con_Tile2>();
        if (tc != null) tc.FlipTo(vertical, forward);
        return tile;
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

    // Takes a list of strings, returns it in decending length order
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
