using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyObject : MonoBehaviour
{
    [SerializeField] private LineRendererScript referenceLine;
    [SerializeField] private float directionMultiplier; //Set which direction the reference line is on

    [SerializeField] private Transform spinner;

    private float previousLength;

    private void Start()
    {
        if (referenceLine == null || spinner == null) {
            Destroy(this);
        }

        directionMultiplier = Mathf.Sign(directionMultiplier);
        previousLength = referenceLine.GetLineLength();

    }

    private void Update()
    {
        if (referenceLine.GetLineLength() > previousLength) {
            Debug.Log("Rotating CounterClockWise");
            spinner.Rotate(Vector3.forward * 5f * directionMultiplier );
            previousLength = referenceLine.GetLineLength();
        } else if (referenceLine.GetLineLength() < previousLength)
        {
            Debug.Log("Rotating ClockWise");
            spinner.Rotate(Vector3.forward * -5f * directionMultiplier );
            previousLength = referenceLine.GetLineLength();
        }
    }

}
