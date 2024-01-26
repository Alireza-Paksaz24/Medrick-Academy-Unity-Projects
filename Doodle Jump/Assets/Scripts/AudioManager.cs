using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] Audio;
    public void SetAudio(int num)
    {
        this.GetComponent<AudioSource>().clip = Audio[num];
    }
}
