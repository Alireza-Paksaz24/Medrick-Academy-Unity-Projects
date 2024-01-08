using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _speed;
    private Rigidbody2D rb;
    [SerializeField] private float _gravity = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _speed = new Vector2(0,0);
        rb.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
