using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmController : MonoBehaviour
{
    [SerializeField] private GameObject wheel;
    [SerializeField] private float wheelSpinSpeed;
    [SerializeField] private float steeringMaxTurnSpeed;
    [SerializeField] private float steeringTurnAcceleration;
    private float steeringTurnSpeed;
    [SerializeField] private OceanPivotController oceanPivotController;
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
        float horizontal;
        if (isInUse)
        {
            horizontal = Input.GetAxisRaw("Walk_H_P" + playerID);
        }
        else
        {
            horizontal = 0f;
        }

        wheel.transform.Rotate(new Vector3(0f, 0f, -horizontal * wheelSpinSpeed));

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

        if (steeringTurnSpeed > steeringMaxTurnSpeed)
        {
            steeringTurnSpeed = steeringMaxTurnSpeed;
        }
        if (steeringTurnSpeed < -steeringMaxTurnSpeed)
        {
            steeringTurnSpeed = -steeringMaxTurnSpeed;
        }

        oceanPivotController.SetRotationAngle(steeringTurnSpeed * Time.deltaTime);
    }

    private void GetCancel()
    {
        if (isInUse)
        {
            if (!Input.GetButton("Button_Left_P" + playerID) && !Input.GetButton("Button_Bottom_P" + playerID))
            {
                stationController.ReturnControlToPlayer();
                oceanPivotController.SetRotationAngle(0f);
                isInUse = false;
            }
        }
        }
}