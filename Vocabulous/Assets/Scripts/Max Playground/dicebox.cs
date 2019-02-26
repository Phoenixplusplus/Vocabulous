using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dicebox : MonoBehaviour
{
    public GameObject slot0;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;
    public GameObject slot5;
    public GameObject slot6;
    public GameObject slot7;
    public GameObject slot8;
    public GameObject slot9;
    public GameObject slot10;
    public GameObject slot11;
    public GameObject slot12;
    public GameObject slot13;
    public GameObject slot14;
    public GameObject slot15;

    public Vector3[] slots;
    private Vector3 oldPos;

    // Start is called before the first frame update
    void Awake()
    {
        oldPos = transform.localPosition;
        PopulateSlots();
    }

    private void PopulateSlots()
    {
        slots = new Vector3[16];
        slots[0] = slot0.transform.localPosition + transform.localPosition;
        slots[1] = slot1.transform.localPosition + transform.localPosition;
        slots[2] = slot2.transform.localPosition + transform.localPosition;
        slots[3] = slot3.transform.localPosition + transform.localPosition;
        slots[4] = slot4.transform.localPosition + transform.localPosition;
        slots[5] = slot5.transform.localPosition + transform.localPosition;
        slots[6] = slot6.transform.localPosition + transform.localPosition;
        slots[7] = slot7.transform.localPosition + transform.localPosition;
        slots[8] = slot8.transform.localPosition + transform.localPosition;
        slots[9] = slot9.transform.localPosition + transform.localPosition;
        slots[10] = slot10.transform.localPosition + transform.localPosition;
        slots[11] = slot11.transform.localPosition + transform.localPosition;
        slots[12] = slot12.transform.localPosition + transform.localPosition;
        slots[13] = slot13.transform.localPosition + transform.localPosition;
        slots[14] = slot14.transform.localPosition + transform.localPosition;
        slots[15] = slot15.transform.localPosition + transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition != oldPos)
        {
            PopulateSlots();
            oldPos = transform.localPosition;
        }
    }
}
