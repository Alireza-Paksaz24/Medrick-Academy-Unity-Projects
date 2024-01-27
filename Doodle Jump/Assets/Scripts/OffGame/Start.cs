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
        SceneManager.LoadScene("Scenes/LeaderBoard");
    }

    public void OnLogout()
    {
        SceneManager.LoadScene("Scenes/Login");
    }
    
    public void OnStore()
    {
        SceneManager.LoadScene("Scenes/Store");
    }
    
}
