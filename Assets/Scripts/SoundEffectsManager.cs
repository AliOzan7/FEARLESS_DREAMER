using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : SingletonPersistent<SoundEffectsManager>
{
    [SerializeField] AudioSource sfxAudioSource;

    [SerializeField] AudioClip slashingWithWeapon;
    [SerializeField] AudioClip throwingWeapon;
    [SerializeField] AudioClip holdingTheChargedWeapon;
    [SerializeField] AudioClip electricHit;
    [SerializeField] AudioClip ghostPoof;
    [SerializeField] AudioClip clownPoof;
    [SerializeField] AudioClip UIButton;
    [SerializeField] AudioClip gameOver;


    //public void PlayBallHittingPaddle() => sfxAudioSource.PlayOneShot(ballHittingPaddle[Random.Range(0, ballHittingPaddle.Length)]);
    public void PlaySlashingWithWeapon() => sfxAudioSource.PlayOneShot(slashingWithWeapon);
    public void PlayThrowingWeapon() => sfxAudioSource.PlayOneShot(throwingWeapon);
    public void PlayHoldingTheChargedWeapon()
    {
        sfxAudioSource.clip = holdingTheChargedWeapon;
        sfxAudioSource.Play();
    }

    public void PlayElectricHit() => sfxAudioSource.PlayOneShot(electricHit);
    public void PlayGhostPoof() => sfxAudioSource.PlayOneShot(ghostPoof);
    public void PlayClownPoof() => sfxAudioSource.PlayOneShot(clownPoof);
    public void PlayUIButton() => sfxAudioSource.PlayOneShot(clownPoof);
    public void PlayGameOver() => sfxAudioSource.PlayOneShot(gameOver);
    public void Pause() => sfxAudioSource?.Pause();
    public void Unpause() => sfxAudioSource?.UnPause();
    public void ClearSounds() => sfxAudioSource?.Stop();
}
