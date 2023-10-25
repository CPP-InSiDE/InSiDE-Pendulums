using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




public class BallsSimulationManager : MonoBehaviour
{
    
    [SerializeField] protected RotationObject realPendulum;
    [SerializeField] public TMP_InputField realPendulumInitialVelocity;
    private float initialVelocity;
    [SerializeField] protected RotationObject predictionPendulum;
    [SerializeField] public TMP_InputField predictionPendulumVelocity;

    [SerializeField] public TMP_InputField appliedForceMagnitude;
    private float forceMagnitude;
    [SerializeField] public TMP_InputField appliedForceTime;
    private float forceTime;

    [SerializeField] protected SpriteRenderer userPredictionPendulumSpriteRenderer;

    //public float bulletVelocity;
    //public float predictedAngularVelocity;
    [SerializeField] private TMP_InputField rotationTimeText;
    [SerializeField] private float rotationTime;

    protected Color defaultPredictionColor;

    [SerializeField] private float marginOfError = .04f;

    private bool applyingForce;
    private float simulationStartTime;

    private void Start()
    {
        applyingForce = false;
        defaultPredictionColor = userPredictionPendulumSpriteRenderer.color;
        Time.timeScale = 1;
    }

    public void StartSimulation() {
        
        realPendulum.StartRotation();
        //predictionPendulum.StartRotation();
        //Start moving the bullet

        simulationStartTime = Time.time;
        applyingForce = true;

        float.TryParse(rotationTimeText.text, out rotationTime);
    }

    public void StartRotation() {
        //Start rotating the rotation object
        //CalculateOutputPendulum();
        //SetPlayerPrediction();

        float.TryParse(rotationTimeText.text, out rotationTime);
        float.TryParse(appliedForceMagnitude.text, out forceMagnitude);
        float.TryParse(appliedForceTime.text, out forceTime);
        float.TryParse(realPendulumInitialVelocity.text, out initialVelocity);
        Debug.Log("Rotation Time: " + rotationTime);
        Invoke("StopSimulation", rotationTime);
        realPendulum.StartRotation();
        //predictionPendulum.StartRotation();
    }

    public void Update()
    {
        if (applyingForce == false) {
            return;
        }

        
        CalculateOutputPendulum();
    }

    public void StopSimulation() {
        //Stop the rotations
       
        realPendulum.StopRotation();
        predictionPendulum.StopRotation();

    }

    protected virtual void CalculateOutputPendulum() {
        float currentTime = Time.time - simulationStartTime;

        if (currentTime > forceTime) {
            currentTime = forceTime;
            applyingForce = false;
        }

        realPendulum.rotationSpeed = 1.79f * initialVelocity + -1f * forceMagnitude * currentTime;


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
            SetExactSolution();
        }
        else {
            userPredictionPendulumSpriteRenderer.color = defaultPredictionColor;
        }
    }

    protected bool WithinAcceptedRange(float testedValue, float expectedValue) {
        if (Mathf.Abs(testedValue - expectedValue) > Mathf.Abs(expectedValue * marginOfError)) {
            return false;
        }

       

        return true;
    }

    protected void SetExactSolution() {
        
        predictionPendulum.rotationSpeed = realPendulum.rotationSpeed;

        //Change text to exact values
       
        predictionPendulumVelocity.text = "" + (realPendulum.rotationSpeed);
    }

    public void SetTimeScale(float timeScale) {
        Time.timeScale = timeScale;
    }
}
