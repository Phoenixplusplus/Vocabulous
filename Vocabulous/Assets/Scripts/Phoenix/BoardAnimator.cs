//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardAnimator : MonoBehaviour
{
    Text thisText;
    GC gameController;
    public GameObject scrubber, chalk;
    public Transform scrubRot, chalkRot;
    Vector3 scrubOriginPos, chalkOriginPos;
    Quaternion scrubOriginRot, chalkOriginRot;
    public Vector3 boardUpVector;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GC.Instance;
    }

    // functions to be called by WordSearchController,
    // subsiquent functions will their respective co-routines and clean up after themselves
    public void WriteWord(string str, float totalTime) { StartCoroutine(WriteWordIE(str, totalTime)); }
    public void ScrubWord(float totalTime) { StartCoroutine(ScrubWordIE(totalTime)); }
    public void UseScrubber(float initialLerpTime) { StartCoroutine(UseScrubberIE(initialLerpTime)); }
    public void UseChalk(float initialLerpTime) { StartCoroutine(UseChalkIE(initialLerpTime)); }
    public string GetWord() { return thisText.text; }
    public void SetWord(string str) { thisText.text = str; }

    // populate a line on the board with a word, 1 letter at a time
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

    // moves the chalk object position to the board and where the word needs to be written
    IEnumerator UseChalkIE(float initialLerpTime)
    {
        chalkOriginPos = chalk.transform.position;
        chalkOriginRot = chalk.transform.rotation;
        chalk.GetComponent<Scrubber>().shaking = true;
        chalk.GetComponent<Scrubber>().ShakeScrubber(2, 0.1f, boardUpVector);
        gameController.SM.PlaySFX(SFX.Chalk_Write);
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

    // removes the word on the board, letter by letter
    IEnumerator ScrubWordIE(float totalTime)
    {

        int len = thisText.text.Length;
        Debug.Log(len);
        for (int i = len; i > -1; i--)
        {
            thisText.text = thisText.text.Substring(0, i);
            yield return new WaitForSeconds(totalTime / len);
        }
        yield break;
    }

    // moves the scrubber object position to the board and where the word needs to be rubbed out
    IEnumerator UseScrubberIE(float initialLerpTime)
    {
        scrubOriginPos = scrubber.transform.position;
        scrubOriginRot = scrubber.transform.rotation;
        scrubber.GetComponent<Scrubber>().shaking = true;
        scrubber.GetComponent<Scrubber>().ShakeScrubber(2, 0.1f, boardUpVector);
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

    // important for the WordSearchController to call this at the right time,
    // there was a bug in the build involving which start functions are called before the other,
    // this mitigates that problem
    public void Init()
    {
        thisText = gameObject.GetComponent<Text>();
    }
}
