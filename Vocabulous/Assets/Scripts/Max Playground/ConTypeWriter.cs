using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConTypeWriter : MonoBehaviour
{
    [SerializeField]
    private string myWord = "";
    private GC gc;
    private TypeKey[] myKeys;
    public GameObject myInput;
    public ShowList myOutput;
    public int MyType = 1; // default, use this to customise and use for other things
    public List<string> answers;

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
        if (Input.GetMouseButtonDown(0) && gc.NewHoverOver >= 7700 && gc.NewHoverOver <= 7728)
        {
            TypeKey k = null;
            foreach (TypeKey K in myKeys)
            {
                if (K.myHoverID == gc.NewHoverOver) k = K;
            }
            if (k != null)
            {
                // check for back space
                if (k.myKey == "back")
                {
                    if (myWord.Length > 0)
                    {
                        k.press();
                        myWord = myWord.Substring(0, myWord.Length - 1);
                        ChangeInputDisplay();
                        return;
                    }
                }
                // check for find
                else if (k.myKey == "find")
                {
                    if (myWord.Length >= 3)
                    {
                        k.press();
                        HitMe(myWord);
                        return;
                    }
                }
                // else add letter
                else
                {
                    if (myWord.Length < 16)
                    {
                        k.press();
                        myWord = myWord + k.myKey;
                        ChangeInputDisplay();
                        return;
                    }
                }
            }
        }
    }

    private void ChangeInputDisplay()
    {
        myInput.transform.position = new Vector3(4.5f - ((float)myWord.Length/2), 0, 1.5f);

        foreach (Transform child in myInput.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject word = gc.assets.MakeWordFromDiceQ(myWord, myInput.transform.position, 1);
        word.transform.parent = myInput.transform;
    }

    private void HitMe(string word)
    {
        switch (MyType)
        {
            case 1:
                if (myWord.Contains(" ") || myWord.Contains("_"))
                {
                    answers = gc.maxTrie.SolveCrossword(myWord);
                }
                else
                {
                    answers = gc.maxTrie.getAnagram(myWord, false, 3);
                    answers = gc.assets.SortList(answers);
                }
                break;
            default:
                break;
        }
        DisplayAnswers();
    }

    private void DisplayAnswers()
    {
        myOutput.Print(answers,"q");
    }

}
