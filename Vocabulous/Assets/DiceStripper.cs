using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceStripper : MonoBehaviour
{
    public OurAssets assets;

    // Start is called before the first frame update
    void Start()
    {
        assets.SpawnDice("a", Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
