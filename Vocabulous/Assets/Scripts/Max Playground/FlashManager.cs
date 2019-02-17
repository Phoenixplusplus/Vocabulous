using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FlashManager : MonoBehaviour
{
    private Text[] Boxes;
    public Text Pos0;
    public Text Pos1;
    public Text Pos2;
    public Text Pos3;
    public Text Pos4;
    public Text Pos5;
    public Text Pos6;
    public Text Pos7;
    public Text Pos8;

    public int DefaultBox;
    public int StartFontSize;
    public int FinalFontSize;
    public Color FontColor;
    public float AnimTime;


    public void Flash(string message, int box)
    {
        Boxes[box].text = message;
        Boxes[box].fontSize = StartFontSize;
        StartCoroutine("DoFlash", box);
    }

    IEnumerator DoFlash (int box)
    {
        float scale = (FinalFontSize - StartFontSize) / AnimTime;
        while (Boxes[box].fontSize < FinalFontSize)
        {
            Boxes[box].fontSize = (int)(Boxes[box].fontSize+(scale * Time.deltaTime));
            yield return null;
        }
        Boxes[box].text = "";
    }

    // Start is called before the first frame update
    void Start()
    {
        Boxes = new Text[] {Pos0, Pos1, Pos2, Pos3, Pos4, Pos5, Pos6, Pos7, Pos8 };
    }


}
