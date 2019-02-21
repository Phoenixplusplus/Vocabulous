using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashProManager : MonoBehaviour
{
    public GameObject prefabFlash;

    public void CustomFlash(FlashProTemplate myTemplate)
    {
        GameObject f = Instantiate(prefabFlash, Vector3.zero, Quaternion.identity);
        f.GetComponent<FlashPro>().ConfigureAndGoGo(myTemplate);
        f.transform.parent = transform.parent.transform;
    }

    public void CustomFlash(FlashProTemplate myTemplate, float delay)
    {
        StartCoroutine(DelayFire(myTemplate, delay));
    }

    IEnumerator DelayFire(FlashProTemplate myTemplate, float delay)
    {
        yield return new WaitForSeconds(delay);
        CustomFlash(myTemplate);
    }

    public void CustomFlash(FlashProTemplate myTemplate, string message)
    {
        FlashProTemplate FT = myTemplate.Copy();
        FT.myMessage1 = message;
        GameObject f = Instantiate(prefabFlash, Vector3.zero, Quaternion.identity);
        f.GetComponent<FlashPro>().ConfigureAndGoGo(FT);
        f.transform.parent = transform.parent.transform;
    }

    public void CustomFlash(FlashProTemplate myTemplate, string message, float delay)
    {
        FlashProTemplate FT = myTemplate.Copy();
        FT.myMessage1 = message;
        StartCoroutine(DelayFire(FT, delay));
    }

    public void CustomFlash(FlashProTemplate myTemplate, string message1, string message2)
    {
        FlashProTemplate FT = myTemplate.Copy();
        FT.myMessage1 = message1;
        FT.myMessage2 = message2;
        GameObject f = Instantiate(prefabFlash, Vector3.zero, Quaternion.identity);
        f.GetComponent<FlashPro>().ConfigureAndGoGo(FT);
        f.transform.parent = transform.parent.transform;
    }

    public void CustomFlash(FlashProTemplate myTemplate, string message1, string message2, float delay)
    {
        FlashProTemplate FT = myTemplate.Copy();
        FT.myMessage1 = message1;
        FT.myMessage2 = message2;
        StartCoroutine(DelayFire(FT, delay));
    }
}
