﻿//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using UnityEngine;


// Utility function
// One shot "move me" script
// give it to a game Object ... will move it from Start to Finish (over prescribed time) then destroy itself
public class Lerp : MonoBehaviour
{
    private bool local = true;
    private Vector3 Opos;
    private Vector3 NPos;
    public float time;
    private Vector3 move;

    public void Configure(Vector3 Start, Vector3 Finish, float Time, bool Local)
    {
        Opos = Start;
        NPos = Finish;
        time = Time;
        local = Local;
        move = (Finish - Start) / Time;
    }

    public void Go()
    {
        if (local) transform.localPosition = Opos;
        else { transform.position = Opos; }
        StartCoroutine("MoveMe");
    }

    IEnumerator MoveMe ()
    {
        while (time >= 0)
        {
            if (local) transform.localPosition = transform.localPosition + (move * Time.deltaTime);
            else { transform.position = transform.position + (move * Time.deltaTime); }
            time -= Time.deltaTime;
            yield return null;
        }
        if (local) transform.localPosition = NPos;
        else { transform.position = NPos; }
        Destroy(this);

    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

}
