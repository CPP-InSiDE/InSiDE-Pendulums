using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SimulationManager simulationManager;
    private Vector3 initialPosition;
    [SerializeField] private GameObject pendulum;
    private bool active = false;
    public float xVelocity;

    private void Start() {
        active = false;
        initialPosition = transform.position;
    }


    //Move at a rate
    public void ResetBullet() {
        transform.position = initialPosition;
    }
    public void StartBullet() {
        transform.position = initialPosition;
        active = true;
        this.enabled = true;
    }

    public void StopBullet() {
        active = false;
    }

    private void Update()
    {
        if (active == true) {
            transform.position += (Vector3.right * xVelocity * Time.deltaTime);
        }
    }
    //Detect when contact is made with the pendulum
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == pendulum) {
            StickToPendulum();
        }
    }


    //Attach itself to the pendulum object and destroy this object
    private void StickToPendulum (){
        transform.parent = pendulum.transform;
        simulationManager.StartRotation();
        this.enabled = false;
    }

}
