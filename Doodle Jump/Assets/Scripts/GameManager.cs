using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _level = 1;

    public int level
    {
        get => _level;
        set => _level= value;
    }
    [System.Serializable]
    public class PlayerData {
        public string username;
        public int score;
    
        public PlayerData(string username, int score) {
            this.username = username;
            this.score = score;
        }
    }
    
    private List<PlayerData> GetPlayerDataList() {
        string json = PlayerPrefs.GetString("PlayersJson", "[]");
        List<PlayerData> players = JsonUtility.FromJson<List<PlayerData>>(json);
        return players;
    }
    
    private List<PlayerData> GetTopPlayers(int numberOfTopPlayers) {
        List<PlayerData> players = GetPlayerDataList();
    
        // Sort players by score in descending order
        players.Sort((player1, player2) => player2.score.CompareTo(player1.score));
    
        // If the list has fewer players than requested, return them all
        if (players.Count < numberOfTopPlayers) {
            return players;
        }
    
        // Otherwise, return the top N players
        return players.GetRange(0, numberOfTopPlayers);
    }
    
    public void AddNewPlayer(string newUsername, int newScore) {
        // Retrieve the existing JSON
        string json = PlayerPrefs.GetString("PlayersJson", "[]");
        List<PlayerData> players = new List<PlayerData>();
        if (json != "[]")
            players = JsonUtility.FromJson<List<PlayerData>>(json);

        // Add the new player data
        PlayerData newPlayer = new PlayerData(newUsername, newScore);
        players.Add(newPlayer);

        // Serialize the updated list back to JSON
        string updatedJson = JsonUtility.ToJson(players);

        // Store the updated JSON string in PlayerPrefs
        PlayerPrefs.SetString("PlayersJson", updatedJson);
        StaticValue.topPlayer.Clear();
        foreach (var i in GetTopPlayers(10))
        {
            string[] tempPlayer = new string[2];
            tempPlayer[0] = i.username;
            tempPlayer[1] = i.score.ToString();
            StaticValue.topPlayer.Add(tempPlayer);
        }
    }
}

