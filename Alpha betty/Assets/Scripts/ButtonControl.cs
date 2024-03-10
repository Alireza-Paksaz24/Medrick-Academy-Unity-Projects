using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _UICanvas;
    [SerializeField] private GameObject _settingCanvas;

    [SerializeField] private Button _settingButton;

    [SerializeField] private Button _refreshButton;
    
    // Start is called before the first frame update
    void Start()
    {
        _settingButton.onClick.AddListener(OpenSettingPanel);
        _refreshButton.onClick.AddListener(RefreshBoard);
    }

    private void OpenSettingPanel()
    {
        _canvas.SetActive(false);
        _UICanvas.SetActive(false);
        _settingCanvas.SetActive(true);
    }

    private void RefreshBoard()
    {
        
    }
}
