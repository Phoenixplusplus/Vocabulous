using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public TrieTest trie;
    TextMesh textMesh;
    char letter;

    void Start()
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        letter = alphabet[Random.Range(0, alphabet.Length)];
        transform.GetChild(0).GetComponent<TextMesh>().text = letter.ToString();
    }

    void OnMouseDown()
    {
        Debug.Log(letter + " clicked, adding to search");
        trie.TrieAddToSearch(letter);
    }
}
