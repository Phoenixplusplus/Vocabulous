using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Placeholders only
public enum Music { Techno, Rock, Smooth, Groove, Indian, Noble, Prestige, Arcade, Punk, Spiritual }
public enum SFX { Bang, Spring, Boom, Laser, Clang, Titter, Woohoo, Whip1, Whip2, Whoosh }

// PLACEHOLDER ONLY ... needs to be rebuild (with the lessons learnt from ToeerL)
[RequireComponent(typeof(AudioSource))]
public class SoundMan : MonoBehaviour
{
    private GC controller;
    public AudioClip[] MusicFiles = new AudioClip[10];
    public AudioClip[] SFXFiles = new AudioClip[10];


    // Start is called before the first frame update
    void Start()
    {
        controller = GC.Instance;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
}
