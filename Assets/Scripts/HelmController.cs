using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmController : MonoBehaviour
{
    [SerializeField] private GameObject wheel;
    [SerializeField] private float wheelSpinSpeed;
    [SerializeField] private float steeringMaxTurnSpeed;
    [SerializeField] private float steeringTurnAcceleration;
    [SerializeField] private GameObject oceanRenderObject;
    [SerializeField] private float oceanHorizontalScrollSpeed;
    private float steeringTurnSpeed;
    private float oceanHorizontalOffset;
    [SerializeField] private IslandHazardController islandHazardController;
    private StationController stationController;
    private int playerID;
    private bool isInUse = false;

    private void Start()
    {
        stationController = GetComponent<StationController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInUse) return;

        InteractionEvent interactionEvent = other.GetComponent<InteractionEvent>();
        if (interactionEvent != null)
        {
            if (interactionEvent.GetAction() != InteractionEvent.Action.Interact) return;

            if (interactionEvent.GetPC().GetHeldObject() == null)
            {
                playerID = interactionEvent.GetPC().GetPlayerId();
                isInUse = true;
                stationController.TakeControlFromPlayer(interactionEvent.GetPC());
            }
        }
    }

    private void Update()
    {
        GetCancel();
        Steer();
    }

    private void Steer()
    {
        // Get horizontal Input
        float horizontal;
        if (isInUse)
        {
            horizontal = Input.GetAxisRaw("Walk_H_P" + playerID);
        }
        else
        {
            horizontal = 0f;
        }

        // Slow down to a stop
        if (horizontal == 0)
        {
            if (steeringTurnSpeed > 0)
            {
                steeringTurnSpeed -= steeringTurnAcceleration * Time.deltaTime;
                if (steeringTurnSpeed < 0)
                {
                    steeringTurnSpeed = 0;
                }
            }
            else if (steeringTurnSpeed < 0)
            {
                steeringTurnSpeed += steeringTurnAcceleration * Time.deltaTime;
                if (steeringTurnSpeed > 0)
                {
                    steeringTurnSpeed = 0;
                }
            }
        }
        else
        {
            steeringTurnSpeed += -horizontal * steeringTurnAcceleration * Time.deltaTime;
        }

        // Limit maximum speed
        if (steeringTurnSpeed > steeringMaxTurnSpeed)
        {
            steeringTurnSpeed = steeringMaxTurnSpeed;
        }
        if (steeringTurnSpeed < -steeringMaxTurnSpeed)
        {
            steeringTurnSpeed = -steeringMaxTurnSpeed;
        }

        // Apply speed to helm, islands, and ocean
        wheel.transform.Rotate(new Vector3(0f, 0f, -horizontal * wheelSpinSpeed));
        islandHazardController.Move(steeringTurnSpeed);
        oceanHorizontalOffset += steeringTurnSpeed / 10000000 * oceanHorizontalScrollSpeed;
        oceanRenderObject.GetComponent<Renderer>().sharedMaterial.SetVector("_ManualOffset", new Vector4(oceanHorizontalOffset, 0f, 0f, 0f));
    }

    private void GetCancel()
    {
        if (isInUse)
        {
            if (!Input.GetButton("Button_Left_P" + playerID) && !Input.GetButton("Button_Bottom_P" + playerID))
            {
                stationController.ReturnControlToPlayer();
                isInUse = false;
            }
        }
    }
}