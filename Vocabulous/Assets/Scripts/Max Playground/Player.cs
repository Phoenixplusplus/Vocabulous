using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerStats
{
    public int NewPlayer;          // flag RE if there is a new Player ... if 0 (or null) no Player, create a default and set to 1
    public string Name;
    public float MusicVolume;      // 0 = Mute, 1 = full
    public float SFXVolume;        // 0 = Mute, 1 = full

    public int WordDiceSize;       // 4 = 4x4 (default), 5 = 5x5, 6 = 6x6
    public int WordDiceGameLength; // 30,60,120,180 seconds
    public int WDPlays;             // STATS
    public int WDHighscore;         // GUI & STATS
    public int WDLongest;           // GUI & STATS
    public float WDAverageScore;    // GUI & STATS
    public int WDMostWords;         // GUI & STATS
    public float WDAverageWords;    // GUI & STATS

    public int WordSearchSize;     // 10 (default), assets not configured for more at the mo
    public int WordSearchGameLength;
    public int WordSearchMinimumLengthWord;
    public int WordSearchMaximumLengthWord;
    public int WordSearchFourLetterWordsCount;
    public int WordSearchFiveLetterWordsCount;
    public int WordSearchSixLetterWordsCount;
    public int WordSearchSevenLetterWordsCount;
    public int WordSearchEightLetterWordsCount;
    public int WordSearchBestTime;
    public int WordSearchAverageTime;
    public int WordSearchWorstTime;
    public int WordSearchTimesCompleted;
    public int WordSearchTimesQuit;

    public int AnagramMinLength;   // 4,5,6 (Default),7,8
    public int ALevel;
    public int AExtras;
    public int AHints;

    public int FWHighScore, FWLongestWord, FWTimesCompleted, FWGameTime;
    public float FWAverageScore, FWAverageWord;

}


public class PlayerManager
{
    public PlayerStats LoadPlayer()
    {
        PlayerStats ret = new PlayerStats();
        int PTest = PlayerPrefs.GetInt("NewPlayer");
        if (PTest == 0) // nothing loaded, make and save a default
        {
            ret = GetDefault();
            SavePlayer(ret);
        }
        else
        {
            ret.NewPlayer = 1;
            ret.Name = PlayerPrefs.GetString("Name");
            ret.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            ret.SFXVolume = PlayerPrefs.GetFloat("SFXVolume");

            ret.WordDiceSize = PlayerPrefs.GetInt("WordDiceSize");
            ret.WordDiceGameLength = PlayerPrefs.GetInt("WordDiceGameLength");
            ret.WDPlays = PlayerPrefs.GetInt("WDPlays");
            ret.WDHighscore = PlayerPrefs.GetInt("WDHighscore");
            ret.WDLongest = PlayerPrefs.GetInt("WDLongest");
            ret.WDAverageScore = PlayerPrefs.GetFloat("WDAverageScore");
            ret.WDMostWords = PlayerPrefs.GetInt("WDMostWords");
            ret.WDAverageWords = PlayerPrefs.GetFloat("WDAverageWords");

            ret.WordSearchSize = PlayerPrefs.GetInt("WordSearchSize");
            ret.WordSearchGameLength = PlayerPrefs.GetInt("WordSearchGameLength");
            ret.WordSearchMinimumLengthWord = PlayerPrefs.GetInt("WordSearchMinimumLengthWord");
            ret.WordSearchMaximumLengthWord = PlayerPrefs.GetInt("WordSearchMaximumLengthWord");
            ret.WordSearchFourLetterWordsCount = PlayerPrefs.GetInt("WordSearchFourLetterWordsCount");
            ret.WordSearchFiveLetterWordsCount = PlayerPrefs.GetInt("WordSearchFiveLetterWordsCount");
            ret.WordSearchSixLetterWordsCount = PlayerPrefs.GetInt("WordSearchSixLetterWordsCount");
            ret.WordSearchSevenLetterWordsCount = PlayerPrefs.GetInt("WordSearchSevenLetterWordsCount");
            ret.WordSearchEightLetterWordsCount = PlayerPrefs.GetInt("WordSearchEightLetterWordsCount");
            ret.WordSearchBestTime = PlayerPrefs.GetInt("WordSearchBestTime");
            ret.WordSearchAverageTime = PlayerPrefs.GetInt("WordSearchAverageTime");
            ret.WordSearchWorstTime = PlayerPrefs.GetInt("WordSearchWorstTime");
            ret.WordSearchTimesCompleted = PlayerPrefs.GetInt("WordSearchTimesCompleted");
            ret.WordSearchTimesQuit = PlayerPrefs.GetInt("WordSearchTimesQuit");

            ret.AnagramMinLength = PlayerPrefs.GetInt("AnagramMinLength");
            ret.ALevel = PlayerPrefs.GetInt("ALevel");
            ret.AExtras = PlayerPrefs.GetInt("AExtras");
            ret.AHints = PlayerPrefs.GetInt("AHints");

            ret.FWHighScore = PlayerPrefs.GetInt("FWHighScore");
            ret.FWLongestWord = PlayerPrefs.GetInt("FWLongestWord");
            ret.FWTimesCompleted = PlayerPrefs.GetInt("FWTimesCompleted");
            ret.FWAverageScore = PlayerPrefs.GetFloat("FWAverageScore");
            ret.FWAverageWord = PlayerPrefs.GetFloat("FWAverageWord");
            ret.FWGameTime = PlayerPrefs.GetInt("FWGameTime");
        }
        return ret;
    }

