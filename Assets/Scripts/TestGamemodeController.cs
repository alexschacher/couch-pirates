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
    [SerializeField] private GameObject playerShip;
    private Vector3 hullWaterStartPosition, hullWaterFullPosition;
    private GameObject[] activeHoles;
    private float secondCounter;

    private void Awake()
    {
        INSTANCE = this;
    }

    private void Start()
    {
        activeHoles = new GameObject[holeSpawnpoints.Capacity];
        hullWaterStartPosition = hullWater.transform.position;
        hullWaterFullPosition = hullWaterFullTransform.transform.position;

        SpawnEnemyShip();
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

        //if (random == 0)
        {
           GameObject enemyShip = Instantiate(enemyShipPrefab, enemyShipLeftSpawnPoint.position, Quaternion.identity);
           enemyShip.GetComponent<EnemyShipController>().Init(enemyShipLeftTargetPosition.position, false, playerShip.transform);
        }
        //else 
        {
            GameObject enemyShip = Instantiate(enemyShipPrefab, enemyShipRightSpawnPoint.position, Quaternion.identity);
            enemyShip.GetComponent<EnemyShipController>().Init(enemyShipRightTargetPosition.position, true, playerShip.transform);
        }
    }
}