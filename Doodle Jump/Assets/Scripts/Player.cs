using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool puase = false;
    
    private Vector2 _speed;
    private Rigidbody2D rb;
    private float lastPlatformY;
    private float _gravity;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private AudioSource _audioManager;
    private SpriteRenderer _sprite;
    private float _lastFire;
    private float _fireRate;
    private bool _shrink;
    private Sprite[] _sprites;

    [SerializeField] private Sprite[] _normalSprite;
    [SerializeField] private Sprite[] _blueSprite;
    [SerializeField] private Sprite[] _bunnySprite;
    [SerializeField] private Sprite[] _astronautSprite;
    [SerializeField] private Sprite[] _ghostSprite;
    [SerializeField] private Sprite[] _indianaSprite;
    [SerializeField] private Sprite[] _snowSprite;
    [SerializeField] private Sprite[] _soccerSprite;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _ball;
    

    // Start is called before the first frame update
    void Start()
    {
        var _spriteArray = new [] {_normalSprite, _blueSprite, _bunnySprite, _astronautSprite, _ghostSprite, _indianaSprite, _snowSprite,_soccerSprite};
        _sprites = _spriteArray[StaticValue.choosenSprite];
        foreach (var i in _spriteArray)
        {
            if (i != _sprites)
            {
                var sprites = i;
                sprites = new Sprite[0];
            }
        }
        lastPlatformY = -10;
        _gravity = 0.2f;
        _shrink = false;
        _lastFire = 0.0f;
        _fireRate = 0.15f;
        _speed = new Vector2(0,10);
        rb = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        lastPlatformY = _spawnManager.SpawnPlatforms(lastPlatformY, _gameManager.level);
        _sprite = GetComponent<SpriteRenderer>();
        _audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioSource>();
        if (_gameManager == null)
            Debug.LogError("Game Manager is null");
        if (_spawnManager == null)
            Debug.LogError("Spawn Manager is null");
        if (_sprite == null)
            Debug.LogError("Sprite Render in Player is null");
        if (_audioManager == null)
            Debug.LogError("Audio Source is null");


        _sprite.sprite = _sprites[0];

#if UNITY_ANDROID
        Input.gyro.enabled = true;
#endif
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
        

        if (_shrink)
        {
            this.transform.localScale = this.transform.localScale - new Vector3(0.02f, 0.02f,0.01f);
            rb.velocity = _speed;
            if (this.transform.localScale.x < 0.1f)
            {
                _shrink = false;
                _speed = Vector2.zero;
                var _camera = GameObject.FindWithTag("MainCamera");
                _camera.GetComponent<Camera>().GameEnded();
                _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y - 30, -0.5f);
                Destroy(this.gameObject);
            }
        }
        else if (!puase)
            rb.velocity = Move();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !puase)
        {
            if ((Time.time - _lastFire) >= _fireRate)
            {
                _lastFire = Time.time;
                StartCoroutine(Fire(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if ((Time.time - _lastFire) >= _fireRate)
            {
                _lastFire = Time.time;
                StartCoroutine(Fire(new Vector3(this.transform.position.x,this.transform.position.y + 10,0)));
            }
        }
    }

    IEnumerator Fire(Vector3 posi)
    {
        // I hvae No idea what I've done
        if (posi.y - this.transform.position.y < 0)
            posi -= -2.0f * (this.transform.position - posi);
        var currentMousePosi = posi;
        var directionFromPlayerToClick = (currentMousePosi - this.transform.position).normalized;
        float angle = Mathf.Atan2(directionFromPlayerToClick.y, directionFromPlayerToClick.x) * Mathf.Rad2Deg;
        _gun.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        _gun.SetActive(true);
        GameObject bullet = Instantiate(_ball,_gun.transform.position,Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(directionFromPlayerToClick);
        var currentSprite = _sprite.sprite;
        _sprite.sprite = _sprites[2];
        yield return new WaitForSeconds(0.15f);
        if (currentSprite == _sprites[2])
            currentSprite = _sprites[1];
        _sprite.sprite = currentSprite;
        _gun.SetActive(false);
        PlayAudio(6);
    }
    Vector2 Move()
    {
        _speed.y -= _gravity;
#if UNITY_ANDROID
        _speed.x = 6.0f * Input.gyro.gravity.x;
#else
        if (Input.GetKey(KeyCode.A))
            _speed.x = -6;
        else if (Input.GetKey(KeyCode.D))
            _speed.x = 6;
        else
            _speed.x = 0;
#endif
        if (_sprite.sprite != _sprites[1] && _speed.x < 0)
            _sprite.sprite = _sprites[1];
        else if (_sprite.sprite != _sprites[0] && _speed.x > 0)
            _sprite.sprite = _sprites[0];
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
        _speed.y = 6;
        PlayAudio(4);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Contains("Plat") && _speed.y <= 0)
        {
            if (other.gameObject.tag == "Weak_Plat")
            {
                other.gameObject.GetComponent<Platforms>().Break();
                PlayAudio(0);
            }else
                Jump();
        }else if (other.gameObject.tag == "Enemy")
        {
            // Player behaviour when collition with enemy
            if ((other.transform.position.y - this.transform.position.y > -0.45f) && !this.gameObject.name.Contains("3"))
            {
                this.GetComponent<BoxCollider2D>().isTrigger = true;
                PlayAudio(3);
            }
            else if (!other.gameObject.name.Contains("3"))
            {
                other.gameObject.GetComponent<Enemy>().Shoot();
                PlayAudio(5);
                _speed.y = 8;
            }
        }
        
    }

    void FallIntoHole(Vector2 direction)
    {
        _speed = direction.normalized;
        _gravity = 0;
        _shrink = true;
        PlayAudio(2);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Enemy3"))
            this.FallIntoHole(other.transform.position - this.transform.position);
    }

    public void NoGravity()
    {
        _gravity = 0;
    }

    public void PlayAudio(int num)
    {
        _audioManager.GetComponent<AudioManager>().SetAudio(num);
        _audioManager.Play();
    }
}
