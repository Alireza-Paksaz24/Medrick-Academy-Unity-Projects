using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform _player; // Reference to the player's transform
    private float lastYPosition; // To track the last highest Y position of the player
    private float _stopYPosition;
    private bool _gameEnded; // You need to set this when the game ends
    private float posi = 4.5f;
    

    void Start()
    {
        // Initialize the last Y position to the player's current Y position
        lastYPosition = _player.position.y;
        _gameEnded = false;
    }

    void Update()
    {
        if (!_gameEnded)
        {
            // Check if the player has moved up
            if (_player.position.y > lastYPosition)
            {
                // Update camera position to follow player's Y position, keeping X and Z constant
                transform.position = new Vector3(transform.position.x, _player.position.y, transform.position.z);
                
                // Update the last Y position to the player's current Y position
                lastYPosition = _player.position.y;
            }
            // If the player moves down, do not update the camera position
        }
        else if (_stopYPosition < _player.position.y)
        {
            // When the game ends, move the camera downwards
            transform.position =  new Vector3(transform.position.x, _player.position.y + posi, transform.position.z);
            posi -= 0.02f;
            Destroy(_player.gameObject,3.0f);
        }
    }

    public void GameEnded()
    {
        _gameEnded = true;
        _stopYPosition = lastYPosition - 30;
    }
}