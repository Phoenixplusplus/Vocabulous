﻿using System.Collections;
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
    public int WDPlays;
    public int WDHighscore;
    public int WDLongest;
    public float WDAverageScore;
    public float WDAverageWords;

    public int WordSearchSize;     // 10 = 10x10, 15 = 15x15 (default), 20 = 20x20
    public int WordSearchGameLength;
    public int WordSearchMinimumLengthWord;
    public int WordSearchMaximumLengthWord;
    public int WordSearchFourLetterWordsCount;
    public int WordSearchFiveLetterWordsCount;
    public int WordSearchSixLetterWordsCount;
    public int WordSearchSevenLetterWordsCount;
    public int WordSearchEightLetterWordsCount;

    public int AnagramMinLength;   // 4,5,6 (Default),7,8
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

            ret.AnagramMinLength = PlayerPrefs.GetInt("AnagramMinLength");

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

        PlayerPrefs.SetInt("AnagramMinLength", player.AnagramMinLength);

    }

    public PlayerStats GetDefault()
    {
        PlayerStats ret = new PlayerStats();

        ret.NewPlayer = 1;          // flag RE if there is a new Player ... if 0 (or null) no Player, create a default and set to 1
        ret.Name = "NewPlayer";
        ret.MusicVolume = 1.0f;    // 0 = Mute, 1 = full
        ret.SFXVolume = 1.0f;      // 0 = Mute, 1 = full

        ret.WordDiceSize = 4;      // 4 = 4x4 (default), 5 = 5x5, 6 = 6x6
        ret.WordDiceGameLength = 30; // purely for testing
        ret.WDPlays = 0;
        ret.WDHighscore = 0;
        ret.WDLongest = 0;
        ret.WDAverageScore = 0.0f;
        ret.WDAverageWords = 0.0f;

        ret.WordSearchSize = 10;     // 10 = 10x10, 15 = 15x15 (default), 20 = 20x20
        ret.WordSearchGameLength = 60;
        ret.WordSearchMinimumLengthWord = 4;
        ret.WordSearchMaximumLengthWord = 8;
        ret.WordSearchFourLetterWordsCount = 4;
        ret.WordSearchFiveLetterWordsCount = 4;
        ret.WordSearchSixLetterWordsCount = 2;
        ret.WordSearchSevenLetterWordsCount = 0;
        ret.WordSearchEightLetterWordsCount = 0;

        ret.AnagramMinLength = 6;

        return ret;
    }

    public void ResetToDefault()
    {
        PlayerStats ret = GetDefault();
        SavePlayer(ret);
    }

    // Will need a series of "Reset" functions here laterz

}
