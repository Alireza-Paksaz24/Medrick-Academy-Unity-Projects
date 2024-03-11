using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectControl : MonoBehaviour
{
    [SerializeField] private AudioClip _typingSound;
    [SerializeField] private AudioClip _endLine;
    [SerializeField] private AudioClip _wrongAnswer;

    [SerializeField] private AudioSource _SFXAudioSource;

    private bool _mute = false;

    public void OnTyping()
    {
        _SFXAudioSource.clip = _typingSound;
        PlaySound();
    }
    
    public void OnEnd(bool correct)
    {
        if (correct)
            _SFXAudioSource.clip = _endLine;
        else
            _SFXAudioSource.clip = _wrongAnswer;
        PlaySound();
    }

    private void PlaySound()
    {
        if (!_mute)
            _SFXAudioSource.Play();
    }
}
