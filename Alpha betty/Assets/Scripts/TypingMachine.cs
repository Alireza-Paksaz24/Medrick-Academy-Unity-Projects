
using TMPro;
using UnityEngine;

public class TypingMachine : MonoBehaviour
{

    private string _text = "";
    private string _tempWord = "";
    
    [SerializeField] private TextMeshProUGUI _textGameObject;


    public void InputWord(string word)
    {
        RemoveTempWord();
        _text += word;

        _tempWord = word;
        PrintText();
    }

    public void Confirm(bool correct)
    {
        if (correct)
            _text += "\n ";
        else
            RemoveTempWord();
        _tempWord = "";
        PrintText();
    }

    private void RemoveTempWord()
    {
            _text = _text.Substring(0, _text.Length - _tempWord.Length);
    }
    private void PrintText()
    {
        _textGameObject.text = _text;
    }
}
