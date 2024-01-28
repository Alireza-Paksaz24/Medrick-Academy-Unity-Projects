using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject _userRecordPanel;

    [SerializeField]
    private GameObject _content;
    void Start()
    {
        foreach (var _player in StaticValue.topPlayer)
        {
            var player = (string [])_player;
            var recordScreen = Instantiate(_userRecordPanel);
            recordScreen.GetComponent<UserRecord>().Setrecord(player[0],player[1]);
            recordScreen.transform.parent = _content.transform;
        }
    }

    public void OnMenuButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Start");
    }
}
