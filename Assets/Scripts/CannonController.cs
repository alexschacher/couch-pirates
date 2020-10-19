using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private GameObject reticleAim;
    [SerializeField] private GameObject reticleVisual;
    [SerializeField] private GameObject reticleOrigin;
    [SerializeField] private GameObject cannonballPrefab;
    [SerializeField] private GameObject cannonballOrigin;
    [SerializeField] private GameObject cannonballBarrelPivot;
    [SerializeField] private GameObject fuse;
    [SerializeField] private Animator barrelAnimator;
    [SerializeField] private Animator cannonBallAnimator;
    [SerializeField] private LineRenderer lineRenderer;
    private Quaternion barrelPivotStartingRotation;
    private float parabolaHeight = 1f;
    private HoldableObject cannonballBeingLoaded;
    private StationController stationController;
    private float reticleRange = 2f;
    private float reticleSpeed = 0.02f;
    private int playerID;
    private Vector3 movementDirection;
    private bool isInUse = false; // Someone is already equipping the cannon
    private bool loaded = false; // There is a cannonball in the cannon
    private bool isLoading = false;
    private bool readyToFire = false; // It is ready to fire, no longer in the loading animation
    private bool rightTriggerHeld = false;

    private void Start()
    {
        stationController = GetComponent<StationController>();
        barrelPivotStartingRotation = cannonballBarrelPivot.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInUse) return;

        InteractionEvent interactionEvent = other.GetComponent<InteractionEvent>();

        if (interactionEvent != null)
        {
            if (interactionEvent.GetAction() != InteractionEvent.Action.Interact) return;

            HoldableObject heldObject = interactionEvent.GetPC().GetHeldObject();

            // Equip cannon if it is loaded and player is not holding something
            if (heldObject == null)
            {
                BecomeEquippedByPlayer(interactionEvent.GetPC());
            }

            // Begin loading the cannon
            if (heldObject != null && !loaded)
            {
                if (heldObject.name == "Cannonball Item(Clone)" && !loaded)
                {
                    cannonballBeingLoaded = heldObject;
                    StartLoadingAnimation();
                    isLoading = true;
                    BecomeEquippedByPlayer(interactionEvent.GetPC());
                    SetReticleEnabled(false);
                }
            }
        }
    }

    void Update()
    {
        SetParabolaHeight();

        if (reticleAim.activeSelf)
        {
            DisplayParabolaTrajectory();
            MoveReticleToCollisionPoint();
        }

        if (!barrelAnimator.enabled)
        {
            SetCannonbarrelRotation();
        }

        if (!isInUse) return;
        CheckInputShoot();
        CheckInputCancel();
        MoveReticle();
    }

    private void SetParabolaHeight()
    {
        parabolaHeight = 4.25f - ((cannonballOrigin.transform.position - reticleAim.transform.position).magnitude / 2);
    }

    private void DisplayParabolaTrajectory()
    {
        Vector3[] linePoints = new Vector3[11];

        for (int i = 0; i <= 10; i++)
        {
            linePoints[i] = MathParabola.Parabola(
                cannonballOrigin.transform.position,
                reticleAim.transform.position,
                parabolaHeight,
                i / 10f);
        }

        lineRenderer.SetPositions(linePoints);
    }

    private void MoveReticleToCollisionPoint()
    {
        RaycastHit raycastHit;
        Vector3 pointA, pointB;
        LayerMask layerMask = LayerMask.GetMask("EnemyHitbox", "OceanHitbox");

        for (int i = 0; i < 13; i++)
        {
            pointA = MathParabola.Parabola(
                cannonballOrigin.transform.position,
                reticleAim.transform.position,
                parabolaHeight,
                i / 10f);

            pointB = MathParabola.Parabola(
                cannonballOrigin.transform.position,
                reticleAim.transform.position,
                parabolaHeight,
                (i + 1) / 10f);

            if (Physics.Raycast(
                pointA,
                (pointB - pointA).normalized,
                out raycastHit,
                (pointA - pointB).magnitude,
                layerMask))
            {
                reticleVisual.transform.position = raycastHit.point;
                reticleVisual.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
                Debug.DrawLine(pointA, pointB, Color.red);
                break;
            }
        }
    }

    private void SetCannonbarrelRotation()
    {
        cannonballBarrelPivot.transform.LookAt(MathParabola.Parabola(
                cannonballOrigin.transform.position,
                reticleAim.transform.position,
                parabolaHeight,
                0.4f));
    }

    private void CheckInputShoot()
    {
        if (Input.GetAxisRaw("Trigger_Right_P" + playerID) < 0.2f && rightTriggerHeld)
        {
            rightTriggerHeld = false;
        }

        if (!loaded || !readyToFire) return;

        if (Input.GetAxisRaw("Trigger_Right_P" + playerID) > 0.7f && !rightTriggerHeld)
        {
            ShootCannonball(reticleAim.transform.position, true);
            SetLoaded(false);
            SetReadyToFire(false);
            rightTriggerHeld = true;
            //BecomeUnequipped();
        }
    }

    private void CheckInputCancel()
    {
        if (!isLoading)
        {
            // If no longer holding aim button
            if (!Input.GetButton("Button_Bottom_P" + playerID) && !Input.GetButton("Button_Left_P" + playerID))
            {
                BecomeUnequipped();
            }
        }
        else
        {
            // If no longer holding a loading button while loading
            if (!Input.GetButton("Button_Bottom_P" + playerID) && !Input.GetButton("Button_Left_P" + playerID))
            {
                CancelLoadingAnimation();
                BecomeUnequipped();
            }
        }
    }

    public void ShootCannonball(Vector3 targetPosition, bool isPlayerShot)
    {
        SetReticleEnabled(false);
        GameObject cannonball = Instantiate(cannonballPrefab, cannonballOrigin.transform.position, Quaternion.identity);
        cannonball.GetComponent<Cannonball>().Init(
            cannonballOrigin.transform.position,
            targetPosition,
            parabolaHeight, isPlayerShot);
    }

    private void MoveReticle()
    {
        // Get Input
        float horizontal = Input.GetAxisRaw("Walk_H_P" + playerID);
        float vertical = -Input.GetAxis("Walk_V_P" + playerID);
        movementDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // If reticle is within a certain radius, move towards that direction
        Vector3 targetReticlePosition = reticleAim.transform.position + (movementDirection * reticleSpeed);
        Vector3 distanceFromOrigin = targetReticlePosition - reticleOrigin.transform.position;
        if (distanceFromOrigin.magnitude < reticleRange)
        {
            reticleAim.transform.position = targetReticlePosition;
        }
        else
        {
            reticleAim.transform.position = reticleOrigin.transform.position + (targetReticlePosition - reticleOrigin.transform.position).normalized * reticleRange;
        }
    }

    private void BecomeEquippedByPlayer(PlayerController pc)
    {
        playerID = pc.GetPlayerId();
        isInUse = true;
        stationController.TakeControlFromPlayer(pc);
        SetReticleEnabled(readyToFire);
    }

    private void BecomeUnequipped()
    {
        stationController.ReturnControlToPlayer();
        SetReticleEnabled(false);
        isInUse = false;
    }

    private void SetLoaded(bool toLoad)
    {
        if (toLoad == true)
        {
            if (cannonballBeingLoaded != null)
            {
                Destroy(cannonballBeingLoaded.gameObject);
            }
            else
            {
                return;
            }
        }
        loaded = toLoad;
        fuse.SetActive(loaded);
    }

    private void SetReadyToFire(bool input)
    {
        readyToFire = input;
        if (readyToFire && isInUse)
        {
            SetReticleEnabled(true);
        }
    }

    private void SetReticleEnabled(bool input)
    {
        reticleAim.SetActive(input);
        //reticleVisual.SetActive(input);
        //lineRenderer.enabled = input;
    }

    public void Event_AnimationHitsLoaded()
    {
        if (!isInUse) return;

        SetLoaded(true);
        isLoading = false;

        if (Input.GetAxisRaw("Trigger_Right_P" + playerID) < 0.1f)
        {
            //BecomeUnequipped();
        }
    }

    public void Event_LoadingAnimationComplete()
    {
        SetReadyToFire(true);
        CancelLoadingAnimation();
    }

    private void StartLoadingAnimation()
    {
        cannonballBarrelPivot.transform.rotation = barrelPivotStartingRotation;
        reticleAim.transform.position = reticleOrigin.transform.position;

        cannonBallAnimator.enabled = true;
        barrelAnimator.enabled = true;
        barrelAnimator.Play("CannonBarrelLoad", 0, 0f);
        cannonBallAnimator.Play("CannonballLoad", 0, 0f);
    }

    private void CancelLoadingAnimation()
    {
        cannonballBarrelPivot.transform.rotation = barrelPivotStartingRotation;
        reticleAim.transform.position = reticleOrigin.transform.position;

        barrelAnimator.StopPlayback();
        barrelAnimator.enabled = false;
        cannonBallAnimator.StopPlayback();
        cannonBallAnimator.enabled = false;
    }
}