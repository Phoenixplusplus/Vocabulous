using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Controlller : MonoBehaviour
{

    public GameObject quad;
    public GameObject letter;
    private string rand = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public void SetLetter (string value)
    {
        letter.GetComponent<TextMesh>().text = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        SetLetter(""+rand[Random.Range(0, 26)]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
