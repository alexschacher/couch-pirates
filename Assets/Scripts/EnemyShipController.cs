using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    [InspectorButton("TakeDamage")] [SerializeField] private bool takeDamageButton;
    [InspectorButton("ShootCannon")] [SerializeField] private bool shootCannonButton;

    [SerializeField] private Transform cannonTarget;
    [SerializeField] private List<CannonController> leftCannons;
    [SerializeField] private List<CannonController> rightCannons;
    [SerializeField] private float health;
    private float startingHealth;
    private float currentSinkDepth;
    private float startingSinkDepth;
    [SerializeField] private float hitSinkSpeed;
    [SerializeField] private float sinkDownSpeed;
    [SerializeField] private float sinkFallbackSpeed;
    [SerializeField] private float sinkTimeTilDestroy;
    private float sinkTimer;
    [SerializeField] private float maxSinkDepth;
    [SerializeField] private float secondsBetweenShots;
    [SerializeField] private float secondsBetweenShotsVariation;
    private float randomTimeBetweenShots;
    private float timeBetweenShotsTimer;
    private bool onRightSide;
    private float initTime;
    private float journeyLength;
    private bool isInPosition = false;
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private Vector2 driftSpeed;
    [SerializeField] private Vector2 driftMaxSpeed;
    private Vector2 driftAcceleration;
    [SerializeField] private Vector2 driftMaxAcceleration;
    [SerializeField] private Vector2 driftAccelerationSpeed;
    [SerializeField] private Vector2 driftMaxDistance;
    private Vector2 driftCurrentAccelerationDirection;
    [SerializeField] private Vector2 driftAccelerationFlipTimeRange;
    private Vector2 driftAccelerationFlipTimer;

    private void Start()
    {
        driftCurrentAccelerationDirection.x = 1f;
        driftCurrentAccelerationDirection.y = 1f;
        startingHealth = health;
        startingSinkDepth = transform.position.y;
        currentSinkDepth = startingSinkDepth;
        randomTimeBetweenShots = Random.Range(secondsBetweenShots - secondsBetweenShotsVariation, secondsBetweenShots + secondsBetweenShotsVariation);
    }

    private void OnTriggerEnter(Collider other)
    {
        Cannonball cannonball = other.gameObject.GetComponent<Cannonball>();
        if (cannonball != null)
        {
            cannonball.Crash();
            TakeDamage();
            return;
        }
    }

    private void Update()
    {
        if (isInPosition)
        {
            Sink();

            if (health > 0)
            {
                Drift();
                ShootCannonsOverTime();
            }
        }
        else
        {
            MoveIntoPosition();
            Drift();
        }
    }
  
    private void Drift()
    {
        // Flip direction timer
        driftAccelerationFlipTimer.x -= Time.deltaTime;
        driftAccelerationFlipTimer.y -= Time.deltaTime;

        if (driftAccelerationFlipTimer.x <= 0)
        {
            driftCurrentAccelerationDirection.x = -driftCurrentAccelerationDirection.x;
            driftAccelerationFlipTimer.x = Random.Range(driftAccelerationFlipTimeRange.x, driftAccelerationFlipTimeRange.y);
        }
        if (driftAccelerationFlipTimer.y <= 0)
        {
            driftCurrentAccelerationDirection.y = -driftCurrentAccelerationDirection.y;
            driftAccelerationFlipTimer.y = Random.Range(driftAccelerationFlipTimeRange.x, driftAccelerationFlipTimeRange.y);
        }


        // if out of drift area (X)
        if (Mathf.Abs(transform.position.x - targetPosition.x) > driftMaxDistance.x)
        {
            // if too far right
            if (transform.position.x > targetPosition.x)
            {
                // acceleration tries to make speed to go left
                driftAcceleration.x -= driftAccelerationSpeed.x * Time.deltaTime;
            }
            else // if too far left
            {
                // acceleration tries to make speed to go right
                driftAcceleration.x += driftAccelerationSpeed.x * Time.deltaTime;
            }
        }
        else // if within drift area (X)
        {
            // acceleration does what it wants to do
            driftAcceleration.x += driftCurrentAccelerationDirection.x * driftAccelerationSpeed.x * Time.deltaTime;
        }



        // if out of drift area (y)
        if (Mathf.Abs(transform.position.z - targetPosition.z) > driftMaxDistance.y)
        {
            // if too far right
            if (transform.position.z > targetPosition.z)
            {
                // acceleration tries to make speed to go left
                driftAcceleration.y -= driftAccelerationSpeed.y * Time.deltaTime;
            }
            else // if too far left
            {
                // acceleration tries to make speed to go right
                driftAcceleration.y += driftAccelerationSpeed.y * Time.deltaTime;
            }
        }
        else // if within drift area (y)
        {
            // acceleration does what it wants to do
            driftAcceleration.y += driftCurrentAccelerationDirection.y * driftAccelerationSpeed.y * Time.deltaTime;
        }


        // Clamp and apply
        driftAcceleration.x = Mathf.Clamp(driftAcceleration.x, -driftMaxAcceleration.x, driftMaxAcceleration.x);
        driftAcceleration.y = Mathf.Clamp(driftAcceleration.y, -driftMaxAcceleration.y, driftMaxAcceleration.y);

        driftSpeed += driftAcceleration * Time.deltaTime;

        driftSpeed.x = Mathf.Clamp(driftSpeed.x, -driftMaxSpeed.x, driftMaxSpeed.x);
        driftSpeed.y = Mathf.Clamp(driftSpeed.y, -driftMaxSpeed.y, driftMaxSpeed.y);

        transform.position += new Vector3(driftSpeed.x, 0f, driftSpeed.y) * Time.deltaTime;
    }

    private void Sink()
    {
        if (health <= 0)
        {
            transform.Translate(0f, -sinkDownSpeed * Time.deltaTime, -sinkFallbackSpeed * Time.deltaTime);
            sinkTimer += Time.deltaTime;

            if (sinkTimer > sinkTimeTilDestroy)
            {
                //TestGamemodeController.INSTANCE.SpawnEnemyShip();
                Destroy(this.gameObject);
            }
        }
        else if (transform.position.y > currentSinkDepth)
        {
            transform.Translate(0f, -hitSinkSpeed * Time.deltaTime, 0f);
        }
    }

    private void ShootCannonsOverTime()
    {
        timeBetweenShotsTimer += Time.deltaTime;
        if (timeBetweenShotsTimer > randomTimeBetweenShots)
        {
            randomTimeBetweenShots = randomTimeBetweenShots = Random.Range(secondsBetweenShots - secondsBetweenShotsVariation, secondsBetweenShots + secondsBetweenShotsVariation);
            timeBetweenShotsTimer = 0f;
            ShootCannon();
        }
    }

    private void ShootCannon()
    {
        if (onRightSide)
        {
            int index = Random.Range(0, leftCannons.Count);
            leftCannons[index].ShootCannonball(cannonTarget.position, false);
        }
        else
        {
            int index = Random.Range(0, rightCannons.Count);
            rightCannons[index].ShootCannonball(cannonTarget.position, false);
        }
    }

    private void TakeDamage()
    {
        health -= 1;
        currentSinkDepth = startingSinkDepth - maxSinkDepth + (maxSinkDepth * health / startingHealth);

        if (onRightSide)
        {
            driftAcceleration.x = driftMaxAcceleration.x;
            driftSpeed.x = driftMaxSpeed.x;
        }
        else
        {
            driftAcceleration.x = -driftMaxAcceleration.x;
            driftSpeed.x = -driftMaxSpeed.x;
        }
    }

    public void Init(Vector3 targetPosition, bool onRightSide, Transform cannonTarget)
    {
        initTime = Time.time;
        startPosition = transform.position;
        this.targetPosition = targetPosition;
        journeyLength = Vector3.Distance(startPosition, targetPosition);
        this.onRightSide = onRightSide;
        this.cannonTarget = cannonTarget;
    }

    private void MoveIntoPosition()
    {
        float distanceCovered = (Time.time - initTime) * moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        if (fractionOfJourney >= 1)
        {
            isInPosition = true;
            transform.position = targetPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
        }
    }

    public void ForceSink()
    {
        health = 0;
    }

    public void SetTimeBetweenShots(float seconds)
    {
        secondsBetweenShots = seconds;
    }
}