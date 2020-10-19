using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBarrel : MonoBehaviour
{
    [SerializeField] private float bobSpeed;
    [SerializeField] private float bobTime;
    [SerializeField] private float moveSpeed;
    private float bobTimer;

    private void OnTriggerEnter(Collider other)
    {
        Cannonball cannonball = other.gameObject.GetComponent<Cannonball>();
        if (cannonball != null)
        {
            Destroy(cannonball.gameObject);
            Destroy(this);
            return;
        }
    }

    void Update()
    {
        bobTimer += Time.deltaTime;
        if (bobTimer > bobTime)
        {
            bobTimer -= bobTime;
            bobSpeed = -bobSpeed;
        }

        transform.Translate(new Vector3(0, bobSpeed, -moveSpeed));

        if (transform.position.z < -10)
        {
            Destroy(this.gameObject);
            TestGamemodeController.INSTANCE.SpawnHole();
        }
    }
}
