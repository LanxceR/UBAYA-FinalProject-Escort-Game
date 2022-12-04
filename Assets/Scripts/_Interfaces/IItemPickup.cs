using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemPickup
{
    void OnPickup(GameObject picker);

    GameObject GetGameObject();
}
