using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAnimationEvents : MonoBehaviour
{
    [SerializeField] private CannonController cannonController;

    public void Event_AnimationHitsLoaded()
    {
        cannonController.Event_AnimationHitsLoaded();
    }

    public void Event_LoadingAnimationComplete()
    {
        cannonController.Event_LoadingAnimationComplete();
    }
}
