using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all animations in the game
/// </summary>
public interface IAnimation
{
    void ChangeAnimationState(string newState);

    IEnumerator ChangeAnimationStateUninterruptible(string newState, bool forceStart, bool stopAfterAnimEnd);
}
