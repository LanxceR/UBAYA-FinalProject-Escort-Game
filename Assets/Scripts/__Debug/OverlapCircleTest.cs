using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 10f);
        Debug.Log(cols);
    }
}
