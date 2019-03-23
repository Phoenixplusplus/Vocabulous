using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIC : MonoBehaviour
{
    GC gameController;
    public GameObject newLibrary, japaneseLibrary;
    public CameraController cameraController;
    public Animator OptionsAnimation, WDOptionsAnimation, WSOptionsAnimation, AOptionsAnimation, FWOptionsAnimation, AboutOptionsAnimation;
    public Button PlayThis, QuitThis;
    public Text[] wordDiceStats = new Text[6];
    public Text[] wordSearchStats = new Text[3];
    public Text[] anagramStats = new Text[3];
    public Text[] freeWordStats = new Text[5];
    public InputField nameField;
    public Image playButtonImage, quitButtonImage;
    public Sprite wordDiceSprite, wordSearchSprite, anagramSprite, freeWordSprite, exitVocabSprite, quitSprite;
    public bool isMainMenuOpen, isWDOpen, isAOpen, isWSOpen, isFWOpen, isAUOpen;

    void Start()
    {
        gameController = GC.Instance;

        TogglePlayButton(false);
        QuitThis.gameObject.SetActive(true);

        // When editting, it's a pain not to have the GUI displayed ... so will "unfold" the GUI animation on start
        //OptionsAnimation.SetBool("OptionsClicked", true);
        //ToggleOptionsInOut();
    }

    void Update()
    {
        if (!cameraController.inPlay)
        {
            if (!isMainMenuOpen || !isWDOpen || !isAOpen || !isWSOpen || !isFWOpen || !isAUOpen)
            {
                if (cameraController.onWordDice || cameraController.onWordSearch || cameraController.onAnagram || cameraController.onFreeWord || cameraController.onSolver) TogglePlayButton(true);
                else TogglePlayButton(false);
            }
            if (isMainMenuOpen || isWDOpen || isAOpen || isWSOpen || isFWOpen || isAUOpen) TogglePlayButton(false);

            if (cameraController.onWordDice) playButtonImage.sprite = wordDiceSprite;
            if (cameraController.onWordSearch) playButtonImage.sprite = wordSearchSprite;
            if (cameraController.onAnagram) playButtonImage.sprite = anagramSprite;
            if (cameraController.onFreeWord) playButtonImage.sprite = freeWordSprite;
            quitButtonImage.sprite = exitVocabSprite;
        }
        else quitButtonImage.sprite = quitSprite;
    }

    // UI Button functions
    // in scene
    public void TogglePlayButton(bool state) { if (PlayThis.gameObject.activeInHierarchy == !state) PlayThis.gameObject.SetActive(state); }
    public void ToggleQuitButton(bool state) { if (QuitThis.gameObject.activeInHierarchy == !state) QuitThis.gameObject.SetActive(state); }
    public void ThemeButtonClicked()
    {
        if (newLibrary.activeInHierarchy)
        {
            newLibrary.SetActive(false);
            japaneseLibrary.SetActive(true);
            return;
        }
        if (japaneseLibrary.activeInHierarchy)
        {
            newLibrary.SetActive(true);
            japaneseLibrary.SetActive(false);
            return;
        }
    }
    public void QuitClicked()
    {
        if (gameController.GameState == 1) Application.Quit();
    }
    public void PlayClicked() { TogglePlayButton(false); ToggleQuitButton(true); }

    // in menu
    public void PopulateWordDiceStats()
    {
        wordDiceStats[0].text = "Times Played: " + gameController.player.WDPlays;
        wordDiceStats[1].text = "Highscore: " + gameController.player.WDHighscore;
        wordDiceStats[2].text = "Average Score: " + gameController.player.WDAverageScore.ToString("0.0");
        wordDiceStats[3].text = "Longest Word: " + gameController.player.WDLongest;
        wordDiceStats[4].text = "Most Words: " + gameController.player.WDMostWords;
        wordDiceStats[5].text = "Average Words: " + gameController.player.WDAverageWords.ToString("0.0");
    }

    public void PopulateWordSearchStats()
    {
        wordSearchStats[0].text = "Times Played: " + gameController.player.WordSearchTimesCompleted;
        wordSearchStats[1].text = "Best Time: " + ConvertTimeToString(gameController.player.WordSearchBestTime);
        wordSearchStats[2].text = "Average Time: " + ConvertTimeToString(gameController.player.WordSearchAverageTime);
    }

    public void PopulateAnagramsStats()
    {
        anagramStats[0].text = "Level: " + gameController.player.ALevel;
        anagramStats[1].text = "Extras: " + gameController.player.AExtras;
        anagramStats[2].text = "Hints Left: " + gameController.player.AHints;
    }

    public void PopulateFreeWordStats()
    {
        freeWordStats[0].text = "Times Played: " + gameController.player.FWTimesCompleted;
        freeWordStats[1].text = "Highscore: " + gameController.player.FWHighScore;
        freeWordStats[2].text = "Average Score: " + gameController.player.FWAverageScore.ToString("0.0");
        freeWordStats[3].text = "Longest Word: " + gameController.player.FWLongestWord + ", " + gameController.player.FWLongestWordCount;
        freeWordStats[4].text = "Average Words: " + gameController.player.FWAverageWord.ToString("0.0");
    }

    public string ConvertTimeToString(float time)
    {
        float minutes = Mathf.Floor(time / 60);
        int seconds = Mathf.RoundToInt(time % 60);
        return minutes + ":" + seconds.ToString("0");
    }

    public void UpdateName()
    {
        gameController.player.Name = nameField.text;
        gameController.SaveStats();
    }

    public void ToggleOptionsInOut()
    {
        gameController.SaveStats();

        bool b = OptionsAnimation.GetBool("OptionsClicked");
        OptionsAnimation.SetBool("OptionsClicked", !b);

        bool WDb = WDOptionsAnimation.GetBool("WDOptionsClicked");
        if (WDb) WDOptionsAnimation.SetBool("WDOptionsClicked", !b);

        bool WSb = WSOptionsAnimation.GetBool("In");
        if (WSb) WSOptionsAnimation.SetBool("In", !b);

        bool Ab = AOptionsAnimation.GetBool("In");
        if (Ab) AOptionsAnimation.SetBool("In", !b);

        bool FWb = FWOptionsAnimation.GetBool("In");
        if (FWb) FWOptionsAnimation.SetBool("In", !b);

        bool ABb = AboutOptionsAnimation.GetBool("In");
        if (ABb) AboutOptionsAnimation.SetBool("In", !b);

        // global bools to reading which menu is open if any
        isMainMenuOpen = OptionsAnimation.GetBool("OptionsClicked");
        isWDOpen = WDOptionsAnimation.GetBool("WDOptionsClicked");
        isWSOpen = WSOptionsAnimation.GetBool("In");
        isAOpen = AOptionsAnimation.GetBool("In");
        isFWOpen = FWOptionsAnimation.GetBool("In");
        isAUOpen = AboutOptionsAnimation.GetBool("In");

        if (gameController.player.Name != "NewPlayer") { nameField.text = gameController.player.Name; }
    }

    public void ToggleWDOptionsInOut()
    {
        bool b = WDOptionsAnimation.GetBool("WDOptionsClicked");
        WDOptionsAnimation.SetBool("WDOptionsClicked", !b);
        if (b) isWDOpen = true;

        bool WSb = WSOptionsAnimation.GetBool("In");
        if (WSb) WSOptionsAnimation.SetBool("In", false);

        bool Ab = AOptionsAnimation.GetBool("In");
        if (Ab) AOptionsAnimation.SetBool("In", false);

        bool FWb = FWOptionsAnimation.GetBool("In");
        if (FWb) FWOptionsAnimation.SetBool("In", false);

        bool ABb = AboutOptionsAnimation.GetBool("In");
        if (ABb) AboutOptionsAnimation.SetBool("In", false);

        // global bools to reading which menu is open if any
        isMainMenuOpen = OptionsAnimation.GetBool("OptionsClicked");
        isWDOpen = WDOptionsAnimation.GetBool("WDOptionsClicked");
        isWSOpen = WSOptionsAnimation.GetBool("In");
        isAOpen = AOptionsAnimation.GetBool("In");
        isFWOpen = FWOptionsAnimation.GetBool("In");
        isAUOpen = AboutOptionsAnimation.GetBool("In");
    }

    public void ToggleWSOptionsInOut()
    {
        bool b = WSOptionsAnimation.GetBool("In");
        WSOptionsAnimation.SetBool("In", !b);
        if (b) isWSOpen = true;

        bool WDb = WDOptionsAnimation.GetBool("WDOptionsClicked");
        if (WDb) WDOptionsAnimation.SetBool("WDOptionsClicked", false);

        bool Ab = AOptionsAnimation.GetBool("In");
        if (Ab) AOptionsAnimation.SetBool("In", false);

        bool FWb = FWOptionsAnimation.GetBool("In");
        if (FWb) FWOptionsAnimation.SetBool("In", false);

        bool ABb = AboutOptionsAnimation.GetBool("In");
        if (ABb) AboutOptionsAnimation.SetBool("In", false);

        // global bools to reading which menu is open if any
        isMainMenuOpen = OptionsAnimation.GetBool("OptionsClicked");
        isWDOpen = WDOptionsAnimation.GetBool("WDOptionsClicked");
        isWSOpen = WSOptionsAnimation.GetBool("In");
        isAOpen = AOptionsAnimation.GetBool("In");
        isFWOpen = FWOptionsAnimation.GetBool("In");
        isAUOpen = AboutOptionsAnimation.GetBool("In");
    }

    public void ToggleAOptionsInOut()
    {
        bool b = AOptionsAnimation.GetBool("In");
        AOptionsAnimation.SetBool("In", !b);
        if (b) isAOpen = true;

        bool FWb = FWOptionsAnimation.GetBool("In");
        if (FWb) FWOptionsAnimation.SetBool("In", false);

        bool WDb = WDOptionsAnimation.GetBool("WDOptionsClicked");
        if (WDb) WDOptionsAnimation.SetBool("WDOptionsClicked", false);

        bool WSb = WSOptionsAnimation.GetBool("In");
        if (WSb) WSOptionsAnimation.SetBool("In", false);

        bool ABb = AboutOptionsAnimation.GetBool("In");
        if (ABb) AboutOptionsAnimation.SetBool("In", false);

        // global bools to reading which menu is open if any
        isMainMenuOpen = OptionsAnimation.GetBool("OptionsClicked");
        isWDOpen = WDOptionsAnimation.GetBool("WDOptionsClicked");
        isWSOpen = WSOptionsAnimation.GetBool("In");
        isAOpen = AOptionsAnimation.GetBool("In");
        isFWOpen = FWOptionsAnimation.GetBool("In");
        isAUOpen = AboutOptionsAnimation.GetBool("In");
    }

    public void ToggleFWOptionsInOut()
    {
        bool b = FWOptionsAnimation.GetBool("In");
        FWOptionsAnimation.SetBool("In", !b);
        if (b) isFWOpen = true;

        bool WDb = WDOptionsAnimation.GetBool("WDOptionsClicked");
        if (WDb) WDOptionsAnimation.SetBool("WDOptionsClicked", false);

        bool WSb = WSOptionsAnimation.GetBool("In");
        if (WSb) WSOptionsAnimation.SetBool("In", false);

        bool Ab = AOptionsAnimation.GetBool("In");
        if (Ab) AOptionsAnimation.SetBool("In", false);

        bool ABb = AboutOptionsAnimation.GetBool("In");
        if (ABb) AboutOptionsAnimation.SetBool("In", false);

        // global bools to reading which menu is open if any
        isMainMenuOpen = OptionsAnimation.GetBool("OptionsClicked");
        isWDOpen = WDOptionsAnimation.GetBool("WDOptionsClicked");
        isWSOpen = WSOptionsAnimation.GetBool("In");
        isAOpen = AOptionsAnimation.GetBool("In");
        isFWOpen = FWOptionsAnimation.GetBool("In");
        isAUOpen = AboutOptionsAnimation.GetBool("In");
    }

    public void ToggleAboutUsInOut()
    {
        bool b = AboutOptionsAnimation.GetBool("In");
        AboutOptionsAnimation.SetBool("In", !b);
        if (b) isAUOpen = true;

        bool WDb = WDOptionsAnimation.GetBool("WDOptionsClicked");
        if (WDb) WDOptionsAnimation.SetBool("WDOptionsClicked", false);

        bool WSb = WSOptionsAnimation.GetBool("In");
        if (WSb) WSOptionsAnimation.SetBool("In", false);

        bool Ab = AOptionsAnimation.GetBool("In");
        if (Ab) AOptionsAnimation.SetBool("In", false);

        bool FWb = FWOptionsAnimation.GetBool("In");
        if (FWb) FWOptionsAnimation.SetBool("In", false);

        // global bools to reading which menu is open if any
        isMainMenuOpen = OptionsAnimation.GetBool("OptionsClicked");
        isWDOpen = WDOptionsAnimation.GetBool("WDOptionsClicked");
        isWSOpen = WSOptionsAnimation.GetBool("In");
        isAOpen = AOptionsAnimation.GetBool("In");
        isFWOpen = FWOptionsAnimation.GetBool("In");
        isAUOpen = AboutOptionsAnimation.GetBool("In");
    }
}
