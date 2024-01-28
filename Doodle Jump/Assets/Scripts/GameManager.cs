using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private int _level = 1;

    public int level {
        get => _level;
        set => _level = value;
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

    [System.Serializable]
    public class PlayerDataList {
        public List<PlayerData> players = new List<PlayerData>();
    }

    private List<PlayerData> GetPlayerDataList() {
        string json = PlayerPrefs.GetString("PlayersJson", "{\"players\":[]}");
        PlayerDataList playerDataList = JsonUtility.FromJson<PlayerDataList>(json);
        return playerDataList.players ?? new List<PlayerData>();
    }

    private List<PlayerData> GetTopPlayers(int numberOfTopPlayers) {
        List<PlayerData> players = GetPlayerDataList();
        players.Sort((player1, player2) => player2.score.CompareTo(player1.score));
        if (players.Count < numberOfTopPlayers) {
            return players;
        }
        return players.GetRange(0, numberOfTopPlayers);
    }

    public void AddNewPlayer(string newUsername, int newScore) {
        string json = PlayerPrefs.GetString("PlayersJson", "{\"players\":[]}");
        PlayerDataList playerDataList = JsonUtility.FromJson<PlayerDataList>(json);
        playerDataList.players.Add(new PlayerData(newUsername, newScore));

        string updatedJson = JsonUtility.ToJson(playerDataList);
        PlayerPrefs.SetString("PlayersJson", updatedJson);
        PlayerPrefs.SetInt(StaticValue.username,StaticValue.playerBalance);
        UpdateTopPlayersList();
    }

    private void UpdateTopPlayersList() {
        StaticValue.topPlayer.Clear();
        foreach (var i in GetTopPlayers(10)) {
            string[] tempPlayer = new string[2] { i.username, i.score.ToString() };
            StaticValue.topPlayer.Add(tempPlayer);
        }
    }
}
