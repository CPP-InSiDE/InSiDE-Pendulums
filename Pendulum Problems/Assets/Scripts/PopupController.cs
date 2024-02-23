using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float popupTime;
    public float lingerTime;
    public AnimationCurve curve;
    float startTime;
    Vector3 vel;
    GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        panel = transform.GetChild(0).gameObject;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;
        if(elapsedTime < popupTime + lingerTime)
        {
            float timeSinceStartPhase = elapsedTime / popupTime;
            panel.transform.localPosition = Vector3.Lerp(startPos, endPos, curve.Evaluate(timeSinceStartPhase));
        }
        else if (elapsedTime < popupTime + lingerTime + popupTime)
        {
            float timeSinceStartPhase = (elapsedTime - (popupTime + lingerTime)) / popupTime;
            panel.transform.localPosition = Vector3.Lerp(endPos, startPos, curve.Evaluate(timeSinceStartPhase));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
