using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConTypeWriter : MonoBehaviour
{
    [SerializeField]
    private string myWord = "";
    private GC gc;
    private TypeKey[] myKeys;

    // Code Time
    // Q:7700, W:7701, E:7702, R:7703, T:7704, Y:7705, U:7706, I:7707, O:7708, P7709:
    // A:7710, S:7711, D:7712, F:7713, G:7714, H:7715, J:7716, K:7717, L:7718
    // Z:7719, X:7720, C:7721, V:7722, B:7723, N:7724, M:7725
    // Space:7726, Back:7727, Find:7728

    void Awake()
    {
        gc = GC.Instance;
        if (gc != null) Debug.Log("TypeWriter Connected to GC");
        myKeys = GetComponentsInChildren<TypeKey>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TypeKey k = null;
            foreach (TypeKey K in myKeys)
            {
                if (K.myHoverID == gc.NewHoverOver) k = K;
            }
            if (k != null)
            {
                k.press();
                // check for back space
                // check for find
                // else add letter
                myWord = myWord + k.myKey;
            }
        }
    }
}
