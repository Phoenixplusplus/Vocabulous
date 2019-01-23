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
    public int WordSearchSize;     // 10 = 10x10, 15 = 15x15 (default), 20 = 20x20
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
            ret.WordSearchSize = PlayerPrefs.GetInt("WordSearchSize");
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
        PlayerPrefs.SetInt("WordSearchSize", player.WordSearchSize);
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
        ret.WordSearchSize = 15;     // 10 = 10x10, 15 = 15x15 (default), 20 = 20x20
        ret.AnagramMinLength = 6;

        return ret;
    }

    // Will need a series of "Reset" functions here laterz

}
