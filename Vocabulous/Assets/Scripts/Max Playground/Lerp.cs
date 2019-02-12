using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
