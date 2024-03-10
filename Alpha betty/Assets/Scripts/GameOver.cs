using UnityEngine;


public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string text = "";
        foreach (var word in StaticValues.words)
        {
            text += word;
            text += "\n";
        }
        text.Substring(0, text.Length - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
