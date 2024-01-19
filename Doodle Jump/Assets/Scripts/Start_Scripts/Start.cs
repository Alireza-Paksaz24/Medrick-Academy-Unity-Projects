using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void OnLeader()
    {
        Debug.Log("Press");
    }
}
