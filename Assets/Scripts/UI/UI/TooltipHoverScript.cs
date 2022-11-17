using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipHoverScript : MonoBehaviour 
{
    public string content;

    private void Start()
    {
        TooltipSystem.Hide();
    }

    void OnMouseEnter()
    {
        TooltipSystem.Show(content);
    }

    void OnMouseExit()
    {
        TooltipSystem.Hide();
    }
}
