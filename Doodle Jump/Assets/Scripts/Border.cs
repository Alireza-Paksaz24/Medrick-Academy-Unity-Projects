using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Border : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
