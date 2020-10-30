using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStream : MonoBehaviour
{
    [SerializeField] private float waterFillSpeed;

    private void Update()
    {
        TestGamemodeController.INSTANCE.FillHullWater(waterFillSpeed * Time.deltaTime);
    }
}
