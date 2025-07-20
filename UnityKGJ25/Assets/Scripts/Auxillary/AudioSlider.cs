using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public  Slider aSlider;
    public AudioMixer aMixer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateAudioMixer()
    {
        if (aSlider && aMixer)
            aMixer.SetFloat("MasterVolume", aSlider.value);
    }
}
