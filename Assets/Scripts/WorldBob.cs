using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBob : MonoBehaviour
{
    [SerializeField] float bobSpeed;
    [SerializeField] float bobAmount;
    private Vector3 startRotation;

    private void Start()
    {
        startRotation = transform.eulerAngles;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(startRotation + new Vector3(0f, 0f, Mathf.Sin(Time.time * bobSpeed) * bobAmount));
    }
}
