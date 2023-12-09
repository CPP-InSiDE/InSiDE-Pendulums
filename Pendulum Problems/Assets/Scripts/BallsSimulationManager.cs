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

    [SerializeField] private TextMeshProUGUI currentTimeScale;
    //public float bulletVelocity;
    //public float predictedAngularVelocity;
    [SerializeField] private TMP_InputField rotationTimeText;
    [SerializeField] private float rotationTime;

    protected Color defaultPredictionColor;

    [SerializeField] private float marginOfError = .04f;

    private bool applyingForce;
    private float simulationStartTime;

    [SerializeField] private GameObject info;

    private void Start()
    {
        applyingForce = false;
        defaultPredictionColor = userPredictionPendulumSpriteRenderer.color;
        Time.timeScale = 1;

        info.SetActive(true);
    }

    public void StartSimulation() {
        
        //realPendulum.StartRotation();
        predictionPendulum.ResetRotation();
        userPredictionPendulumSpriteRenderer.color = defaultPredictionColor;
        //Start moving the bullet

        simulationStartTime = Time.time;
        applyingForce = true;

        //rotationTime = 10f;
        //float.TryParse(rotationTimeText.text, out rotationTime);

        float.TryParse(rotationTimeText.text, out rotationTime);
        rotationTime = Mathf.Max(rotationTime, .1f);
        float.TryParse(appliedForceMagnitude.text, out forceMagnitude);
        float.TryParse(appliedForceTime.text, out forceTime);

        float.TryParse(realPendulumInitialVelocity.text, out initialVelocity);
        initialVelocity *= -1f;
        //Debug.Log("Rotation Time: " + rotationTime);
        CancelInvoke("StopSimulation");
        Invoke("StopSimulation", forceTime + rotationTime);

        realPendulum.rotationSpeed = initialVelocity;
        realPendulum.StartRotation();

        info.SetActive(false);
    }

    public void StartRotation() {
        //Start rotating the rotation object
        //CalculateOutputPendulum();
        //SetPlayerPrediction();

        
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

        info.SetActive(true);

    }

    protected virtual void CalculateOutputPendulum() {
        float currentTime = Time.time - simulationStartTime;

        if (currentTime > forceTime) {
            currentTime = forceTime;
            applyingForce = false;
            //check prediction?
            SetPlayerPrediction();
            predictionPendulum.StartRotation();

            Debug.Log("Finished applying force");
        }

        realPendulum.rotationSpeed = initialVelocity + -1f * forceMagnitude * currentTime / 1.79f;


    }


    
    protected virtual void SetPlayerPrediction() {
        //Set predictionblock
        float predictedRotationalVelocity;
        float.TryParse(predictionPendulumVelocity.text, out predictedRotationalVelocity);

        predictionPendulum.rotationSpeed = -1f * predictedRotationalVelocity;


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
       
        predictionPendulumVelocity.text = "" + Mathf.Abs(realPendulum.rotationSpeed).ToString("F3");
    }

    public void SetTimeScale(float timeScale) {
        Time.timeScale = Mathf.Pow(10, timeScale); // timeScale;
        Debug.Log("Setting time scale to: " + Mathf.Pow(10, timeScale));
        currentTimeScale.text = Mathf.Pow(10, timeScale).ToString("F3");
    }
}
