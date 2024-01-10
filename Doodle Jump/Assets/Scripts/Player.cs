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
        _speed = new Vector2(0,12);
        rb = GetComponent<Rigidbody2D>();
        lastPlatformY = _spawnManager.SpawnPlatforms(lastPlatformY, _gameManager.level);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lastPlatformY - this.transform.position.y < 5)
        {
            _gameManager.level++;
            lastPlatformY = _spawnManager.SpawnPlatforms(lastPlatformY, _gameManager.level);
        }

        _speed.y -= _gravity;
        if (Input.GetKey(KeyCode.A))
            _speed.x = -6;
        else if (Input.GetKey(KeyCode.D))
            _speed.x = 6;
        else
            _speed.x = 0;
        if (this.transform.position.x <= -3.33f)
        {
            var vector3 = this.transform.position;
            vector3.x = 3.0f;
            this.transform.position = vector3;
        }
        if (this.transform.position.x >= 2.99f)
        {
            var vector3 = this.transform.position;
            vector3.x = -3.32f;
            this.transform.position = vector3;
        }

        rb.velocity = _speed;
    }

    void Jump()
    {
        _speed.y = 12;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Contains("Plat") && _speed.y < 0)
        {
            if (other.gameObject.tag == "Weak_Plat")
            {
                other.gameObject.GetComponent<Platforms>().Break();
                
            }else
                Jump();
            
        }
    }
}
