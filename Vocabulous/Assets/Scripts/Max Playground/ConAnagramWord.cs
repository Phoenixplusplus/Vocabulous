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

    // Start is called before the first frame update
    void Start()
    {
        myTiles = new List<Con_Tile2>();
        foreach (Transform child in transform)
        {
            myTiles.Add(child.GetComponent<Con_Tile2>());
        }
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
            c.Roll(0.5f);
            yield return new WaitForSeconds(gap);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
