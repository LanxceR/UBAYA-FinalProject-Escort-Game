using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnywhere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("Button has been pressed");
        }
    }
}
