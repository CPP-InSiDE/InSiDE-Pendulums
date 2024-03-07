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
    
    [SerializeField] private Image fillBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Transform popupSpot;
    [SerializeField] private GameObject popupSuccess;
    [SerializeField] private GameObject popupFailure;

    private void Start()
    {
        applyingForce = false;
        defaultPredictionColor = userPredictionPendulumSpriteRenderer.color;
        Time.timeScale = 1;

        info.SetActive(true);
    }

    public void StartSimulation() {
        ResetSimulation();
        //realPendulum.StartRotation();
        //Start moving the bullet

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

        progressText.text = "Applying force...";
    }
    public void ResetSimulation()
    {
        StopSimulation();
        predictionPendulum.ResetRotation();
        realPendulum.ResetRotation();
        userPredictionPendulumSpriteRenderer.color = defaultPredictionColor;
        simulationStartTime = Time.time;
        fillBar.fillAmount = 0;
        progressText.text = "";
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
        applyingForce = false;

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
            progressText.text = "Finished applying force.";
        }

        realPendulum.rotationSpeed = initialVelocity + -1f * forceMagnitude * currentTime / 1.79f;
        if(fillBar)
        {
            fillBar.fillAmount = currentTime/forceTime;
        }

    }


    
    protected virtual void SetPlayerPrediction() {
        //Set predictionblock
        float predictedRotationalVelocity;
        float.TryParse(predictionPendulumVelocity.text, out predictedRotationalVelocity);

        predictionPendulum.rotationSpeed = -1f * predictedRotationalVelocity;


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
       
        predictionPendulumVelocity.text = "" + Mathf.Abs(realPendulum.rotationSpeed).ToString("F3");
    }

    public void SetTimeScale(float timeScale) {
        Time.timeScale = Mathf.Pow(10, timeScale); // timeScale;
        Debug.Log("Setting time scale to: " + Mathf.Pow(10, timeScale));
        currentTimeScale.text = Mathf.Pow(10, timeScale).ToString("F3");
    }
}
