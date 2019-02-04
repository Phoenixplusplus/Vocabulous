using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConAnagram : MonoBehaviour
{
    private GC gc;
    public string Anagram;
    private List<string> AnswersList;
    public GameObject AnswersDisplay;
    public GameObject TilesDisplay;

    // Start is called before the first frame update
    void Start()
    {
        gc = GC.Instance;
        AnswersList = new List<string>();
        StartGame();
    }

    void StartGame ()
    {
        Anagram = gc.maxTrie.GetRandomWord(6);
        Debug.Log(Anagram);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
