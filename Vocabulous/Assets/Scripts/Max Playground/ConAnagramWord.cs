using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConAnagramWord : MonoBehaviour
{

    public string myWord;
    [SerializeField]
    private List<Con_Tile2> myTiles;
    [SerializeField]
    private bool animating = false;

    void Awake()
    {
        myTiles = new List<Con_Tile2>();
        foreach (Transform child in transform)
        {
            myTiles.Add(child.GetComponent<Con_Tile2>());
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    public void Roll (float gap)
    {
        if (animating)
        {
            Debug.Log("This word is already animating - so leaving now");
            return;
        }
        animating = true;
        StartCoroutine("myRoll",gap);
    }

    IEnumerator myRoll (float gap)
    {
        foreach (Con_Tile2 c in myTiles)
        {
            if (!c.forward) c.Roll(0.5f);
            yield return new WaitForSeconds(gap);
        }
    }

    public void RevealHint ()
    {
        List<int> pos = new List<int>();
        for (int i = 0; i < myTiles.Count; i++)
        {
            if (!myTiles[i].forward) pos.Add(i);
        }
        if (pos.Count <= 1)
        {
            Debug.Log("Cannot give hint, enough letters of this word revealed");
        }
        else
        {
            myTiles[pos[Random.Range(0, pos.Count)]].Roll(0.5f);
        }
    }

    public bool IsHintable ()
    {
        int pos = 0;
        for (int i = 0; i < myTiles.Count; i++)
        {
            if (!myTiles[i].forward) pos++;
        }
        return pos > 1;
    }


    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
