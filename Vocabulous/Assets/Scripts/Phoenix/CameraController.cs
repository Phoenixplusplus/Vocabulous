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
    float mouseX;
    public float mouseSensitivty = 4f;
    public float lerpSpeed = 1f;
    public bool inPlay = false;
    public bool playWordDice, playWordSearch, playAnagram, playWordDrop, playSolver,
                onWordDice, onWordSearch, onAnagram, onWordDrop, onSolver,
                quitting;
    public Transform notInPlayTransform, wordDiceCameraTransform, wordSearchTransform, anagramTransform, wordDropTransform, solverTransform;

    void Start()
    {
        gameController = GC.Instance;

        transform.LookAt(cameraParent);
        targetAngle = new Vector3(0f, 20f, 0f);
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

        if (!inPlay) if (Input.GetMouseButton(0)) { targetAngle.y += (Input.GetAxis("Mouse X") * mouseSensitivty); }

        if (currentAngle.y > (gameController.RotTranWordDice.y + 360) - 20f && currentAngle.y < (gameController.RotTranWordDice.y + 360) + 20f && !inPlay) onWordDice = true;
        else onWordDice = false;
        if (currentAngle.y > (gameController.RotWordSearch.y + 360) - 20f && currentAngle.y < (gameController.RotWordSearch.y + 360) + 20f && !inPlay) onWordSearch = true;
        else onWordSearch = false;
        if (currentAngle.y > (gameController.RotTranAnagram.y + 360) - 20f && currentAngle.y < (gameController.RotTranAnagram.y + 360) + 20f && !inPlay) onAnagram = true;
        else onAnagram = false;
        if (currentAngle.y > (gameController.RotTranWordrop.y + 360) - 20f && currentAngle.y < (gameController.RotTranWordrop.y + 360) + 20f && !inPlay) onWordDrop = true;
        else onWordDrop = false;
        if (currentAngle.y > (gameController.RotTranGame5.y + 360) - 20f && currentAngle.y < (gameController.RotTranGame5.y + 360) + 20f && !inPlay) onSolver = true;
        else onSolver = false;

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
        if (playSolver)
        {
            transform.position = Vector3.Lerp(transform.position, solverTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, solverTransform.rotation, Time.deltaTime);
        }
        if (quitting)
        {
            transform.position = Vector3.Lerp(transform.position, notInPlayTransform.position, Time.deltaTime * 3f);
            transform.rotation = Quaternion.Lerp(transform.rotation, notInPlayTransform.rotation, Time.deltaTime * 3f);
            if (Vector3.Distance(transform.position, notInPlayTransform.position) < 0.05f)
            {
                quitting = false;
                inPlay = false;
                gameController.SetGameState(1);
            }
        }
    }

    // button click functions
    public void RotateToGameWordDice() { targetAngle = new Vector3(0, gameController.RotTranWordDice.y, 0); }
    public void RotateToGameWordSearch() { targetAngle = new Vector3(0, gameController.RotWordSearch.y, 0); }
    public void RotateToGameAnagram() { targetAngle = new Vector3(0, gameController.RotTranAnagram.y, 0); }
    public void RotateToGameWordDrop() { targetAngle = new Vector3(0, gameController.RotTranWordrop.y, 0); }
    public void RotateToGame5() { targetAngle = new Vector3(0, gameController.RotTranGame5.y, 0); }

    public void PlayClicked()
    {
        inPlay = true;
        if (onWordDice) playWordDice = true;
        if (onWordSearch) playWordSearch = true;
        if (onAnagram) playAnagram = true;
        if (onWordDrop) playWordDrop = true;
        if (onSolver) playSolver = true;
    }

    public void QuitClicked()
    {
        quitting = true;

        playWordDice = false;
        playWordSearch = false;
        playAnagram = false;
        playWordDrop = false;
        playSolver = false;
        onWordDice = false;
        onWordSearch = false;
        onAnagram = false;
        onWordDrop = false;
        onSolver = false;
    }
}
