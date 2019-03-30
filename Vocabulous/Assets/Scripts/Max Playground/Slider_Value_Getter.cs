//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using UnityEngine.UI;
using UnityEngine;

// Unity really has to make this a feature
// converts GUI slider values and modifies the GC variables associated with them.
public class Slider_Value_Getter : MonoBehaviour
{
    public bool MusicSlider;
    public bool SFXSlider;


    private Slider mySlider;
    private GC gc;
    void Start()
    {
        mySlider = GetComponent<Slider>();
        gc = GC.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (MusicSlider) mySlider.value = gc.player.MusicVolume;
        if (SFXSlider) mySlider.value = gc.player.SFXVolume;
    }
}
