using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private float _speed = 0;
    private int level;
    private bool _isPlayed = false;
    private GameObject _camera;

    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _audioClips;
    
    void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera");
        level = GameObject.Find("Game_Manager").GetComponent<GameManager>().level;
        if (level > 4 && Random.Range(0,10) < level)
            _speed = 0.08f;
        if (this.gameObject.name.Contains("3"))
            _speed = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.gameObject.name.Contains("3"))
            return;
        if (this.transform.position.y - _camera.transform.position.y < 10 && !_isPlayed)
        {
            _audio.Play();
            _isPlayed = true;
        }
            
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
            Destroy(this.gameObject,0.02f);
            _audio.clip = _audioClips;
        }else if (this.name.Contains("2"))
        {
            this.name = "Enemy1";
        }
    }
}
