using TMPro;
using UnityEngine;

public class UserRecord : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _username, _score;
    public void Setrecord(object username, object score)
    {
        _username.text = username.ToString();
        _score.text = score.ToString();
    }
}