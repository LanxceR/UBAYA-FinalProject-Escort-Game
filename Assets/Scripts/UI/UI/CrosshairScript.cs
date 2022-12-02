using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    //[SerializeField]
    //private Camera mainCamera;

    private Camera UICamera;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;

        foreach (Camera c in Camera.allCameras)
        {
            if (c.gameObject.name.Contains("UI"))
            {
                UICamera = c;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 mouseCursorPos = Camera.allCameras
        //transform.position = mouseCursorPos;
        //Cursor.visible = false;
        Vector3 mouseCursorPos = UICamera.ScreenToWorldPoint(Input.mousePosition);
        mouseCursorPos.z = 0f;
        transform.position = mouseCursorPos;
    }
}
