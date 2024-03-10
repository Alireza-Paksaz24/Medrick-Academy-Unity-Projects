using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _UICanvas;
    [SerializeField] private GameObject _settingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OpenSettingPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenSettingPanel()
    {
        _canvas.SetActive(false);
        _UICanvas.SetActive(false);
        _settingCanvas.SetActive(true);
    }
}
