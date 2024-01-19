using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _gameOverInfoText;
    public void ActiveGameOver(float posi)
    {
        var str = Convert.ToString(Convert.ToInt32(posi)) +" = "+ "ﺯﺎﯿﺘﻣﺍ";
        _gameOverInfoText.text = str;
        this.gameObject.SetActive(true);
        this.transform.position = new Vector3(0.2f,posi-31,0);
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene("Scenes/Game");
    }
}
