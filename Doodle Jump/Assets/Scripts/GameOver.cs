using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _gameOverInfoText;

    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    public void ActiveGameOver(float posi)
    {
        var str = Convert.ToString(Convert.ToInt32(posi * 100)) +" = "+ "ﺯﺎﯿﺘﻣﺍ";
        _gameOverInfoText.text = str;
        this.gameObject.SetActive(true);
        this.transform.position = new Vector3(0.2f,posi-30 ,0);
        SaveToDB(Convert.ToInt32(posi * 100));
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene("Start");
    }

    void SaveToDB(int score)
    {
        GameObject.Find("Game_Manager").GetComponent<GameManager>().AddNewPlayer(StaticValue.username, score);
    }
}
