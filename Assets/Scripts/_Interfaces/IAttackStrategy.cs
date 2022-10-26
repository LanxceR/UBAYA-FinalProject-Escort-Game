using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all attack strategies (melee, ranged, etc) in the game
/// </summary>
public interface IAttackStrategy
{
    void BeginAttack();

    void AttackWithAnim();

    void ExecuteAttack();
}
