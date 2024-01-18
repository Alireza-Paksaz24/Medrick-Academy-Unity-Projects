using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Transform _cameraLastPosition;
    private TextMeshProUGUI _score;
    private float _lastPosition;
    void Start()
    {
        _score = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        _cameraLastPosition = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        _lastPosition = _cameraLastPosition.position.y;
    }

    private void Update()
    {
        if (_lastPosition < _cameraLastPosition.position.y)
        {
            _lastPosition = _cameraLastPosition.position.y;
            var scoreString =Convert.ToString(Convert.ToInt32(_lastPosition)) +" "+ "ﺯﺎﯿﺘﻣﺍ";
            _score.text = scoreString;
        }
    }
}
