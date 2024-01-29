using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Store : MonoBehaviour
{

    [SerializeField] private GameObject[] _storeMarket;
    [SerializeField] private Text _balance;

    private ArrayList skins = new ArrayList();
    private static readonly string[] ALL_VALUE = new string[] {"normal","blue","bunny","Astronaut","Ghost","Indiana","Snow","Soccer"};

    // Start is called before the first frame update
    void Start()
    {
        _balance.text = PlayerPrefs.GetInt(StaticValue.username,0).ToString();
        var store = PlayerPrefs.GetString("store", "normal");
        var tempSkins = store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var i in tempSkins)
        {
            skins.Add(i);
        }
        changeColor();

    }

    public void OnReturn()
    {
        SceneManager.LoadScene("Scenes/Start");
    }

    public void OnPressStore(int index)
    {
        GameObject choose = GameObject.Find(ALL_VALUE[index]);
        var chooseTransform = choose.transform;
        int value = Convert.ToInt16(chooseTransform.GetChild(0).GetComponent<Text>().text);
        if (skins.Contains(choose.name))
        {
            Debug.Log("1");
            StaticValue.choosenSprite = index;
            changeColor();
        }
        else if (value <= StaticValue.playerBalance)
        {
            Debug.Log("2");
            StaticValue.playerBalance -= value;
            PlayerPrefs.SetInt(StaticValue.username,StaticValue.playerBalance);
            StaticValue.choosenSprite = index;
            skins.Add(choose.name);
            string skinsString = string.Join(",", skins.Cast<string>().ToArray());
            PlayerPrefs.SetString("store", skinsString);
            changeColor();
            _balance.text = PlayerPrefs.GetInt(StaticValue.username,0).ToString();
            
        }
    }

    void changeColor()
    {
        var i = 0;
        foreach (var skin in _storeMarket)
        {
            if (skins.Contains(skin.gameObject.name))
            {
                skin.GetComponent<Image>().color = Color.grey;
            }
            if (i == StaticValue.choosenSprite)
                skin.GetComponent<Image>().color = Color.green;
            i++;
        }
    }
}
