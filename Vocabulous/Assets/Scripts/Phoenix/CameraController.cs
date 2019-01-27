using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private GC gameController;
    public Transform cameraParent;
    public Vector3 currentAngle;
    public Vector3 targetAngle;
    public Button PlayThis, QuitThis;
    public Animator OptionsAnimation;
    float mouseX;
    public float mouseSensitivty = 4f;
    public float lerpSpeed = 1f;
    public bool inPlay = false;
    public bool playWordDice, playWordSearch, playAnagram, playWordDrop, playGame5,
                onWordDice, onWordSearch, onAnagram, onWordDrop, onGame5,
                quitting;
    public Transform notInPlayTransform, wordDiceCameraTransform, wordSearchTransform, anagramTransform, wordDropTransform, game5Transform;

    void Start()
    {
        gameController = GC.Instance;

        transform.LookAt(cameraParent);
        targetAngle = new Vector3(0f, 20f, 0f);

        PlayThis.gameObject.SetActive(false);
    }

    void Update()
    {
        // angle consistency
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * lerpSpeed),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * lerpSpeed),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * lerpSpeed));

        cameraParent.transform.eulerAngles = currentAngle;

        if (currentAngle.y < 0) currentAngle.y = 360f;
        if (currentAngle.y > 360f) currentAngle.y = 0f;

        // GameState == 1, looking at table choosing a game
        if (gameController.GameState == 1)
        {
            if (Input.GetMouseButton(0)) { targetAngle.y += (Input.GetAxis("Mouse X") * mouseSensitivty); }

            ToggleQuitButton(false);
            if (onWordDice || onWordSearch || onAnagram || onWordDrop || onGame5) TogglePlayButton(true);
            else TogglePlayButton(false);
        }
        else
        {
            TogglePlayButton(false);
            ToggleQuitButton(true);
        }

        if (currentAngle.y > (gameController.RotTranWordDice.y + 360) - 20f && currentAngle.y < (gameController.RotTranWordDice.y + 360) + 20f && gameController.GameState == 1) onWordDice = true;
        else onWordDice = false;
        if (currentAngle.y > (gameController.RotWordSearch.y + 360) - 20f && currentAngle.y < (gameController.RotWordSearch.y + 360) + 20f && gameController.GameState == 1) onWordSearch = true;
        else onWordSearch = false;
        if (currentAngle.y > (gameController.RotTranAnagram.y + 360) - 20f && currentAngle.y < (gameController.RotTranAnagram.y + 360) + 20f && gameController.GameState == 1) onAnagram = true;
        else onAnagram = false;
        if (currentAngle.y > (gameController.RotTranWordrop.y + 360) - 20f && currentAngle.y < (gameController.RotTranWordrop.y + 360) + 20f && gameController.GameState == 1) onWordDrop = true;
        else onWordDrop = false;
        if (currentAngle.y > (gameController.RotTranGame5.y + 360) - 20f && currentAngle.y < (gameController.RotTranGame5.y + 360) + 20f && gameController.GameState == 1) onGame5 = true;
        else onGame5 = false;
        //

        if (playWordDice)
        {
            transform.position = Vector3.Lerp(transform.position, wordDiceCameraTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, wordDiceCameraTransform.rotation, Time.deltaTime);
        }
        if (playWordSearch)
        {
            transform.position = Vector3.Lerp(transform.position, wordSearchTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, wordSearchTransform.rotation, Time.deltaTime);
        }
        if (playAnagram)
        {
            transform.position = Vector3.Lerp(transform.position, anagramTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, anagramTransform.rotation, Time.deltaTime);
        }
        if (playWordDrop)
        {
            transform.position = Vector3.Lerp(transform.position, wordDropTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, wordDropTransform.rotation, Time.deltaTime);
        }
        if (playGame5)
        {
            transform.position = Vector3.Lerp(transform.position, game5Transform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, game5Transform.rotation, Time.deltaTime);
        }
        if (quitting)
        {
            transform.position = Vector3.Lerp(transform.position, notInPlayTransform.position, Time.deltaTime * 3f);
            transform.rotation = Quaternion.Lerp(transform.rotation, notInPlayTransform.rotation, Time.deltaTime * 3f);
            if (Vector3.Distance(transform.position, notInPlayTransform.position) < 0.05f)
            {
                quitting = false;
                inPlay = false;
                gameController.GameState = 1;
            }
        }
    }

    public void TogglePlayButton(bool state) { if (PlayThis.gameObject.activeInHierarchy == !state) PlayThis.gameObject.SetActive(state); }
    public void ToggleQuitButton(bool state) { if (QuitThis.gameObject.activeInHierarchy == !state) QuitThis.gameObject.SetActive(state); }

    // UI animations
    public void ToggleOptionsInOut()
    {
        bool b = OptionsAnimation.GetBool("OptionsClicked");
        OptionsAnimation.SetBool("OptionsClicked", !b);
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
        if (onWordDice) { playWordDice = true; TogglePlayButton(false); }
        if (onWordSearch) playWordSearch = true;
        if (onAnagram) playAnagram = true;
        if (onWordDrop) playWordDrop = true;
        if (onGame5) playGame5 = true;
    }

    public void QuitClicked()
    {
        quitting = true;

        playWordDice = false;
        playWordSearch = false;
        playAnagram = false;
        playWordDrop = false;
        playGame5 = false;
        onWordDice = false;
        onWordSearch = false;
        onAnagram = false;
        onWordDrop = false;
        onGame5 = false;
        ToggleQuitButton(false);
    }
}
