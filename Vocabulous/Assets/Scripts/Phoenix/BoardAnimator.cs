using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardAnimator : MonoBehaviour
{
    Text thisText;
    public GameObject scrubber, chalk;
    public Transform scrubRot, chalkRot;
    Vector3 scrubOriginPos, chalkOriginPos;
    Quaternion scrubOriginRot, chalkOriginRot;

    // Start is called before the first frame update
    void Start()
    {
        thisText = gameObject.GetComponent<Text>();
    }

    public void WriteWord(string str, float totalTime) { StartCoroutine(WriteWordIE(str, totalTime)); }
    public void ScrubWord(float totalTime) { StartCoroutine(ScrubWordIE(totalTime)); }
    public void UseScrubber(float initialLerpTime) { StartCoroutine(UseScrubberIE(initialLerpTime)); }
    public void UseChalk(float initialLerpTime) { StartCoroutine(UseChalkIE(initialLerpTime)); }
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

    IEnumerator UseChalkIE(float initialLerpTime)
    {
        chalkOriginPos = chalk.transform.position;
        chalkOriginRot = chalk.transform.rotation;
        chalk.GetComponent<Scrubber>().shaking = true;
        chalk.GetComponent<Scrubber>().ShakeScrubber(2, 0.1f, transform.up);
        float t = 0;
        while (t < initialLerpTime)
        {
            chalk.transform.position = Vector3.Lerp(chalk.transform.position, (transform.position + transform.right) - (transform.up * .8f), t / initialLerpTime);
            chalk.transform.rotation = Quaternion.Lerp(chalk.transform.rotation, chalkRot.transform.rotation, t / initialLerpTime);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        chalk.GetComponent<Scrubber>().shaking = false;
        while (t < initialLerpTime)
        {
            chalk.transform.position = Vector3.Lerp(chalk.transform.position, chalkOriginPos, t / initialLerpTime);
            chalk.transform.rotation = Quaternion.Lerp(chalk.transform.rotation, chalkOriginRot, t / initialLerpTime);
            t += Time.deltaTime;
            yield return null;
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
        scrubOriginPos = scrubber.transform.position;
        scrubOriginRot = scrubber.transform.rotation;
        scrubber.GetComponent<Scrubber>().shaking = true;
        scrubber.GetComponent<Scrubber>().ShakeScrubber(2, 0.1f, transform.up);
        float t = 0;
        while (t < initialLerpTime)
        {
            scrubber.transform.position = Vector3.Lerp(scrubber.transform.position, (transform.position + transform.right * .5f) - transform.up * .5f, t / initialLerpTime);
            scrubber.transform.rotation = Quaternion.Lerp(scrubber.transform.rotation, scrubRot.transform.rotation, t / initialLerpTime);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        scrubber.GetComponent<Scrubber>().shaking = false;
        while (t < initialLerpTime)
        {
            scrubber.transform.position = Vector3.Lerp(scrubber.transform.position, scrubOriginPos, t / initialLerpTime);
            scrubber.transform.rotation = Quaternion.Lerp(scrubber.transform.rotation, scrubOriginRot, t / initialLerpTime);
            t += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
