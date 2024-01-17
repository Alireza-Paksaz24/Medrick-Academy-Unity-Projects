using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private float _speed = 0;
    private int level;
    void Start()
    {
        level = GameObject.Find("Game_Manager").GetComponent<GameManager>().level;
        if (level > 4 && Random.Range(0,10) < level)
            _speed = 0.08f;
        if (this.gameObject.name.Contains("3"))
            _speed = 0;
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

    public void Shoot()
    {
        if (this.name.Contains("1"))
        {
            Destroy(this.gameObject);
        }else if (this.name.Contains("2"))
        {
            this.name = "Enemy1";
        }
    }
}
