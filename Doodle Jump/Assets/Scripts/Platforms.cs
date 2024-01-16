using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    private GameObject _camera;
    private GameManager _gameManager;
    private int level;
    [SerializeField]private float _speed = 0.12f;
    [SerializeField] private Sprite _breakSprite;
    private void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        level = _gameManager.level;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_camera.transform.position.y - this.transform.position.y > 5)
            Destroy(this.gameObject);
        if (this.tag == "Mover_Plat")
        {
            this.transform.Translate(Vector3.left * _speed * level / 8);
            if (this.transform.position.x < -2.1f || this.transform.position.x > 2.1f)
                _speed *= -1;
        }
    }

    public void Break()
    {
        if (this.tag == "Weak_Plat")
        {
            this.GetComponent<SpriteRenderer>().sprite = _breakSprite;
            var rb = this.GetComponent<Rigidbody2D>(); 

            rb.gravityScale = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            //Destroy(this.gameObject);
            Debug.Log("Destroyed");
        }
    }
}
