using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullWater : MonoBehaviour
{
    private float amountOfWaterBucketRemoves = 0.75f;

    private void OnTriggerEnter(Collider other)
    {
        InteractionEvent interactionEvent = other.GetComponent<InteractionEvent>();
        if (interactionEvent != null)
        {
            if (interactionEvent.GetPC().GetHeldObject() != null)
            {
                BucketController bucket = interactionEvent.GetPC().GetHeldObject().GetComponent<BucketController>();
                if (bucket != null)
                {
                    if (!bucket.HasWater())
                    {
                        bucket.SetHasWater(true);
                        TestGamemodeController.INSTANCE.EmptyHullWater(amountOfWaterBucketRemoves);
                    }
                }
            }
        }
    }
}
