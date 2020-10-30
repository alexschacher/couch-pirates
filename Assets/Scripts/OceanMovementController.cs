using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanMovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private void Update()
    {
        transform.position = transform.position + new Vector3(0f, 0f, -moveSpeed * Time.deltaTime);
    }
}
