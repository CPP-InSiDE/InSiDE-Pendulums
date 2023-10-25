using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] private SimulationManager simulationManager;

    private Vector2 initialVelocity;
    public Vector2 velocity;
    private Vector2 initialAcceleration;
    public Vector2 acceleration;

    private float activatedTime;
    private float timeSinceActivation;
    private bool active = false;
    private Vector2 startPosition;

    private void Start()
    {
        initialVelocity = velocity;
        initialAcceleration = acceleration;
        startPosition = transform.position;
        active = false;
    }

    public void Activate() {
        activatedTime = Time.time;
        active = true;
    }

    public void Deactivate()
    { 
        //activatedTime = Time.time;
        active = false;
    }

    private void FixedUpdate()
    {
        if (active == true) {
            timeSinceActivation = Time.time - activatedTime;

            transform.position = startPosition + velocity * timeSinceActivation + (.5f * acceleration * timeSinceActivation * timeSinceActivation);
        }
    }

    public void SetVelocityMagnitude(float magnitude) {
        velocity = initialVelocity.normalized * magnitude;
    }

    public void SetAccelerationMagnitude(float magnitude)
    {
        acceleration = initialAcceleration.normalized * magnitude;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall") {
            simulationManager.StopSimulation();
        }
    }

    public int CheckOriginalVelocityDirection (){
        //Debug.Log("Velocity Dot Product: " + Vector2.Dot(velocity, initialVelocity));
        return (int) Mathf.Sign(Vector2.Dot(velocity, initialVelocity));
    }

    public int CheckOriginalAccelerationDirection()
    {

        return (int)Mathf.Sign(Vector2.Dot(initialAcceleration, acceleration));
    }

}
