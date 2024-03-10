using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _UICanvas;
    [SerializeField] private GameObject _settingCanvas;

    [SerializeField] private Button _settingButton;

    [SerializeField] private Button _refreshButton;
    void Start()
    {
        _settingButton.onClick.AddListener(OpenSettingPanel);
        _refreshButton.onClick.AddListener(RefreshBoard);

        // Create a new EventTrigger component if not already attached to the _refreshButton
        EventTrigger trigger = _refreshButton.gameObject.GetComponent<EventTrigger>() ?? _refreshButton.gameObject.AddComponent<EventTrigger>();

        // Create a new entry for the pointer enter event
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter; // Specify the type of event

        // Add a callback to the entry
        pointerEnterEntry.callback.AddListener((data) => { PointerEnterRefresh((PointerEventData)data); });

        // Add the entry to the triggers list
        trigger.triggers.Add(pointerEnterEntry);
        
        _settingButton.transform.DORotate(new Vector3(0, 0, 360), 10, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    private void OpenSettingPanel()
    {
        _canvas.SetActive(false);
        _UICanvas.SetActive(false);
        _settingCanvas.SetActive(true);
    }

    private void RefreshBoard()
    {
        GameObject.Find("Block_Panel").GetComponent<BlcoksManager>().Shuffle2DArray();
    }

// Modified PointerEnterRefresh method to accept PointerEventData
    private void PointerEnterRefresh(PointerEventData eventData)
    {
        _refreshButton.transform.DOKill();
        _refreshButton.transform.DORotate(new Vector3(0, 0, 180), 1)
            .SetEase(Ease.Linear);
    }

}
