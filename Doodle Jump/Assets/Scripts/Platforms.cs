using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    private GameObject _camera;
    private GameManager _gameManager;
    [SerializeField]private float _speed = 15.0f;
    private void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera.transform.position.y - this.transform.position.y > 4.3)
            Destroy(this.gameObject);
        if (this.tag == "Mover_Plat")
        {
            this.transform.Translate(Vector3.left * _speed * _gameManager.level / 2);
            if (this.transform.position.x < -2.0f || this.transform.position.x > 2.0f)
                _speed *= -1;
        }
    }
}
