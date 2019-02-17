using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Flashes { AlreadyGot, NewWord};


public class FlashPanelManager : MonoBehaviour
{

    public GameObject[] myFlashes = new GameObject[2];
    public GameObject defaultFlash;

    public void Flash (Flashes type)
    {
        GameObject f = Instantiate(myFlashes[(int)type], Vector3.zero, Quaternion.identity);
        f.transform.parent = transform.parent.transform;
    }

    public void CustomFlash (FlashTemplate myTemplate)
    {
        GameObject f = Instantiate(defaultFlash, Vector3.zero, Quaternion.identity);
        f.GetComponent<Flash>().ConfigureAndGoGo(myTemplate);
        f.transform.parent = transform.parent.transform;
    }

    public void CustomFlash(FlashTemplate myTemplate, string message)
    {
        GameObject f = Instantiate(defaultFlash, Vector3.zero, Quaternion.identity);
        f.GetComponent<Flash>().ConfigureAndGoGo(myTemplate,message);
        f.transform.parent = transform.parent.transform;
    }

}
