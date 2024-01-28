using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour
{
    public void OnDonePress()
    {
        StaticValue.playerBalance = PlayerPrefs.GetInt(StaticValue.username, 0);
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
    public static int playerBalance = 500;
    public static int choosenSprite = 0;
}