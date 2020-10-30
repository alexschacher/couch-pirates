using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanPivotController : MonoBehaviour
{
    private float rotationAngle;
    [SerializeField] private GameObject playerShip;

    private void Update()
    {
        transform.RotateAround(playerShip.transform.position, new Vector3(0f, 1f, 0f), rotationAngle * Time.deltaTime);
    }

    public void SetRotationAngle(float rotAngle)
    {
        rotationAngle = rotAngle;
    }
}
