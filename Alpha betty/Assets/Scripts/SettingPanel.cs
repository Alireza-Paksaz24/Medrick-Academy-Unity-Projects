using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _UICanvas;
    [SerializeField] private GameObject _settingCanvas;
    [SerializeField] private GameObject _SFXButton;
    [SerializeField] private GameObject _musicButton;
    [SerializeField] private GameObject _SFXControler;
    [SerializeField] private AudioSource _backgroundMusic;

    private bool _isSFXAcrive = true;
    private bool _isMusicAcrive = true;
    public void OnSFX()
    {
        if (_isSFXAcrive)
        {
            Debug.Log("Deactive");
            _SFXButton.GetComponent<Image>().color = Color.red;
            _SFXControler.GetComponent<SoundEffectControl>().setMute(true);
        }
        else
        {
            Debug.Log("Active");
            _SFXButton.GetComponent<Image>().color = Color.green;
            _SFXControler.GetComponent<SoundEffectControl>().setMute(false);
        }
        _isSFXAcrive = !_isSFXAcrive;
    }

    public void OnMusic()
    {
        if (_isMusicAcrive)
        {
            Debug.Log("Deactive");
            _musicButton.GetComponent<Image>().color = Color.red;
            _backgroundMusic.mute = true;
        }
        else
        {
            Debug.Log("Active");
            _musicButton.GetComponent<Image>().color = Color.green;
            _backgroundMusic.mute = false;
        }
        _isMusicAcrive = !_isMusicAcrive;
    }
    
    public void OnDone()
    {
        Time.timeScale = 1;
        _canvas.SetActive(true);
        _UICanvas.SetActive(true);
        _settingCanvas.SetActive(false);
    }
}
