using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _wordText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    private string text = "";
    private const string apiUrlTemplate = "https://api.dictionaryapi.dev/api/v2/entries/en/{0}";

    // Use this for initialization
    void Start()
    {
        _scoreText.text = "Score: " + Convert.ToString(StaticValues.score);
        List<string> words = StaticValues.words;
        StartCoroutine(FetchDefinitions(words));
    }

    IEnumerator FetchDefinitions(List<string> words)
    {
        StringBuilder resultBuilder = new StringBuilder();
        foreach (var word in words)
        {
            string url = string.Format(apiUrlTemplate, word);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // Parse the response
                    string responseText = webRequest.downloadHandler.text;
                    var definitions = ParseDefinition(responseText);
                    resultBuilder.AppendLine($"{word}:\n   {definitions}");
                }
                else
                {
                    Debug.LogError($"Error fetching definition for {word}: {webRequest.error}");
                    resultBuilder.AppendLine($"{word}: Definition not found.");
                }
            }
        }

        Debug.Log(resultBuilder.ToString());
        text += resultBuilder.ToString() + "\n";
        _wordText.text = text;
        
        // Here you can do whatever you want with the result string
        // For example, show it in a UI Text component
    }

    private string ParseDefinition(string jsonResponse)
    {
        // This is a very basic and brittle way to extract a definition.
        // Consider using a JSON parsing library like JsonUtility or Newtonsoft.Json for more robust parsing.
        const string searchPattern = "\"definition\":\"";
        int definitionStartIndex = jsonResponse.IndexOf(searchPattern) + searchPattern.Length;
        if (definitionStartIndex == searchPattern.Length - 1) return "No definition found.";
        int definitionEndIndex = jsonResponse.IndexOf("\"", definitionStartIndex);
        string definition = jsonResponse.Substring(definitionStartIndex, definitionEndIndex - definitionStartIndex);
        return definition;
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Scenes/Game");
        StaticValues.words = new List<string>();
        StaticValues.score = 0;
    }
}
