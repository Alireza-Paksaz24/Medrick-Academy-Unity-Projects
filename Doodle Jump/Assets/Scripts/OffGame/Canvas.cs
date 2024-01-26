using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour
{
    public void OnDonePress()
    {
        SceneManager.LoadScene("Scenes/Start");
    }

    public void OnUsernameEnter(string user)
    {
        StaticValue.username = user;
    }
}

public static class StaticValue {
    public static string username = "Doodle";
    public static ArrayList topPlayer = new ArrayList();
}