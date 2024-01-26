using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject _userRecordPanel;

    [SerializeField]
    private GameObject _content;
    void Start()
    {
        var arrayOfTopPlayer = (string[]) StaticValue.topPlayer[0];
        foreach (var player in arrayOfTopPlayer)
        {
            var recordScreen = Instantiate(_userRecordPanel);
            recordScreen.GetComponent<UserRecord>().Setrecord(player[0],player[1]);
            recordScreen.transform.parent = _content.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
