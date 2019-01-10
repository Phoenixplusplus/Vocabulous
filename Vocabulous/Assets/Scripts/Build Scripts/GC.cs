using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GC : MonoBehaviour
{
    [Header("This won't stay empty for long")]
    public int forKickOff = 0;


    // --------------------//
    // establish Singelton //
    // ------------------- //
    public static GC Instance
    {
        get
        {
            return instance;
        }
    }
    private static GC instance = null;
    void Awake()
    {
        if (instance)
        {
            Debug.Log("Already a GameController - going to die now .....");
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //---------------------------//
    // Finished Singelton set up //
    // --------------------------//


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
