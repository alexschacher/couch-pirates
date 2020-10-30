using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGamemodeController : MonoBehaviour
{
    public static TestGamemodeController INSTANCE;
    [SerializeField] private List<Transform> barrelSpawnpoints;
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private float secondsBetweenBarrels;
    [SerializeField] private List<Transform> holeSpawnpoints;
    [SerializeField] private GameObject holePrefab;
    [SerializeField] private GameObject hullWater;
    [SerializeField] private GameObject hullWaterFullTransform;
    [SerializeField] private GameObject enemyShipPrefab;
    [SerializeField] private Transform enemyShipLeftTargetPosition;
    [SerializeField] private Transform enemyShipRightTargetPosition;
    [SerializeField] private Transform enemyShipLeftSpawnPoint;
    [SerializeField] private Transform enemyShipRightSpawnPoint;
    [SerializeField] private GameObject shootTarget;
    private Vector3 hullWaterStartPosition, hullWaterFullPosition;
    private GameObject[] activeHoles;
    private GameObject leftShip, rightShip;
    private float secondCounter;
    public bool preventWater = false;
    public bool preventSink = false;

    private void Awake()
    {
        INSTANCE = this;
    }

    private void Start()
    {
        activeHoles = new GameObject[holeSpawnpoints.Capacity];
        hullWaterStartPosition = hullWater.transform.position;
        hullWaterFullPosition = hullWaterFullTransform.transform.position;

        //SpawnEnemyShip();
    }

    private void Update()
    {
        //SpawnBarrels();
    }

    private void SpawnBarrels()
    {
        secondCounter += Time.deltaTime;
        if (secondCounter > secondsBetweenBarrels)
        {
            secondCounter -= secondsBetweenBarrels;
            int randomIndex = Random.Range(0, barrelSpawnpoints.Count - 1);
            Instantiate(barrelPrefab, barrelSpawnpoints[randomIndex].position, Quaternion.identity);
        }
    }

    public void SpawnHole()
    {
        return; // old code

        int randomIndex = Random.Range(0, 4);

        if (activeHoles[randomIndex] == null)
        {
            activeHoles[randomIndex] = Instantiate(holePrefab, holeSpawnpoints[randomIndex].position, holeSpawnpoints[randomIndex].rotation);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (activeHoles[i] == null)
                {
                    activeHoles[i] = Instantiate(holePrefab, holeSpawnpoints[i].position, holeSpawnpoints[i].rotation);
                    break;
                }
            }
        }
    }

    public void FillHullWater(float amount)
    {
        if (preventWater) return;

        hullWater.transform.Translate(Vector3.up * amount);

        if (hullWater.transform.position.y > hullWaterFullPosition.y)
        {
            hullWater.transform.position = hullWaterFullPosition;
        }
    }

    public void EmptyHullWater(float amount)
    {
        hullWater.transform.Translate(Vector3.up * -amount);

        if (hullWater.transform.position.y < hullWaterStartPosition.y)
        {
            hullWater.transform.position = hullWaterStartPosition;
        }
    }

    public void SpawnEnemyShip()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            if (leftShip == null)
            {
                SpawnEnemyShip(true);
            }
            else if (rightShip == null)
            {
                SpawnEnemyShip(false);
            }
        }
        else
        {
            if (rightShip == null)
            {
                SpawnEnemyShip(false);
            }
            else if (leftShip == null)
            {
                SpawnEnemyShip(true);
            }
        }
    }

    public void SpawnEnemyShip(bool onLeft)
    {
        if (onLeft)
        {
            if (leftShip != null) return;

            GameObject enemyShip = Instantiate(enemyShipPrefab, enemyShipLeftSpawnPoint.position, Quaternion.identity);
            enemyShip.GetComponent<EnemyShipController>().Init(enemyShipLeftTargetPosition.position, false, shootTarget.transform);
            leftShip = enemyShip;
        }
        else
        {
            if (rightShip != null) return;

            GameObject enemyShip = Instantiate(enemyShipPrefab, enemyShipRightSpawnPoint.position, Quaternion.identity);
            enemyShip.GetComponent<EnemyShipController>().Init(enemyShipRightTargetPosition.position, true, shootTarget.transform);
            rightShip = enemyShip;
        }
    }

    public void SetHullWaterEmpty()
    {
        hullWater.transform.position = hullWaterStartPosition;
    }

    public void SetHullWaterFull()
    {
        hullWater.transform.position = hullWaterFullPosition;
    }
}