using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundsController : MonoBehaviour
{
    public AudioSource buttonAudioSource;
    public AudioClip buttonClip;
    public AudioClip baseButtonClip;
    public void PlayButtonsSound()
    {
        buttonAudioSource.PlayOneShot(buttonClip);
    }    
    public void PlayBaseButtonsSound()
    {
        buttonAudioSource.PlayOneShot(baseButtonClip);
    }
}
