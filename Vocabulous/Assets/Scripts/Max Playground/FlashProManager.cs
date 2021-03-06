﻿//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Manager for creating Flashes (and static GUI items)
// Attached to the GC

public class FlashProManager : MonoBehaviour
{
    #region Member Declaration
    public GameObject prefabFlash;
    public GameObject freeGUI;
    private List<GameObject> myGUIitems;
    #endregion

    #region Unity API
    void Start()
    {
        // stash reference for efficiency
        myGUIitems = new List<GameObject>();
    }
    #endregion

    #region Public Add/Delete Statis GUI methods
    public GameObject AddGUIItem (string text, float x, float y, float width, Color color)
    {
        GameObject GUI = Instantiate(freeGUI, Vector3.zero, Quaternion.identity);
        GUI.transform.parent = transform.parent.transform;
        GUI.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(x*Screen.width, y* Screen.height, 0), Quaternion.identity);
        GUI.GetComponent<RectTransform>().sizeDelta = new Vector2(width * Screen.width, 0);
        GUI.GetComponent<TextMeshProUGUI>().text = text;
        GUI.GetComponent<TextMeshProUGUI>().color = color;
        myGUIitems.Add(GUI);
        return GUI;
    }

    public void KillStaticGUIs ()
    {
        foreach (GameObject gui in myGUIitems)
        {
            Destroy(gui.gameObject);
        }
        myGUIitems.Clear();
    }
    #endregion

    #region Public Add/Delete Flashes (multiple Overloads)
    // Use this for tidy up during scene changes
    public void KillAllFlashes ()
    {
        StopAllCoroutines(); // kills the ones requested on delay
        GameObject[] flashes = GameObject.FindGameObjectsWithTag("GUIFlash");
        foreach (GameObject flash in flashes)
        {
            Destroy(flash);
        }

    }

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
    #endregion
}
