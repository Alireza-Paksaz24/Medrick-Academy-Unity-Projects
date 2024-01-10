using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 0;
    private int level;
    void Start()
    {
        level = GameObject.Find("Game_Manager").GetComponent<GameManager>().level;
        if (level > 4)
            _speed = 0.08f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_speed != 0)
        {
            this.transform.Translate(Vector3.left * _speed * level / 6);
            if (this.transform.position.x < -2f || this.transform.position.x > 2f)
                _speed *= -1;
        }
    }
}
