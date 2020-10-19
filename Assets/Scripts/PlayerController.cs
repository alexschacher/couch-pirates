using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int playerID = 1;
    [SerializeField] private GameObject interactionEventPrefab;
    private float speed = 175f;
    private float turnSmoothTime = 0.04f;
    private float turnSmoothVelocity;
    private Rigidbody rb;
    private Vector3 movementDirection;
    private bool hasControl = true;
    private HoldableObject heldObject;
    private float justDroppedTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        DetermineMovementDirection();
        DetectInteractionInput();
        if (justDroppedTimer > 0)
        {
            justDroppedTimer -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void DetermineMovementDirection()
    {
        // Get Input
        float horizontal = Input.GetAxisRaw("Walk_H_P" + playerID);
        float vertical = -Input.GetAxis("Walk_V_P" + playerID);
        movementDirection = new Vector3(horizontal, 0f, vertical).normalized;
    }
    private void ApplyMovement()
    {
        // If moving
        if (movementDirection.magnitude >= 0.1f && hasControl == true)
        {
            // Change rotation smoothly
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move the player
            rb.velocity = new Vector3(
                movementDirection.x * speed * Time.deltaTime,
                rb.velocity.y,
                movementDirection.z * speed * Time.deltaTime);
        }
        // If stopped
        else
        {
            rb.velocity = new Vector3(
                0f,
                rb.velocity.y,
                0f);
        }
    }

    private void DetectInteractionInput()
    {
        if (hasControl)
        {
            if (Input.GetButtonDown("Button_Bottom_P" + playerID) || Input.GetButtonDown("Button_Left_P" + playerID))
            {
                GameObject intEvent = Instantiate(interactionEventPrefab, transform.position, Quaternion.identity);
                intEvent.GetComponent<InteractionEvent>().Init(this, InteractionEvent.Action.Interact);
            }

            if (Input.GetButtonDown("Button_Top_P" + playerID))
            {
                GameObject intEvent = Instantiate(interactionEventPrefab, transform.position, Quaternion.identity);

                if (heldObject == null && justDroppedTimer <= 0)
                {
                    intEvent.GetComponent<InteractionEvent>().Init(this, InteractionEvent.Action.Grab);
                    Debug.Log("Grab");
                }
                else
                {
                    intEvent.GetComponent<InteractionEvent>().Init(this, InteractionEvent.Action.Drop);
                    Debug.Log("Drop");
                }
            }
        }
    }

    public bool GetHasControl()
    {
        return hasControl;
    }
    public void SetHasControl(bool toHaveControl)
    {
        if (toHaveControl)
        {
            hasControl = true;
            UnFreeze();
        }
        else
        {
            hasControl = false;
            Freeze();
        }
        
    }
    private void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void UnFreeze()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public int GetPlayerId()
    {
        return playerID;
    }

    public HoldableObject GetHeldObject()
    {
        return heldObject;
    }
    public void SetHeldObject(HoldableObject obj)
    {
        heldObject = obj;
    }
    public void RemoveHeldObject()
    {
        heldObject = null;
        justDroppedTimer = 0.1f;
    }
}