using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
                onWordDice, onWordSearch, onAnagram, onFreeWord, onSolver,
                clickedWordDice, clickedWordSearch, clickedAnagram, clickedFreeWord, clickedSolver,
                quitting;
    public Transform notInPlayTransform, wordDiceCameraTransform, wordSearchTransform, anagramTransform, freeWordTransform, solverTransform;
    Vector3 wordDiceCameraTransformOrigin, wordSearchCameraTransformOrigin, anagramCameraTransformOrigin, freeWordCameraTransformOrigin, solverCameraTransformOrigin,
            wordDiceUnrotatedForward, wordSearchUnrotatedForward, anagramUnrotatedForward, freeWordUnrotatedForward, solverUnrotatedForward;

    void Start()
    {
        gameController = GC.Instance;

        // set origin positions for panning to go back to
        wordDiceCameraTransformOrigin = wordDiceCameraTransform.position;
        wordSearchCameraTransformOrigin = wordSearchTransform.position;
        anagramCameraTransformOrigin = anagramTransform.position;
        freeWordCameraTransformOrigin = freeWordTransform.position;
        solverCameraTransformOrigin = solverTransform.position;

        // set true forward direction for panning
        // unrotate angle of original transform, grab its forward and assign, then rotate back (long winded but half decent way)
        Vector3 wdt = wordDiceCameraTransform.transform.eulerAngles;
        wordDiceCameraTransform.transform.eulerAngles = new Vector3(0, wordDiceCameraTransform.eulerAngles.y, wordDiceCameraTransform.eulerAngles.z);
        wordDiceUnrotatedForward = wordDiceCameraTransform.forward;
        wordDiceCameraTransform.transform.eulerAngles = wdt;

        Vector3 wst = wordSearchTransform.transform.eulerAngles;
        wordSearchTransform.transform.eulerAngles = new Vector3(0, wordSearchTransform.eulerAngles.y, wordSearchTransform.eulerAngles.z);
        wordSearchUnrotatedForward = wordSearchTransform.forward;
        wordSearchTransform.transform.eulerAngles = wst;

        Vector3 at = anagramTransform.transform.eulerAngles;
        anagramTransform.transform.eulerAngles = new Vector3(0, anagramTransform.eulerAngles.y, anagramTransform.eulerAngles.z);
        anagramUnrotatedForward = anagramTransform.forward;
        anagramTransform.transform.eulerAngles = at;

        Vector3 wddt = freeWordTransform.transform.eulerAngles;
        freeWordTransform.transform.eulerAngles = new Vector3(0, freeWordTransform.eulerAngles.y, freeWordTransform.eulerAngles.z);
        freeWordUnrotatedForward = freeWordTransform.forward;
        freeWordTransform.transform.eulerAngles = wddt;

        Vector3 st = solverTransform.transform.eulerAngles;
        solverTransform.transform.eulerAngles = new Vector3(0, solverTransform.eulerAngles.y, solverTransform.eulerAngles.z);
        solverUnrotatedForward = solverTransform.forward;
        solverTransform.transform.eulerAngles = st;

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

        // out game rotation around table
        if (!inPlay) if (Input.GetMouseButton(0)) if (!EventSystem.current.IsPointerOverGameObject()) { targetAngle.y += (Input.GetAxis("Mouse X") * mouseSensitivty); }

        // panning within game
        if (inPlay)
        {
            if (Input.GetMouseButton(1))
            {
                if (playWordDice)
                {
                    wordDiceCameraTransform.position += wordDiceCameraTransform.right * (Input.GetAxis("Mouse X") * mouseSensitivty);
                    wordDiceCameraTransform.position += wordDiceUnrotatedForward * (Input.GetAxis("Mouse Y") * mouseSensitivty);
                }
                if (playWordSearch)
                {
                    wordSearchTransform.position += wordSearchTransform.right * (Input.GetAxis("Mouse X") * mouseSensitivty);
                    wordSearchTransform.position += wordSearchUnrotatedForward * (Input.GetAxis("Mouse Y") * mouseSensitivty);
                }
                if (playAnagram)
                {
                    anagramTransform.position += anagramTransform.right * (Input.GetAxis("Mouse X") * mouseSensitivty);
                    anagramTransform.position += anagramUnrotatedForward * (Input.GetAxis("Mouse Y") * mouseSensitivty);
                }
                if (playWordDrop)
                {
                    freeWordTransform.position += freeWordTransform.right * (Input.GetAxis("Mouse X") * mouseSensitivty);
                    freeWordTransform.position += freeWordUnrotatedForward * (Input.GetAxis("Mouse Y") * mouseSensitivty);
                }
                if (playSolver)
                {
                    solverTransform.position += solverTransform.right * (Input.GetAxis("Mouse X") * mouseSensitivty);
                    solverTransform.position += solverUnrotatedForward * (Input.GetAxis("Mouse Y") * mouseSensitivty);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (playWordDice) { wordDiceCameraTransform.position = wordDiceCameraTransformOrigin; }
                if (playWordSearch) { wordSearchTransform.position = wordSearchCameraTransformOrigin; }
                if (playAnagram) { anagramTransform.position = anagramCameraTransformOrigin; }
                if (playWordDrop) { freeWordTransform.position = freeWordCameraTransformOrigin; }
                if (playSolver) { solverTransform.position = solverCameraTransformOrigin; }
            }
        }

        // for UIC to display play game button
        // for angles that are positive, do not + 360
        if (currentAngle.y > 0 && currentAngle.y <= 90f && !inPlay) onWordDice = true;
        else onWordDice = false;
        if (currentAngle.y < 360f && currentAngle.y >= 270f) onWordSearch = true;
        else onWordSearch = false;
        if (currentAngle.y > 90f && currentAngle.y <= 180f && !inPlay) onAnagram = true;
        else onAnagram = false;
        if (currentAngle.y > 180f && currentAngle.y <= 270f && !inPlay) onFreeWord = true;
        else onFreeWord = false;
        //if (currentAngle.y > 288 - 36f && currentAngle.y <= 360 - 36f && !inPlay) onSolver = true;
        //else onSolver = false;

        // transitioning into game area
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
            transform.position = Vector3.Lerp(transform.position, freeWordTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, freeWordTransform.rotation, Time.deltaTime);
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

        // clicked on 3d asset of a game, rotate to it, then transition in
        if (clickedWordDice)
        {
            if (targetAngle == new Vector3(0, gameController.RotTranWordDice.y, 0))
            {
                if (onWordDice)
                {
                    gameController.SetGameState(2);
                    clickedWordDice = false;
                }
            }
        }
        if (clickedWordSearch)
        {
            if (targetAngle == new Vector3(0, gameController.RotWordSearch.y, 0))
            {
                if (onWordSearch)
                {
                    gameController.SetGameState(2);
                    clickedWordSearch = false;
                }
            }
        }
        if (clickedAnagram)
        {
            if (targetAngle == new Vector3(0, gameController.RotTranAnagram.y, 0))
            {
                if (onAnagram)
                {
                    gameController.SetGameState(2);
                    clickedAnagram = false;
                }
            }
        }
        if (clickedFreeWord)
        {
            if (targetAngle == new Vector3(0, gameController.RotTranFreeWord.y, 0))
            {
                if (onFreeWord)
                {
                    gameController.SetGameState(2);
                    clickedFreeWord = false;
                }
            }
        }
        if (clickedSolver)
        {
            if (targetAngle == new Vector3(0, gameController.RotTranSolver.y, 0))
            {
                if (onSolver)
                {
                    gameController.SetGameState(2);
                    clickedSolver = false;
                }
            }
        }
    }

    // asset click functions / button click functions
    public void RotateToGameWordDice()
    {
        clickedWordDice = true;
        targetAngle = new Vector3(0, gameController.RotTranWordDice.y, 0);

    }
    public void RotateToGameWordSearch()
    {
        clickedWordSearch = true;
        targetAngle = new Vector3(0, gameController.RotWordSearch.y, 0);
    }
    public void RotateToGameAnagram()
    {
        clickedAnagram = true;
        targetAngle = new Vector3(0, gameController.RotTranAnagram.y, 0);
    }
    public void RotateToGameFreeWord()
    {
        clickedFreeWord = true;
        targetAngle = new Vector3(0, gameController.RotTranFreeWord.y, 0);
    }
    public void RotateToSolver()
    {
        clickedSolver = true;
        targetAngle = new Vector3(0, gameController.RotTranSolver.y, 0);
    }

    public void PlayClicked()
    {
        inPlay = true;
        if (onWordDice) playWordDice = true;
        if (onWordSearch) playWordSearch = true;
        if (onAnagram) playAnagram = true;
        if (onFreeWord) playWordDrop = true;
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
        onFreeWord = false;
        onSolver = false;
    }
}
