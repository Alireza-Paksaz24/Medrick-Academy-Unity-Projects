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
        var a = (string[]) StaticValue.topPlayer[0];
        Debug.Log(a);
        Debug.Log(a[0]);
    }
}
