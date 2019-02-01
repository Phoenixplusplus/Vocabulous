using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnimator : MonoBehaviour
{
    TextMesh thisText;
    public GameObject scrubber;
    public Transform scrubRot;
    Vector3 scrubOriginPos;
    Quaternion scrubOriginRot;

    // Start is called before the first frame update
    void Start()
    {
        thisText = gameObject.GetComponent<TextMesh>();
        scrubOriginPos = scrubber.transform.position;
        scrubOriginRot = scrubber.transform.rotation;
    }

    public void WriteWord(string str, float totalTime) { StartCoroutine(WriteWordIE(str, totalTime)); }
    public void ScrubWord(float totalTime) { StartCoroutine(ScrubWordIE(totalTime)); }
    public void UseScrubber(float initialLerpTime) { StartCoroutine(UseScrubberIE(initialLerpTime)); }
    public string GetWord() { return thisText.text; }
    public void SetWord(string str) { thisText.text = str; }

    IEnumerator WriteWordIE(string str, float totalTime)
    {
        thisText.text = "";
        for (int i = 0; i < str.Length; i++)
        {
            thisText.text += str[i];
            yield return new WaitForSeconds(totalTime / str.Length);
        }
        yield break;
    }

    IEnumerator ScrubWordIE(float totalTime)
    {
        int len = thisText.text.Length;
        for (int i = len; i > -1; i--)
        {
            thisText.text = thisText.text.Substring(0, i);
            yield return new WaitForSeconds(totalTime / len);
        }
        yield break;
    }

    IEnumerator UseScrubberIE(float initialLerpTime)
    {
        float t = 0;
        while (t < initialLerpTime)
        {
            scrubber.transform.position = Vector3.Lerp(scrubber.transform.position, (transform.position + transform.right) - transform.up * 0.5f, t / initialLerpTime);
            scrubber.transform.rotation = Quaternion.Lerp(scrubber.transform.rotation, scrubRot.transform.rotation, t / initialLerpTime);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < initialLerpTime)
        {
            scrubber.transform.position = Vector3.Lerp(scrubber.transform.position, scrubOriginPos + new Vector3(0, .5f, 0), t / initialLerpTime);
            scrubber.transform.rotation = Quaternion.Lerp(scrubber.transform.rotation, scrubOriginRot, t / initialLerpTime);
            t += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
    {
        float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
        float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
        float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
        Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
        return Lerped;
    }
}
