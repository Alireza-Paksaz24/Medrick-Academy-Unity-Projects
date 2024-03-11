using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Blocks : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    private SoundEffectControl _soundEffectControl;
    private BlcoksManager _blockManager;
    private int[] _posi = new int[2];
    private int[] _destPosi = new int[2];
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
    
    private void Start()
    {
        _soundEffectControl = GameObject.Find("Sound Manager").GetComponent<SoundEffectControl>();
        _blockManager = this.transform.parent.GetComponent<BlcoksManager>();
        blockImageComponent = GetComponent<Image>();
        character = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text[0];
    }

    public void SetPosi(int x, int y)
    {
        _posi[0] = x;
        _posi[1] = y;
        SetDestPosi(_posi[0],_posi[1]);
    }
    public void SetDestPosi(int x, int y)
    {
        _destPosi[0] = x;
        _destPosi[1] = y;
    }
    public int[] GetDestPosi()
    {
        return _destPosi;
    }
    public int[] GetPosi()
    {
        return _posi;
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
            if (!_blockManager.CheckWord(word))
            {
                foreach (var block in _selected)
                {
                    block.GetComponent<Blocks>().ShakeAnimation(0);
                }
            }
            foreach (var block in _selected)
            {
                block.GetComponent<Blocks>().Release();
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
        if (_currentBlock == _nextBlock && _currentBlock != null)
        {
            _selected.Remove(_currentBlock);
            _currentBlock.GetComponent<Blocks>().Release();
            _currentBlock = this.gameObject;
            CheckWord(word);
            _blockManager.TypeInMachine(word);
        }
        if (_isClicked && !_isSelected)
        {
            _soundEffectControl.OnTyping();
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
        blockImageComponent.color = Color.red;
        _selected.Add(this.gameObject);
        word += character.ToString();
        CheckWord(word);
        _isSelected = true;
        _blockManager.TypeInMachine(word);
    }

    private void CheckWord(string word)
    {
        Color color = Color.red;;
        if (_blockManager.SearchWord(word))
            color = Color.green;
        foreach (var block in _selected)
        {
            block.GetComponent<Image>().color = color;
        }
        
    }

    public void Release()
    {
        _nextBlock = null;
        _previuseBlock = null;
        this.GetComponent<Image>().color = new Color(0.459f,0.412f,0.43f);
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

    public List<GameObject> GetSelected()
    {
        return _selected;
    }

    public void MoveBlock(Vector2 newPosition)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        float time = (rectTransform.anchoredPosition.y - newPosition.y)/306;
        rectTransform.DOAnchorPos(newPosition, time).SetEase(Ease.InOutSine);
    }

    public void ShakeAnimation(int level)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        int range = 0;
        switch (level)
        {
            case 0:
                range = 5;
                break;
            case 1:
                range = -10;
                break;
            case 2:
                range = 5;
                break;
            case 3:
                return;
        }
        rectTransform.DOAnchorPos(rectTransform.anchoredPosition + new Vector2(range,0), 0.1f).SetEase(Ease.InOutSine).OnComplete(() => ShakeAnimation(level+1));
    }
}