using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC : MonoBehaviour
{
    public int maximumTurns, currentTurn;
    public TrieTest Trie;
    public List<CubeScript> cubesList = new List<CubeScript>();
    public List<GameObject> SelectedCubes = new List<GameObject>();
    public List<GameObject> NeighbourCubes = new List<GameObject>();
    public Color cubeDefaultColour = new Color(0.824f, 0.412f, 0.118f);
    public Color cubeNeighbourColour = new Color(0, 0, 1);
    public Color cubeSelectedColour = new Color(1, 0, 0);

    void Start()
    {
        /* populate cubes in level because I'm too lazy to drag n drop :( */
        foreach(GameObject cube in GameObject.FindGameObjectsWithTag("Cube"))
        {
            cubesList.Add(cube.GetComponent<CubeScript>());
        }
    }

    /* inputs */
    void Update()
    {
        /* right click */
        if (Input.GetMouseButtonDown(1))
        {
            /* search Trie and store in bool */
            bool success = Trie.TrieSearch(false, false, false, 0, true);

            /* reset cubes back to default settings
             * if found a word, start coroutine on those cubes */
            foreach (CubeScript cube in cubesList)
            {
                if (success && cube.wasSelected) StartCoroutine(cube.cubeFoundWord(1f));
                cube.wasSelected = false;
                cube.gameObject.GetComponent<Renderer>().material.color = cubeDefaultColour;
            }
            /* clear cube lists after search */
            SelectedCubes.Clear();
            NeighbourCubes.Clear();
            /* rest current turn */
            currentTurn = 0;
        }
    }

    /* called by cubes */
    /* reset colours of neighbours (unless was selected) and clears the NeighbourCubes list */
    public void NeighboursDelete()
    {
        foreach (GameObject cube in NeighbourCubes)
        {
            if (!cube.GetComponent<CubeScript>().wasSelected) cube.GetComponent<Renderer>().material.color = cubeDefaultColour;
        }
        NeighbourCubes.Clear();
    }

    /* set colours of neighbours (unless was selected) of cubes in NeighbourCubes list. List is populated when a cube is clicked, by the cube */
    public void NeighboursNew()
    {
        foreach (GameObject cube in NeighbourCubes)
        {
            if (!cube.GetComponent<CubeScript>().wasSelected) cube.GetComponent<Renderer>().material.color = cubeNeighbourColour;
        }
    }
}
