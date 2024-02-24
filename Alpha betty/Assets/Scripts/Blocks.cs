using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Blocks : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    private BlcoksManager _blockManager;
    
    private static bool _isClicked = false;
    private GameObject _nextBlock = null;
    private GameObject _previuseBlock = null;
    private static GameObject _currentBlock = null;
    private static string word = "";
    private static List<GameObject> _selected = new List<GameObject>();
    
    private bool _isSelected = false;
    
    private Image blockImageComponent;
    private char character;
    private bool _destroy = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.gameObject == _currentBlock)
        {
            // string temp = "";
            // foreach (var i in _selected)
            // {
            //     temp += i.name;
            // }
            // Debug.Log(temp);
            // Debug.Log("Current Block = "+_currentBlock);
            // Debug.Log("Previuse Block = "+_previuseBlock);
            Debug.Log(word);
        }
    }
    
    private void Start()
    {
        _blockManager = this.transform.parent.GetComponent<BlcoksManager>();
        blockImageComponent = GetComponent<Image>();
        character = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text[0];
    }

    public void SetNextBlock(GameObject block)
    {
        _nextBlock = block;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BlockStatusChange(true);
            _isClicked = false;
            if (_blockManager.IsCorrect(word))
            {
                Debug.Log("Yeah");
                _destroy = true;
            }else
                Debug.Log("No way");
            foreach (var block in _selected)
            {
                block.GetComponent<Blocks>().Release();
                if (_destroy)
                    Destroy(block);
            }
            _selected.Clear();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BlockStatusChange(false);
            SelectBlock();
            _currentBlock = this.gameObject;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log(word);
        if (_currentBlock == _nextBlock && _currentBlock != null)
        {
            _selected.Remove(_currentBlock);
            _currentBlock.GetComponent<Blocks>().Release();
            _currentBlock = this.gameObject;
        }
        if (_isClicked && !_isSelected)
        {
            SelectBlock();
            if (_currentBlock != null)
            {
                _currentBlock.GetComponent<Blocks>().SetNextBlock(this.gameObject);
                _previuseBlock = _currentBlock;
            }
            _currentBlock = this.gameObject;
        }
    }
    
    private void SelectBlock()
    {
        blockImageComponent.color = Color.green;
        _selected.Add(this.gameObject);
        word += character.ToString();
        _isSelected = true;
    }

    public void Release()
    {
        _nextBlock = null;
        _previuseBlock = null;
        this.GetComponent<Image>().color = Color.white;
        _isSelected = false;
        string temp = word.Substring(0, word.Length - 1);
        word = temp;
    }

    private static void BlockStatusChange(bool release)
    {
        if (release)
            _isClicked = false;
        else
            _isClicked = true;
    }

    public void SetChar(string character)
    {
        this.character = character[0];
        transform.GetChild(0).GetComponent<TMP_Text>().text = character;
    }

}