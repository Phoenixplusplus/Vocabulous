using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Game_Controller : MonoBehaviour
{
    public GameObject OverlayPrefab;
    private GameGrid grid;
    private MaxTrie trie;


    // Start is called before the first frame update
    void Start()
    {
        trie = GetComponent<MaxTrie>();
        grid = new GameGrid() { dx = 4, dy = 4 };
        Debug.Log("New Grid x: " + grid.dx.ToString() + " y: " + grid.dy.ToString());
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Instantiate(OverlayPrefab, new Vector3(i, j, 0),Quaternion.identity); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
