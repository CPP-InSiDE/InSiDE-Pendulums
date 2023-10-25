using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArrow : MonoBehaviour
{
    private RectTransform arrow;
    private int directionMultiplier = 1;
    void Start()
    {
        arrow = gameObject.GetComponent<RectTransform>();
    }

    public int GetDirectionMultiplier() {
        return directionMultiplier;
    }
    // Update is called once per frame
    public void FlipDirection()
    {
        directionMultiplier *= -1;
        arrow.Rotate(new Vector3(0, 0, 180f));
    }
}
