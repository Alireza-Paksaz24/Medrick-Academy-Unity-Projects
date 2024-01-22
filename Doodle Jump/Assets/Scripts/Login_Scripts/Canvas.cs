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
        PlayerData.username = user;
    }
    
    // [System.Serializable]
    // public class PlayerData {
    //     public string username;
    //     public int score;
    //
    //     public PlayerData(string username, int score) {
    //         this.username = username;
    //         this.score = score;
    //     }
    // }
    //
    // public List<PlayerData> GetPlayerDataList() {
    //     string json = PlayerPrefs.GetString("PlayersJson", "[]");
    //     List<PlayerData> players = JsonUtility.FromJson<List<PlayerData>>(json);
    //     return players;
    // }
    //
    // public List<PlayerData> GetTopPlayers(int numberOfTopPlayers) {
    //     List<PlayerData> players = GetPlayerDataList();
    //
    //     // Sort players by score in descending order
    //     players.Sort((player1, player2) => player2.score.CompareTo(player1.score));
    //
    //     // If the list has fewer players than requested, return them all
    //     if (players.Count < numberOfTopPlayers) {
    //         return players;
    //     }
    //
    //     // Otherwise, return the top N players
    //     return players.GetRange(0, numberOfTopPlayers);
    // }

}

public static class PlayerData {
    public static string username = "Doodle";
    public static int score;
}