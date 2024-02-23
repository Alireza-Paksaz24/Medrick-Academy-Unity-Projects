using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Blocks : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    private static bool _isClicked = false;
    private static string word = "";
    private bool _isSelected = false;
    private static List<Image> _selected = new List<Image>();
    private Image blockImageComponent;
    private char character;

    private void Start()
    {
        blockImageComponent = GetComponent<Image>();
        character = 'a';
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BlockStatusChange(false);
            SelectBlock();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BlockStatusChange(true);
            _isClicked = false;
            foreach (var block in _selected)
            {
                block.color = Color.white;
                block.gameObject.GetComponent<Blocks>().Release();
            }

            _selected = new List<Image>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isClicked && !_isSelected)
        {
            SelectBlock();
        }
    }

    private void SelectBlock()
    {
        blockImageComponent.color = Color.green;
        _selected.Add(blockImageComponent);
        word += character.ToString();
        _isSelected = true;
    }

    public void Release()
    {
        _isSelected = false;
    }

    private static void BlockStatusChange(bool release)
    {
        if (release)
            _isClicked = false;
        else
            _isClicked = true;
    }
}