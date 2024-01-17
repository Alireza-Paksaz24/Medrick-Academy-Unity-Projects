using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Vector2 direction;
    private void Start()
    {
        this.transform.parent = GameObject.FindWithTag("SpawnManager").transform;
        // _speed = 50;
    }

    public void SetDirection(Vector2 dir)
    {
        this.direction = dir;
    }
    void FixedUpdate()
    {
        transform.Translate(direction * _speed);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DestroyEnemy(other.gameObject);
    }

    void DestroyEnemy(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other);
            Destroy(this.gameObject);
        }
    }
}