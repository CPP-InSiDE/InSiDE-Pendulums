using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    public float rotationSpeed;
    [SerializeField] private bool active = false;

    private Quaternion initialTransform;

    private void Start()
    {
        active = false;
        initialTransform = this.transform.rotation;
        //transform.rotation;
    }
    public void ResetRotation()
    {
        gameObject.transform.rotation = initialTransform;
    }
    public void StartRotation()
    {
        active = true;
        gameObject.transform.rotation = initialTransform;
    }

    public void StopRotation() {
        active = false;
    }

    private void Update()
    {
        if (active == true) {
            float currentRotationDegrees = transform.rotation.eulerAngles.z;

           
            float newRotationDegrees = currentRotationDegrees + rotationSpeed * Mathf.Rad2Deg * Time.deltaTime;

            transform.rotation = Quaternion.Euler(0, 0, newRotationDegrees);
        }
    }
}
