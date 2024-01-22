using System;
using System.Collections;
using System.Collections.Generic;
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