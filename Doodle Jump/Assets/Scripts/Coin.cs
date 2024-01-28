using UnityEditor;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AnimationClip moveUpAnimation; // اینجا یک Animation Clip را از Editor به این متغیر اختصاص دهید

    private Vector3 spawnPosition;
    private Animator animator;

    void Start()
    {
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StaticValue.playerBalance++;
            Destroy(this.gameObject);
        }
    }
}
