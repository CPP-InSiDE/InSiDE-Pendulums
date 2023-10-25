using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputValidation : MonoBehaviour
{
    private TMP_InputField userInput;
    [SerializeField] private int digitsAfterDecimal = 3;
    private void Start()
    {
        userInput = gameObject.GetComponent<TMP_InputField>();    
    }

    public void CheckInput() {
        float setValue = 0;

        float.TryParse(userInput.text, out setValue);

        setValue = Mathf.Round(setValue * Mathf.Pow(10, digitsAfterDecimal)) / Mathf.Pow(10, digitsAfterDecimal);

        userInput.text = "" + Mathf.Abs(setValue);
    }
}