    public void SavePlayer(PlayerStats player)
    {
        PlayerPrefs.SetInt("NewPlayer", 1);
        PlayerPrefs.SetString("Name", player.Name);
        PlayerPrefs.SetFloat("MusicVolume", player.MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume",player.SFXVolume);

        PlayerPrefs.SetInt("WordDiceSize", player.WordDiceSize);
        PlayerPrefs.SetInt("WordDiceGameLength", player.WordDiceGameLength);
        PlayerPrefs.SetInt("WDPlays", player.WDPlays);
        PlayerPrefs.SetInt("WDHighscore", player.WDHighscore);
        PlayerPrefs.SetInt("WDLongest", player.WDLongest);
        PlayerPrefs.SetFloat("WDAverageScore", player.WDAverageScore);
        PlayerPrefs.SetInt("WDMostWords", player.WDMostWords);
        PlayerPrefs.SetFloat("WDAverageWords", player.WDAverageWords);

        PlayerPrefs.SetInt("WordSearchSize", player.WordSearchSize);
        PlayerPrefs.SetInt("WordSearchGameLength", player.WordSearchGameLength);
        PlayerPrefs.SetInt("WordSearchMinimumLengthWord", player.WordSearchMinimumLengthWord);
        PlayerPrefs.SetInt("WordSearchMaximumLengthWord", player.WordSearchMaximumLengthWord);
        PlayerPrefs.SetInt("WordSearchFourLetterWordsCount", player.WordSearchFourLetterWordsCount);
        PlayerPrefs.SetInt("WordSearchFiveLetterWordsCount", player.WordSearchFiveLetterWordsCount);
        PlayerPrefs.SetInt("WordSearchSixLetterWordsCount", player.WordSearchSixLetterWordsCount);
        PlayerPrefs.SetInt("WordSearchSevenLetterWordsCount", player.WordSearchSevenLetterWordsCount);
        PlayerPrefs.SetInt("WordSearchEightLetterWordsCount", player.WordSearchEightLetterWordsCount);
        PlayerPrefs.SetInt("WordSearchBestTime", player.WordSearchBestTime);
        PlayerPrefs.SetInt("WordSearchAverageTime", player.WordSearchAverageTime);
        PlayerPrefs.SetInt("WordSearchWorstTime", player.WordSearchWorstTime);
        PlayerPrefs.SetInt("WordSearchTimesCompleted", player.WordSearchTimesCompleted);
        PlayerPrefs.SetInt("WordSearchTimesQuit", player.WordSearchTimesQuit);

        PlayerPrefs.SetInt("AnagramMinLength", player.AnagramMinLength);
        PlayerPrefs.SetInt("ALevel", player.ALevel);
        PlayerPrefs.SetInt("AExtras", player.AExtras);
        PlayerPrefs.SetInt("AHints", player.AHints);

        PlayerPrefs.SetInt("FWHighScore", player.FWHighScore);
        PlayerPrefs.SetInt("FWLongestWord", player.FWLongestWord);
        PlayerPrefs.SetInt("FWTimesCompleted", player.FWTimesCompleted);
        PlayerPrefs.SetInt("FWGameTime", player.FWGameTime);
        PlayerPrefs.GetFloat("FWAverageScore", player.FWAverageScore);
        PlayerPrefs.GetFloat("FWAverageWord", player.FWAverageWord);
    }

    public PlayerStats GetDefault()
    {
        PlayerStats ret = new PlayerStats();

        ret.NewPlayer = 1;          // flag RE if there is a new Player ... if 0 (or null) no Player, create a default and set to 1
        ret.Name = "NewPlayer";
        ret.MusicVolume = 1.0f;    // 0 = Mute, 1 = full
        ret.SFXVolume = 1.0f;      // 0 = Mute, 1 = full

        ret.WordDiceSize = 4;      // 4 = 4x4 (default), 5 = 5x5, 6 = 6x6
        ret.WordDiceGameLength = 180; // purely for testing
        ret.WDPlays = 0;
        ret.WDHighscore = 0;
        ret.WDLongest = 0;
        ret.WDAverageScore = 0.0f;
        ret.WDMostWords = 0;
        ret.WDAverageWords = 0.0f;

        ret.WordSearchSize = 10;     // 10 = 10x10, 15 = 15x15 (default), 20 = 20x20
        ret.WordSearchGameLength = 599;
        ret.WordSearchMinimumLengthWord = 4;
        ret.WordSearchMaximumLengthWord = 8;
        ret.WordSearchFourLetterWordsCount = 4;
        ret.WordSearchFiveLetterWordsCount = 4;
        ret.WordSearchSixLetterWordsCount = 2;
        ret.WordSearchSevenLetterWordsCount = 0;
        ret.WordSearchEightLetterWordsCount = 0;
        ret.WordSearchBestTime = 599;
        ret.WordSearchAverageTime = 0;
        ret.WordSearchWorstTime = 0;
        ret.WordSearchTimesCompleted = 0;
        ret.WordSearchTimesQuit = 0;

        ret.AnagramMinLength = 6;
        ret.ALevel = 0;
        ret.AExtras = 0;
        ret.AHints = 10;

        ret.FWHighScore = 0;
        ret.FWLongestWord = 0;
        ret.FWTimesCompleted = 0;
        ret.FWAverageScore = 0;
        ret.FWAverageWord = 0;
        ret.FWGameTime = 120;

        return ret;
    }

    public void ResetToDefault()
    {
        PlayerStats ret = GetDefault();
        SavePlayer(ret);
    }

    // Will need a series of "Reset" functions here laterz
    public void ResetAnagrams()
    {
        PlayerPrefs.SetInt("AnagramMinLength", 6);
        PlayerPrefs.SetInt("ALevel", 0);
        PlayerPrefs.SetInt("AExtras", 0);
        PlayerPrefs.SetInt("AHints", 10);
    }

    public void ResetWordDice ()
    {
        PlayerPrefs.SetInt("WordDiceSize", 4);
        PlayerPrefs.SetInt("WordDiceGameLength", 180);
        PlayerPrefs.SetInt("WDPlays", 0);
        PlayerPrefs.SetInt("WDHighscore", 0);
        PlayerPrefs.SetInt("WDLongest", 0);
        PlayerPrefs.SetFloat("WDAverageScore", 0f);
        PlayerPrefs.SetInt("WDMostWords", 0);
        PlayerPrefs.SetFloat("WDAverageWords", 0f);
    }


}
