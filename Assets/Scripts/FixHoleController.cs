using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHoleController : MonoBehaviour
{
    private float fixProgress;
    [SerializeField] private float fixSpeed;
    [SerializeField] private GameObject hole;
    [SerializeField] private GameObject progressBarGreen;
    [SerializeField] private GameObject progressBarRed;
    [SerializeField] private float waterFillSpeed;

    private void OnTriggerEnter(Collider other)
    {
        InteractionEvent interactionEvent = other.GetComponent<InteractionEvent>();
        if (interactionEvent != null)
        {
            if (interactionEvent.GetAction() != InteractionEvent.Action.Interact) return;

            if (interactionEvent.GetPC().GetHeldObject() == null)
            {
                fixProgress += fixSpeed;
                progressBarGreen.SetActive(true);
                progressBarRed.SetActive(true);
                progressBarGreen.transform.localScale = new Vector3(fixProgress, 1f, 1f);
                if (fixProgress > 1)
                {
                    Destroy(hole);
                }
            }
        }
    }

    private void Update()
    {
        TestGamemodeController.INSTANCE.FillHullWater(waterFillSpeed * Time.deltaTime);
    }
}