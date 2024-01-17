using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _speed;
    private Rigidbody2D rb;
    private float lastPlatformY = -10;
    private float _gravity = 0.5f;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private SpriteRenderer _sprite;
    private float _lastFire;
    private float _fireRate;
    
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _ball;
    
    // Start is called before the first frame update
    void Start()
    {
        _lastFire = 0.0f;
        _fireRate = 0.15f;
        _speed = new Vector2(0,20);
        rb = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        lastPlatformY = _spawnManager.SpawnPlatforms(lastPlatformY, _gameManager.level);
        _sprite = GetComponent<SpriteRenderer>();   
        if (_gameManager == null)
            Debug.LogError("Game Manager is null");
        if (_spawnManager == null)
            Debug.LogError("Spawn Manager is null");
        if (_sprite == null)
            Debug.LogError("Sprite Render in Player is null");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(Input.mousePosition);
        if (lastPlatformY - this.transform.position.y < 5)
        {
            _gameManager.level++;
            lastPlatformY = _spawnManager.SpawnPlatforms(lastPlatformY, _gameManager.level);
        }
        rb.velocity = Move();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if ((Time.time - _lastFire) >= _fireRate)
            {
                _lastFire = Time.time;
                StartCoroutine(Fire());
            }
        }
    }

    IEnumerator Fire()
    {
        // I hvae No idea what I've done
        var currentMousePosi = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var directionFromPlayerToClick = (currentMousePosi - this.transform.position).normalized;
        float angle = Mathf.Atan2(directionFromPlayerToClick.y, directionFromPlayerToClick.x) * Mathf.Rad2Deg;
        _gun.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        _gun.SetActive(true);
        GameObject bullet = Instantiate(_ball,_gun.transform.position,Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(directionFromPlayerToClick);
        var currentSprite = _sprite.sprite;
        _sprite.sprite = _sprites[2];
        yield return new WaitForSeconds(0.15f);
        _sprite.sprite = currentSprite;
        _gun.SetActive(false);
    }
    Vector2 Move()
    {        
        _speed.y -= _gravity;
        if (Input.GetKey(KeyCode.A))
            _speed.x = -6;
        else if (Input.GetKey(KeyCode.D))
            _speed.x = 6;
        else
            _speed.x = 0;
        if (_sprite.sprite != _sprites[0] && _speed.x < 0)
            _sprite.sprite = _sprites[0];
        else if (_sprite.sprite != _sprites[1] && _speed.x > 0)
            _sprite.sprite = _sprites[1];
        if (this.transform.position.x <= -3.20f)
        {
            var vector3 = this.transform.position;
            vector3.x = 2.9f;
            this.transform.position = vector3;
        }
        if (this.transform.position.x >= 2.91f)
        {
            var vector3 = this.transform.position;
            vector3.x = -3.21f;
            this.transform.position = vector3;
        }
        return _speed;
    }
    
    
    void Jump()
    {
        _speed.y = 12;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Contains("Plat") && _speed.y <= 0)
        {
            if (other.gameObject.tag == "Weak_Plat")
            {
                other.gameObject.GetComponent<Platforms>().Break();
                
            }else
                Jump();
        }else if (other.gameObject.tag == "Enemy")
        {
            if (other.transform.position.y - this.transform.position.y > -0.9)
            {
                Destroy(this.GetComponent<BoxCollider2D>());
            }
            else if (!other.gameObject.name.Contains("3"))
            {
                other.gameObject.GetComponent<Enemy>().Shoot();
                Jump();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
            Destroy(this.gameObject);
    }
}
