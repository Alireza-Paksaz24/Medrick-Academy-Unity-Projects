using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private Transform _player; // Reference to the player's transform
    private float lastYPosition; // To track the last highest Y position of the player
    private float _stopYPosition;
    private bool _gameEnded; // You need to set this when the game ends
    private float posi = 4.5f;
    

    void Start()
    {
        lastYPosition = _player.position.y;
        _gameEnded = false;
    }

    void Update()
    {
        if (_player == null)
            _player = GameObject.Find("GameOver").GetComponent<Transform>();
        if (!_gameEnded)
        {
            if (_player.position.y > lastYPosition)
            {
                transform.position = new Vector3(transform.position.x, _player.position.y, transform.position.z);
                
                lastYPosition = _player.position.y;
            }
        }
        else if (_stopYPosition < _player.position.y)
        {
            transform.position =  new Vector3(transform.position.x, _player.position.y + posi, transform.position.z);
            posi -= 0.02f;
            Destroy(_player.gameObject,3.0f);
        }
    }

    public void GameEnded()
    {
        _gameEnded = true;
        _stopYPosition = lastYPosition - 30;
        _gameOver.GetComponent<GameOver>().ActiveGameOver(lastYPosition);
    }
}