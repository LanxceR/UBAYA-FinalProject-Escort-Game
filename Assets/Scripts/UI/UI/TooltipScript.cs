using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class TooltipScript : MonoBehaviour
{
    public TextMeshProUGUI contentField;

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Input.mousePosition;
        transform.position = position;
    }

    public void SetText(string content)
    {
        contentField.text = content;
    }
}
