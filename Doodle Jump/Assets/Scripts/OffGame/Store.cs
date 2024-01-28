using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Store : MonoBehaviour
{

    [SerializeField] private Text _balance;
    // Start is called before the first frame update
    void Start()
    {
        _balance.text = PlayerPrefs.GetInt(StaticValue.username,0).ToString();
    }

    public void OnReturn()
    {
        SceneManager.LoadScene("Scenes/Start");
    }
}
