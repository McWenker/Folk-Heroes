using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public static SFXHandler instance;

    [SerializeField] private AudioSource SFXsource;
	
    public static void PlaySFXStatic(AudioClip SFXtoPlay)
    {
        instance.PlaySFX(SFXtoPlay);
    }

    public static void PlaySFXStatic(AudioClip SFXtoPlay, float flutterAmp)
    {
        instance.PlaySFX(SFXtoPlay, flutterAmp);
    }

    private void PlaySFX(AudioClip SFXtoPlay)
    {
        SFXsource.clip = SFXtoPlay;
        SFXsource.Play();
    }

    private void PlaySFX(AudioClip SFXtoPlay, float flutterAmp)
    {
        SFXsource.clip = SFXtoPlay;
        SFXsource.pitch = Random.Range(1 - flutterAmp / 2, 1 + flutterAmp / 2);
        SFXsource.Play();
    }

    private void Awake()
    {
        instance = this;
    }
}
