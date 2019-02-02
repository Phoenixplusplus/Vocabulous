using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrubber : MonoBehaviour
{
    public float shakePower = 0.5f;

    public void ShakeScrubber(float initialTime) { StartCoroutine(ShakeScrubberIE(1f)); }

    IEnumerator ShakeScrubberIE(float initialTime)
    {
        float t = 0;
        while (t < initialTime)
        {
            transform.localPosition = transform.localPosition + Random.insideUnitSphere * shakePower;
            t += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
