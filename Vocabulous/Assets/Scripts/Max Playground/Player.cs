using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerStats
{
    int NewPlayer;          // flag RE if there is a new Player ... if 0 (or null) no Player, create a default and set to 1
    string name;
    float MusicVolume;      // 0 = Mute, 1 = full
    float SFXVolume;        // 0 = Mute, 1 = full
    int WordDiceSize;       // 4 = 4x4 (default), 5 = 5x5, 6 = 6x6
    int WordSearchSize;     // 10 = 10x10, 15 = 15x15 (default), 20 = 20x20
    int AnagramMinLength;   // 4,5,6 (Default),7,8
}


public class PlayerManager
{

    public void LoadPlayer(ref PlayerStats player)
    {

    }

    public void SavePlayer(ref PlayerStats player)
    {

    }

    public void SetDefault(PlayerStats player)
    {

    }

}
