using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed;

    private Vector2 direction;
    private void Start()
    {
        this.transform.parent = GameObject.FindWithTag("SpawnManager").transform;
        _speed = 1.5f;
    }

    public void SetDirection(Vector2 dir)
    {
        this.direction = dir;
        if (direction.x < 0.2 && direction.y < 0.2)
            direction *= 2.5f;
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
            other.GetComponent<Enemy>().Shoot();
            Destroy(this.gameObject);
        }
    }
}