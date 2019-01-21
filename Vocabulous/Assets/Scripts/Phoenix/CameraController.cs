using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform cameraParent;
    public Vector3 currentAngle;
    public Vector3 targetAngle;
    public Button Bwoggle, WordSplerch, Game3, Game4, Stats, PlayThis, QuitThis;
    public Animator ButtonAnimations;
    float mouseX;
    public float mouseSensitivty = 4f;
    public float lerpSpeed = 1f;
    public bool inPlay = false;
    public bool playBwoggle, playWordSplerch, playGame3, playGame4, playStats,
                onBwoggle, onWordSplerch, onGame3, onGame4, onStats,
                quitting;
    public Transform notInPlayTransform, bwoggleCameraTransform, wordSplerchTransform, game3Transform, game4Transform, statsTransform;

    void Start()
    {
        transform.LookAt(cameraParent);
        targetAngle = new Vector3(0f, 20f, 0f);

        PlayThis.gameObject.SetActive(false);
        ToggleButtonsSlide();
    }

    void Update()
    {
        if (!inPlay)
        {
            if (Input.GetMouseButton(0)) { targetAngle.y += (Input.GetAxis("Mouse X") * mouseSensitivty); }
        }

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * lerpSpeed),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * lerpSpeed),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * lerpSpeed));

        cameraParent.transform.eulerAngles = currentAngle;

        if (currentAngle.y < 0) currentAngle.y = 360f;
        if (currentAngle.y > 360f) currentAngle.y = 0f;

        if (currentAngle.y > 278 && currentAngle.y < 318f && !inPlay) onBwoggle = true;
        else onBwoggle = false;
        if (currentAngle.y > 220f && currentAngle.y < 260 && !inPlay) onWordSplerch = true;
        else onWordSplerch = false;
        if (currentAngle.y > 165f && currentAngle.y < 205f && !inPlay) onGame3 = true;
        else onGame3 = false;
        if (currentAngle.y > 110f && currentAngle.y < 150f && !inPlay) onGame4 = true;
        else onGame4 = false;
        if (currentAngle.y > 57f && currentAngle.y < 97f && !inPlay) onStats = true;
        else onStats = false;

        if (!inPlay)
        {
            ToggleQuitButton(false);
            if (onBwoggle || onWordSplerch || onGame3 || onGame4 || onStats) TogglePlayButton(true);
            else TogglePlayButton(false);
        }
        else
        {
            TogglePlayButton(false);
            ToggleQuitButton(true);
        }

        if (playBwoggle)
        {
            transform.position = Vector3.Lerp(transform.position, bwoggleCameraTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, bwoggleCameraTransform.rotation, Time.deltaTime);
        }
        if (playWordSplerch)
        {
            transform.position = Vector3.Lerp(transform.position, wordSplerchTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, wordSplerchTransform.rotation, Time.deltaTime);
        }
        if (playGame3)
        {
            transform.position = Vector3.Lerp(transform.position, game3Transform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, game3Transform.rotation, Time.deltaTime);
        }
        if (playGame4)
        {
            transform.position = Vector3.Lerp(transform.position, game4Transform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, game4Transform.rotation, Time.deltaTime);
        }
        if (playStats)
        {
            transform.position = Vector3.Lerp(transform.position, statsTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, statsTransform.rotation, Time.deltaTime);
        }
        if (quitting)
        {
            transform.position = Vector3.Lerp(transform.position, notInPlayTransform.position, Time.deltaTime * 3f);
            transform.rotation = Quaternion.Lerp(transform.rotation, notInPlayTransform.rotation, Time.deltaTime * 3f);
            if (Vector3.Distance(transform.position, notInPlayTransform.position) < 0.05f)
            {
                quitting = false;
                inPlay = false;
                ToggleButtonsSlide();
            }
        }
    }

    public void TogglePlayButton(bool state) { if (PlayThis.gameObject.activeInHierarchy == !state) PlayThis.gameObject.SetActive(state); }
    public void ToggleQuitButton(bool state) { if (QuitThis.gameObject.activeInHierarchy == !state) QuitThis.gameObject.SetActive(state); }

    // UI animations
    public void ToggleButtonsSlide()
    {
        bool b = ButtonAnimations.GetBool("SlideState");
        ButtonAnimations.SetBool("SlideState", !b);
    }

    // button click functions
    public void RotateToGameBwoggle() { targetAngle = new Vector3(0, 298, 0); }
    public void RotateToGameWordSplerch() { targetAngle = new Vector3(0, 240, 0); }
    public void RotateToGame3() { targetAngle = new Vector3(0, 185, 0); }
    public void RotateToGame4() { targetAngle = new Vector3(0, 130, 0); }
    public void RotateToStats() { targetAngle = new Vector3(0, 77, 0); }

    public void PlayClicked()
    {
        inPlay = true;
        if (onBwoggle) { playBwoggle = true; TogglePlayButton(false); }
        if (onWordSplerch) playWordSplerch = true;
        if (onGame3) playGame3 = true;
        if (onGame4) playGame4 = true;
        if (onStats) playStats = true;

        ToggleButtonsSlide();
    }

    public void QuitClicked()
    {
        //StartCoroutine(ReturnToStartTransform(2f));
        quitting = true;

        playBwoggle = false;
        playWordSplerch = false;
        playGame3 = false;
        playGame4 = false;
        playStats = false;
        onBwoggle = false;
        onWordSplerch = false;
        onGame3 = false;
        onGame4 = false;
        onStats = false;
        ToggleQuitButton(false);
    }

    IEnumerator ReturnToStartTransform(float lerpTime)
    {
        float t = 0;

        playBwoggle = false;
        playWordSplerch = false;
        playGame3 = false;
        playGame4 = false;
        playStats = false;
        onBwoggle = false;
        onWordSplerch = false;
        onGame3 = false;
        onGame4 = false;
        onStats = false;
        ToggleQuitButton(false);

        while (t < lerpTime)
        {
            transform.position = Vector3.Lerp(transform.position, notInPlayTransform.position, t / lerpTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, notInPlayTransform.rotation, t / lerpTime);
            t += Time.deltaTime;
            yield return null;
        }
        inPlay = false;

        ToggleButtonsSlide();
        yield break;
    }
}
