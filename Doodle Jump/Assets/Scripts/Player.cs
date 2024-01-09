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
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        if (_speed.y < 0)
            _speed.y = 12;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
            Jump();
    }
}
