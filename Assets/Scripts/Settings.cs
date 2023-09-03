using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Settings : SingletonPersistent<Settings>
{
    [SerializeField] private AudioMixer mixer;
    public float MasterVolume { get; private set; } = 1f;
    public float SFXVolume { get; private set; } = 1f;
    public float MovementEdgeTolerance { get; private set; } = 0.4f;
    public float Lerp { get; private set; } = 0.12f;
    public float BallSpeed { get; private set; } = 20f;

    public void SetMusicVolume(float newValue)
    {
        MasterVolume = newValue;
        mixer.SetFloat("MusicVolume", Mathf.Log10(newValue) * 20);
    }

    public void SetSFXVolume(float newValue)
    {
        SFXVolume = newValue;
        mixer.SetFloat("SFXVolume", Mathf.Log10(newValue) * 20);
    }

    public void SetMovementEdgeTolerance(float newValue) => MovementEdgeTolerance = newValue;

    public void SetLerp(float newValue) => Lerp = newValue;

    public void SetBallSpeed(float newValue) => BallSpeed = newValue;
}
