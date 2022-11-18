using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaragePurchaseScript : MonoBehaviour
{
    public GameObject UI;

    public void Purchase()
    {
        UI.SetActive(true);
    }

    public void ConfirmPurchase()
    {

    }

    public void CancelPurchase()
    {
        UI.SetActive(false);
    }
}
