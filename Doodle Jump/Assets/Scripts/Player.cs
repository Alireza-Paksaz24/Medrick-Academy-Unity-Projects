using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _speed;
    private Rigidbody2D rb;
    private float lastPlatformY = -4;
    [SerializeField] private float _gravity = 1.0f;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _speed = new Vector2(0,0);
        rb = GetComponent<Rigidbody2D>();
        lastPlatformY = _spawnManager.SpawnPlatforms(lastPlatformY, _gameManager.level);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPlatformY - this.transform.position.y < 5)
        {
            _gameManager.level++;
            lastPlatformY = _spawnManager.SpawnPlatforms(lastPlatformY, _gameManager.level);
        }

        _speed.y -= _gravity;
        rb.velocity = _speed;
    }

    void Jump()
    {
        _speed.y = 30;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
            Jump();
    }
}
