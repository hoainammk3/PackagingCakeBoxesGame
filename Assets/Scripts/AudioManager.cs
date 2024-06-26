using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource vfxAudioSource;
    
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip clickClip;

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    public void PlayWinClip()
    {
        vfxAudioSource.clip = winClip;
        vfxAudioSource.PlayOneShot(winClip);
    }

    public void PlayLoseClip()
    {
        vfxAudioSource.clip = loseClip;
        vfxAudioSource.PlayOneShot(loseClip);
    }

    public void PlayMoveClip()
    {
        vfxAudioSource.clip = moveClip;
        vfxAudioSource.PlayOneShot(moveClip);
    }

    public void PlayClickClip()
    {
        vfxAudioSource.clip = clickClip;
        vfxAudioSource.PlayOneShot(clickClip);
    }

    public void SetVolumeMusic(float volume)
    {
        musicAudioSource.volume = volume;
    }

    public void SetVolumeVfx(float volume)
    {
        vfxAudioSource.volume = volume;
    }

    public float GetVolumeMusic()
    {
        return musicAudioSource.volume;
    }

    public float GetVolumeVfx()
    {
        return vfxAudioSource.volume;
    }
}
