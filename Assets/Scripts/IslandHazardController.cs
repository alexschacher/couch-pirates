using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandHazardController : MonoBehaviour
{
    [SerializeField] private GameObject islandHazard;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float steeringSpeed;
    [SerializeField] private float spawnPositionVariance;
    [SerializeField] private Vector2 timeRangeBetweenHazardSpawns;
    private float timer;
    private float currentTimeBetweenHazardSpawns;
    private Vector3 islandStartPosition;

    private void Start()
    {
        islandStartPosition = islandHazard.transform.position;
        currentTimeBetweenHazardSpawns = timeRangeBetweenHazardSpawns.x;
    }

    public void Move(float amount)
    {
        islandHazard.transform.position += new Vector3(amount * steeringSpeed * Time.deltaTime, 0f, 0f);
    }

    private void Update()
    {
        islandHazard.transform.position += new Vector3(0f, 0f, moveSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer > currentTimeBetweenHazardSpawns)
        {
            SpawnHazard();
        }
    }

    private void SpawnHazard()
    {
        islandHazard.transform.position = islandStartPosition + new Vector3(Random.Range(-spawnPositionVariance, spawnPositionVariance), 0f, 0f);
        currentTimeBetweenHazardSpawns = Random.Range(timeRangeBetweenHazardSpawns.x, timeRangeBetweenHazardSpawns.y);
        timer = 0f;
    }
}