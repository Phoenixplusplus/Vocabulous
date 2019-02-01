using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnimator : MonoBehaviour
{
    TextMesh thisText;

    // Start is called before the first frame update
    void Start()
    {
        thisText = gameObject.GetComponent<TextMesh>();
    }

    public void WriteWord(string str, float letterTime) { StartCoroutine(WriteWordIE(str, letterTime)); }
    public void ScrubWord(float letterTime) { StartCoroutine(ScrubWordIE(letterTime)); }
    public string GetWord() { return thisText.text; }
    public void SetWord(string str) { thisText.text = str; }

    IEnumerator WriteWordIE(string str, float letterTime)
    {
        thisText.text = "";
        for (int i = 0; i < str.Length; i++)
        {
            thisText.text += str[i];
            yield return new WaitForSeconds(letterTime);
        }
        yield break;
    }

    IEnumerator ScrubWordIE(float letterTime)
    {
        for (int i = thisText.text.Length; i > -1; i--)
        {
            thisText.text = thisText.text.Substring(0, i);
            yield return new WaitForSeconds(letterTime);
        }
        yield break;
    }
}
