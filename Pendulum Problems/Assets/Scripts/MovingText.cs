using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingText : MonoBehaviour
{
    [SerializeField] private Transform target;
    private RectTransform textRect;

    private void Start() {
        textRect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        textRect.anchoredPosition = target.position;
    }
}
