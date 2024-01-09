using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private float lastYPosition; // To track the last highest Y position of the player

    void Start()
    {
        // Initialize the last Y position to the player's current Y position
        lastYPosition = player.position.y;
    }

    void Update()
    {
        // Check if the player has moved up
        if (player.position.y > lastYPosition)
        {
            // Update camera position to follow player's Y position, keeping X and Z constant
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
            
            // Update the last Y position to the player's current Y position
            lastYPosition = player.position.y;
        }
        // If the player moves down, do not update the camera position
    }
}