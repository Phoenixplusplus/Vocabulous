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

public class Scrubber : MonoBehaviour
{
    public float shakePower = 0.5f;
    float t;
    bool movingUp;
    public bool shaking;
    Vector3 startPos;

    public void ShakeScrubber(float initialTime, float threshold, Vector3 localUp) { StartCoroutine(ShakeScrubberIE(initialTime, threshold , localUp)); }

    IEnumerator ShakeScrubberIE(float initialTime, float threshold, Vector3 localUp)
    {
        if (shaking)
        {
            float t = 0;
            float ti = 0;
            while (t < initialTime)
            {
                t += Time.deltaTime;
                ti += Time.deltaTime;

                if (ti > threshold)
                {
                    ti = 0;
                    movingUp = !movingUp;
                }

                if (movingUp == true) { transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + localUp, t / initialTime); }
                else { transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition - localUp, t / initialTime); }

                if (!shaking) yield break;

                yield return null;
            }
        }
        yield break;
    }
}
