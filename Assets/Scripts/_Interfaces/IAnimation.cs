using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all animations in the game
/// </summary>
public interface IAnimation
{
    bool ChangeAnimationState(string newState, bool forceStart);

    IEnumerator ChangeAnimationStateUninterruptible(string newState, bool forceStart, bool stopAfterAnimEnd);
}
