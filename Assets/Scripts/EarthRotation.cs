using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotation : MonoBehaviour
{
    public float earthAxisAngle = 23.5f;
    public float angularVelocity = -12;
    public bool RotateEarth = true;

    private bool isRotating = false;

    private Vector3 spawnPos;
    private Quaternion spawnRot;


    void Awake()
    {
        spawnPos = transform.position;
        spawnRot = transform.rotation;
    }


    void ResetToSpawn()
    {
        transform.position = spawnPos;
        transform.rotation = spawnRot;
    }


    private void OnEnable()
    {
        POIManager.POIsLoaded += StartRotation;
        POIManager.StopRotation += StopRotation;
    }


    void Update()
    {
        if (isRotating)
            transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime, Space.Self);
    }

    void StartRotation()
    {
        transform.eulerAngles = new Vector3(0f, 0f, earthAxisAngle);
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
        ResetToSpawn();
    }
}
