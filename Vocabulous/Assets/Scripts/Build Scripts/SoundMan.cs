using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All Music and SFX are purchased and Free-to-use licence
// https://www.promusicpack.com/license.html
// http://www.soundeffectpack.com/license.html
// Sources (Open Commons) from https://www.freemusicpublicdomain.com/royalty-free-classical-music/

// Placeholders only
public enum Music { Bells,Chill,Country,Mantra,Morning,Suspense,Techno,WindOfChange,ClassPiano,ClassGuitar,ModPiano,ModGuitar }
public enum SFX { NineSecTick,Applause,Bell,Cheer,Chime,ClockChime,CoinDrop,Coins,CoinsIn, CoinsUp,CorkPop,Key1,Key2,DrumWhistleCrash,PianoSlideDown,PianoSlideUp,Signing,Whistle,Yeehaw,Chalk_Up, Chalk_Write,Dice_Roll }
public enum TileSFX {Collect1,Collect2,Drop1,Drop2,Drop3,Drop4,QuickMove,Shuffle1,Shuffle2,Shuffle3,Shuffle4,Simp_Down,Splerge, ShuffleQuick, ShuffleQuick2, Splerge2 };
public enum WordSFX { GetWord, GetWord2, GetWord3, GetWord4, GetWord5, GetWord6, SameWord };
public enum MiscSFX { TimeUp, TimeUp2, TimeUp3, Pop, Pop2, Pop3, Pop4, Pop5, Pop6, SwishQuick };

// PLACEHOLDER ONLY ... needs to be rebuild (with the lessons learnt from TowerL)
// N/B Sound manager has a number of AudioSources ... held in sources[]
// sources[0] = Backing Track
// sources[1 - (SFXChannels-1)] are for SFX 
[RequireComponent(typeof(AudioSource))]
public class SoundMan : MonoBehaviour
{
    private GC gc;
    public AudioClip[] MusicFiles = new AudioClip[12];
    public AudioClip LibraryAmbient;
    public AudioClip[] SFXFiles = new AudioClip[22];
    public AudioClip[] TileSFX = new AudioClip[16];
    public AudioClip[] WordSFX = new AudioClip[7];
    public AudioClip[] MiscSFX = new AudioClip[10];
    public bool TEST_MUSIC;
    public Music Music_To_test;
    public bool TEST_SFX;
    public SFX SFX_To_test;


    private AudioSource[] sources;
    private int SFXChannels;
    private float MusicVol;
    private float SFXVol;
    private int CurrSFXChannel = 1;

    #region UITY API
    void Start()
    {
        gc = GC.Instance;
        sources = GetComponents<AudioSource>();
        SFXChannels = sources.Length - 1;
        SetAllVolumes();
    }

#if UNITY_EDITOR
    void Update ()
    {
        // Only used to enable Inspector testing of Music/SFX Enums
        // Take care to remove the "#if UNITY_EDITOR" if you need an an actual Update() method
        if (TEST_MUSIC)
        {
            PlayMusic(Music_To_test);
            TEST_MUSIC = false;
        }
        if (TEST_SFX)
        {
            PlaySFX(SFX_To_test);
            TEST_SFX = false;
        }
    }
#endif

#endregion

    #region SET VOLUMES

    public void SetVolume_Music (float volume)
    {
        MusicVol = Mathf.Clamp(volume, 0, 1);
        SetMusicVolume();
    }

    public void SetVolume_SFX (float volume)
    {
        SFXVol = Mathf.Clamp(volume, 0, 1);
        SetSFXVolume();
    }

    void SetAllVolumes()
    {
        MusicVol = gc.player.MusicVolume;
        SFXVol = gc.player.SFXVolume;
        foreach (AudioSource AS in sources)
        {
            AS.volume = SFXVol;
        }
        sources[0].volume = MusicVol;
    }

    void SetMusicVolume()
    {
        sources[0].volume = MusicVol;
        gc.player.MusicVolume = MusicVol;
    }

    void SetSFXVolume()
    {
        gc.player.SFXVolume = SFXVol;
        for (int i = 1; i < SFXChannels; i++)
        {
            sources[i].volume = SFXVol;
        }
    }
    #endregion

    #region PLAY MUSIC/SFX

    public void PlayRandomTrack()
    {
        // thanks to https://answers.unity.com/questions/514555/enum-count.html (Feb 2019)
        int number = System.Enum.GetValues(typeof(Music)).Length;
        // and https://stackoverflow.com/questions/29482/cast-int-to-enum-in-c-sharp (Feb 2019)
        PlayMusic((Music)Random.Range(0, number));
    }

    public void PlayLobbyMusic()
    {
        sources[0].clip = LibraryAmbient;
        sources[0].loop = true;
        sources[0].Play(0);
    }

    public void PlayMusic(Music choice)
    {
        sources[0].clip = MusicFiles[(int)choice];
        sources[0].loop = true;
        sources[0].Play(0);
    }

    public void PlaySFX (SFX choice)
    {
        sources[CurrSFXChannel].clip = SFXFiles[(int)choice];
        sources[CurrSFXChannel].loop = false;
        sources[CurrSFXChannel].Play(0);
        ToggleChannel();
    }

    public void PlayTileSFX(TileSFX choice)
    {
        sources[CurrSFXChannel].clip = TileSFX[(int)choice];
        sources[CurrSFXChannel].loop = false;
        sources[CurrSFXChannel].Play(0);
        ToggleChannel();
    }

    public void PlayWordSFX(WordSFX choice)
    {
        sources[CurrSFXChannel].clip = WordSFX[(int)choice];
        sources[CurrSFXChannel].loop = false;
        sources[CurrSFXChannel].Play(0);
        ToggleChannel();
    }

    public void PlayMiscSFX(MiscSFX choice)
    {
        sources[CurrSFXChannel].clip = MiscSFX[(int)choice];
        sources[CurrSFXChannel].loop = false;
        sources[CurrSFXChannel].Play(0);
        ToggleChannel();
    }

    public void PlayTileSFX (TileSFX choice, float delay)
    {
        StartCoroutine(DelaySFX(choice, delay));
    }

    public void PlaySFX(SFX choice, float delay)
    {
        StartCoroutine(DelaySFX(choice, delay));
    }

    public void PlayWordSFX(WordSFX choice, float delay)
    {
        StartCoroutine(DelaySFX(choice, delay));
    }

    public void PlayMiscSFX(MiscSFX choice, float delay)
    {
        StartCoroutine(DelaySFX(choice, delay));
    }

    IEnumerator DelaySFX(SFX choice, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySFX(choice);
    }

    IEnumerator DelaySFX(TileSFX choice, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayTileSFX(choice);
    }

    IEnumerator DelaySFX(WordSFX choice, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayWordSFX(choice);
    }

    IEnumerator DelaySFX(MiscSFX choice, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayMiscSFX(choice);
    }

    private void ToggleChannel()
    {
        if (CurrSFXChannel == SFXChannels) { CurrSFXChannel = 1; }
        else { CurrSFXChannel++; }
    }
    #endregion

    #region KILL MUSIC &/OR SFX

    public void KillSFX ()
    {
        StopAllCoroutines(); // kills ones on delay
        for (int i = 1; i < SFXChannels; i++)
        {
            sources[i].clip = null;
        }
    }

    public void KillMusic ()
    {
        sources[0].clip = null;
    }

    public void KillAllSound ()
    {
        KillSFX();
        KillMusic();
    }

    #endregion

}

