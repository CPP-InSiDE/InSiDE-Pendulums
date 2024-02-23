using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHelper : MonoBehaviour
{
    public string problemName;
    // Start is called before the first frame update
    void Start()
    {
        ServerManager.main.UpdateProblemName(problemName);
        ServerManager.main.TryStartProblem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
