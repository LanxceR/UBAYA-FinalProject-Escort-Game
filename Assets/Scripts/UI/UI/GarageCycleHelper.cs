using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageCycleHelper : MonoBehaviour
{
    // Start is called before the first frame update
    public void CycleLeft()
    {
        this.GetComponentInParent<GarageConvoyInfo>().CycleLeft();
    }

    // Update is called once per frame
    public void CycleRight()
    {
        this.GetComponentInParent<GarageConvoyInfo>().CycleRight();
    }
}
