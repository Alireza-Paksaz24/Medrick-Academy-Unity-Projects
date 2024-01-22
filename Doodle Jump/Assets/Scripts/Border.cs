using UnityEngine;

public class Border : MonoBehaviour
{
    private bool _flag = true;
    
    // Start is called before the first frame update
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }else if (Time.time > 0.5f && _flag)
        {
            _flag = false;
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().GameEnded();
            var player = GetComponent<Player>();
            if (player != null)
                player.NoGravity();
        }
    }
}
