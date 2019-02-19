using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIC : MonoBehaviour
{
    GC gameController;
    public CameraController cameraController;
    public Animator OptionsAnimation;
    public Button PlayThis, QuitThis;

    void Start()
    {
        gameController = GC.Instance;

        TogglePlayButton(false);
        ToggleQuitButton(false);

        // When editting, it's a pain not to have the GUI displayed ... so will "unfold" the GUI animation on start
        //OptionsAnimation.SetBool("OptionsClicked", true);
        //ToggleOptionsInOut();
    }

    void Update()
    {
        if (!cameraController.inPlay)
        {
            if (cameraController.onWordDice || cameraController.onWordSearch || cameraController.onAnagram || cameraController.onFreeWord || cameraController.onSolver) TogglePlayButton(true);
            else TogglePlayButton(false);
        }
    }

    // UI animations
    public void ToggleOptionsInOut()
    {
        bool b = OptionsAnimation.GetBool("OptionsClicked");
        OptionsAnimation.SetBool("OptionsClicked", !b);
    }

    // UI Button functions
    public void TogglePlayButton(bool state) { if (PlayThis.gameObject.activeInHierarchy == !state) PlayThis.gameObject.SetActive(state); }
    public void ToggleQuitButton(bool state) { if (QuitThis.gameObject.activeInHierarchy == !state) QuitThis.gameObject.SetActive(state); }
    public void QuitClicked() { ToggleQuitButton(false); }
    public void PlayClicked() { TogglePlayButton(false); ToggleQuitButton(true); }
}
