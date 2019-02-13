using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLerper : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 FinishPosition;
    public float time = 0.5f;
    private float Timer;
    public float scaleStart;
    public float scaleFinish;
    private float scaler;
    private bool animating;
    public Vector3 Move;

    public void LerpForward()
    {
        if (animating)
        {
            Debug.Log("Tile Already animating");
            return;
        }
        animating = true;
        SetToStart();
        Move = (FinishPosition - StartPosition) / time;
        Timer = time;
        StartCoroutine("myLerp", FinishPosition);
    }

    public void LerpBackwards()
    {
        if (animating)
        {
            Debug.Log("Tile Already animating");
            return;
        }
        animating = true;
        SetToFinish();
        Move = (StartPosition - FinishPosition) / time;
        Timer = time;
        StartCoroutine("myLerp", StartPosition);
    }

    IEnumerator myLerp (Vector3 to)
    {
        while (Timer >= 0)
        {
            transform.localPosition = transform.localPosition + (Move * Time.deltaTime);
            Timer -= Time.deltaTime;
            yield return null;
        }
        Timer = 0;
        SetToVector(to);
        animating = false;
    }

    public void SetToStart ()
    {
        StopAllCoroutines();
        animating = false;
        transform.localPosition = StartPosition;
    }

    public void SetToFinish ()
    {
        StopAllCoroutines();
        animating = false;
        transform.localPosition = FinishPosition;
    }

    public void SetToVector (Vector3 newPosition)
    {
        transform.localPosition = newPosition;
    }

}
