using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;


[System.Serializable]
public class BlockInputs
{
    public PhysicsObject affectedBlock;
    [SerializeField] public TMP_InputField blockVelocity;
    [SerializeField] public GravityArrow velocityDirection;
    [SerializeField] public TMP_InputField blockAcceleration;
    [SerializeField] public GravityArrow accelerationDirection;
}

public class SimulationManager : MonoBehaviour
{
    
    [SerializeField] protected RotationObject realPendulum;
    //[SerializeField] public TMP_InputField realPendulumVelocity;
    [SerializeField] protected RotationObject predictionPendulum;
    [SerializeField] public TMP_InputField predictionPendulumVelocity;
    [SerializeField] private Bullet bullet;
    [SerializeField] private TMP_InputField bulletVelocityText;
    [SerializeField] private float bulletVelocity;

    [SerializeField] protected SpriteRenderer userPredictionPendulumSpriteRenderer;

    //public float bulletVelocity;
    //public float predictedAngularVelocity;
    [SerializeField] private TMP_InputField rotationTimeText;
    [SerializeField] private float rotationTime;

    protected Color defaultPredictionColor;

    [SerializeField] private float marginOfError = .04f;

    [SerializeField] private TextMeshProUGUI currentTimeScale;
    [SerializeField] private Transform popupSpot;
    [SerializeField] private GameObject popupSuccess;
    [SerializeField] private GameObject popupFailure;

    private void Start()
    {
        defaultPredictionColor = userPredictionPendulumSpriteRenderer.color;
        Time.timeScale = 1;
    }

    public void StartSimulation() {
        ResetSimulation();
        //Start moving the bullet

        float.TryParse(bulletVelocityText.text, out bulletVelocity);
        bullet.xVelocity = bulletVelocity;
        bullet.StartBullet();
    }

    public void StartRotation() {
        CancelInvoke("StopSimulation");
        //Start rotating the rotation object
        CalculateOutputPendulum();
        SetPlayerPrediction();

        float.TryParse(rotationTimeText.text, out rotationTime);
        Debug.Log("Rotation Time: " + rotationTime);
        Invoke("StopSimulation", rotationTime);
        realPendulum.StartRotation();
        predictionPendulum.StartRotation();
    }

    public void ResetSimulation()
    {
        StopSimulation();
        bullet.ResetBullet();
        realPendulum.ResetRotation();
        predictionPendulum.ResetRotation();
    }

    public void StopSimulation() {
        //Stop the rotations
        bullet.StopBullet();
        realPendulum.StopRotation();
        predictionPendulum.StopRotation();

    }

    protected virtual void CalculateOutputPendulum() {
        //.06 * 300 * .36 * k = (.06 * .36^2 + 1.5 * .54^2)w
        //6.48 * k = (.007776 + .4374)w
        //6.48 * k = .445176 w
        //14.556 * k = w    Whats K

        //w = .04852014 * Velocity * k

        realPendulum.rotationSpeed = .04852014f * bulletVelocity;
       

    }


    protected void SetBlockKinematics(BlockInputs block) {
        float setVelocity = 0;

        float.TryParse(block.blockVelocity.text, out setVelocity);
        
        block.blockVelocity.text = "" + Mathf.Abs(setVelocity);
        setVelocity = Mathf.Abs(setVelocity) * block.velocityDirection.GetDirectionMultiplier();
        

        float setAcceleration = 0;
        float.TryParse(block.blockAcceleration.text, out setAcceleration);
        
        block.blockAcceleration.text = "" + Mathf.Abs(setAcceleration);
        setAcceleration = Mathf.Abs(setAcceleration) * block.accelerationDirection.GetDirectionMultiplier();


        block.affectedBlock.SetVelocityMagnitude(setVelocity);
        block.affectedBlock.SetAccelerationMagnitude(setAcceleration);

        
    }
    protected virtual void SetPlayerPrediction() {
        //Set predictionblock
        float predictedRotationalVelocity;
        float.TryParse(predictionPendulumVelocity.text, out predictedRotationalVelocity);

        predictionPendulum.rotationSpeed = predictedRotationalVelocity;


        if (WithinAcceptedRange(predictionPendulum.rotationSpeed, realPendulum.rotationSpeed) == true)
        {
            userPredictionPendulumSpriteRenderer.color = Color.green;
            ServerManager.main.Attempt(true);
            Instantiate(popupSuccess, popupSpot.position, popupSpot.rotation, popupSpot);
            SetExactSolution();
        }
        else {
            userPredictionPendulumSpriteRenderer.color = defaultPredictionColor;
            ServerManager.main.Attempt(false);
            Instantiate(popupFailure, popupSpot.position, popupSpot.rotation, popupSpot);
        }
    }

    protected bool WithinAcceptedRange(float testedValue, float expectedValue) {
        if (Mathf.Abs(testedValue - expectedValue) > Mathf.Abs(marginOfError)) {
            return false;
        }

       

        return true;
    }

    protected void SetExactSolution() {
        
        predictionPendulum.rotationSpeed = realPendulum.rotationSpeed;

        //Change text to exact values
       
        predictionPendulumVelocity.text = "" + (realPendulum.rotationSpeed).ToString("F3");
    }

    public void SetTimeScale(float timeScale) {
        Time.timeScale = Mathf.Pow(10, timeScale); // timeScale;
        Debug.Log("Setting time scale to: " + Mathf.Pow(10, timeScale));
        currentTimeScale.text = Mathf.Pow(10, timeScale).ToString("F3");
    }
}
