using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;

    public float a;

    public LayerMask groundLayer;

    // Best Start: Gravity = 5 / _SPEED = 20
    [SerializeField] private float _SPEED = 20.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var isGrounded = true;//(rb.velocity.y == 0.0f);
        if (isGrounded)
        {
            rb.velocity += Vector2.up * _SPEED;
        }

        rb.velocity += Vector2.left * Input.GetAxis("Horizontal") * _SPEED/a;
    }
}
    
