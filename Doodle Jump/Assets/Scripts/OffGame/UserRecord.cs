using TMPro;
using UnityEngine;

public class UserRecord : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _username, _score;
    public void Setrecord(string username, string score)
    {
        _username.text = username;
        _score.text = score;
    }
}