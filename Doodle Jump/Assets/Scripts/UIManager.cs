using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Transform _cameraLastPosition;
    private TextMeshProUGUI _score;
    private float _lastPosition;
    private Vector2 _playerVelocity;

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _player;
    
    
    void Start()
    {
        _playerVelocity = new Vector2(-100, -100);
        _score = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        _cameraLastPosition = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        _lastPosition = _cameraLastPosition.position.y;
    }

    private void Update()
    {
        if (_lastPosition < _cameraLastPosition.position.y)
        {
            _lastPosition = _cameraLastPosition.position.y;
            var scoreString =Convert.ToString(Convert.ToInt32(_lastPosition * 100)) +" = "+ "ﺯﺎﯿﺘﻣﺍ";
            _score.text = scoreString;
        }
    }

    public void OnPressPuase()
    {
        _player.GetComponent<Player>().puase = !_player.GetComponent<Player>().puase;
        var _velocity = _player.GetComponent<Rigidbody2D>().velocity;
        if (_playerVelocity == new Vector2(-100,-100))
        {
            _playerVelocity = new Vector2(_velocity.x,_velocity.y);
            _velocity = Vector2.zero;
            _pausePanel.SetActive(true);
        }
        else
        {
            _velocity = _playerVelocity;
            _playerVelocity = new Vector2(-100, -100);
            _pausePanel.SetActive(false);

        }

        _player.GetComponent<Rigidbody2D>().velocity = _velocity;

    }
}
