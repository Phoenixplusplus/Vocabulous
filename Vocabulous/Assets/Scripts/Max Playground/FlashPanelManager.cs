using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashPanelManager : MonoBehaviour
{
    public GameObject defaultFlash;

    public void CustomFlash (FlashTemplate myTemplate)
    {
        GameObject f = Instantiate(defaultFlash, Vector3.zero, Quaternion.identity);
        f.GetComponent<Flash>().ConfigureAndGoGo(myTemplate);
        f.transform.parent = transform.parent.transform;
    }

    public void CustomFlash(FlashTemplate myTemplate,float delay)
    {
        StartCoroutine(DelayFire(myTemplate, delay));
    }

    IEnumerator DelayFire (FlashTemplate myTemplate, float delay)
    {
        yield return new WaitForSeconds(delay);
        CustomFlash(myTemplate);
    }

    public void CustomFlash(FlashTemplate myTemplate, string message)
    {
        FlashTemplate FT = myTemplate.Copy();
        FT.myMessage1 = message;
        GameObject f = Instantiate(defaultFlash, Vector3.zero, Quaternion.identity);
        f.GetComponent<Flash>().ConfigureAndGoGo(FT);
        f.transform.parent = transform.parent.transform;
    }

    public void CustomFlash(FlashTemplate myTemplate, string message, float delay)
    {
        FlashTemplate FT = myTemplate.Copy();
        FT.myMessage1 = message;
        StartCoroutine(DelayFire(FT, delay));
    }

    public void CustomFlash(FlashTemplate myTemplate, string message1, string message2)
    {
        FlashTemplate FT = myTemplate.Copy();
        FT.myMessage1 = message1;
        FT.myMessage2 = message2;
        GameObject f = Instantiate(defaultFlash, Vector3.zero, Quaternion.identity);
        f.GetComponent<Flash>().ConfigureAndGoGo(FT);
        f.transform.parent = transform.parent.transform;
    }

    public void CustomFlash(FlashTemplate myTemplate, string message1, string message2, float delay)
    {
        FlashTemplate FT = myTemplate.Copy();
        FT.myMessage1 = message1;
        FT.myMessage2 = message2;
        StartCoroutine(DelayFire(FT, delay));
    }

}
