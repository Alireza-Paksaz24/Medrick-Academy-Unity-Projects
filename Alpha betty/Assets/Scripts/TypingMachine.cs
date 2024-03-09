
using TMPro;
using UnityEngine;

public class TypingMachine : MonoBehaviour
{

    private string _leftText = "";
    private string _rightText = "";
    private string _tempWord = "";
    private bool _isLeft = true;
    
    [SerializeField] private TextMeshProUGUI _leftTextGameObject;
    [SerializeField] private TextMeshProUGUI _rightTextGameObject;


    public void InputWord(string word)
    {
        if (_isLeft)
        {
            _leftText += word;
        }
        else
            _rightText += word;

        PrintText();
        
    }
    
    private void PrintText()
    {
        _leftTextGameObject.text = _leftText;
        _rightTextGameObject.text = _rightText;
    }
}
