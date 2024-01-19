using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Border : MonoBehaviour
{
    private bool _flag = false;
    
    // Start is called before the first frame update
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }else if (Time.time > 0.5f)
        {
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>().GameEnded();
            GetComponent<Player>().NoGravity();
        }

        Debug.Log(other.name);
    }
}
