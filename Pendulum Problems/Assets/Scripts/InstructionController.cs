using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionController : MonoBehaviour
{
    Button button;
    TextMeshProUGUI text;
    GameObject panel;
    bool showing = true;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        text = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        panel = button.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnToggleInstructions()
    {
        showing = !showing;
        panel.SetActive(showing);
        text.text = showing? "Hide Instructions" : "Show Instructions";
    }
}
