using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ServerManager : MonoBehaviour
{
    public string problemName;
    public TMP_InputField loginInput;
    public GameObject failText;
    public Button submitButton;
    public GameObject mainPanel;
    Coroutine failTextCoroutine;
    bool canAcceptSubmission = true;
    //string URI = "https://eea3cd8f-0bdd-4a0a-b3b1-e28fd00d815a-00-1vvq7amip0l3c.kirk.replit.dev/";
    //string URI = "http://20.168.13.43/";
    string URI = "https://inside.slothlab.info/";
    float nextPingTime;
    float pingInterval = 30f;
    bool isPinging;

    public static ServerManager main;

    private void Awake()
    {
        if(ServerManager.main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (isPinging)
        {
            if(Time.time > nextPingTime)
            {
                nextPingTime = Time.time + pingInterval;
                Ping();
            }
        }
    }
    public void UpdateProblemName(string newName)
    {
        problemName = newName;
    }
    public void Login()
    {
        if(!canAcceptSubmission) { return; }
        canAcceptSubmission = false;
        submitButton.interactable = false;
        string uri = URI + "login";
        Dictionary<string,string> parameters = new Dictionary<string,string>();
        parameters["id"] = GetHash(loginInput.text);
        StartCoroutine(GetRequest(uri, parameters));
    }
    public void TryStartProblem()
    {
        if(!canAcceptSubmission) { return; }
        canAcceptSubmission = false;
        if(submitButton) submitButton.interactable = false;
        string uri = URI + "start";
        Dictionary<string,string> parameters = new Dictionary<string,string>();
        parameters["id"] = GetHash(loginInput.text);
        parameters["problem"] = problemName;
        StartCoroutine(GetRequest(uri, parameters));
    }
    public void Ping()
    {
        string uri = URI + "ping";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters["id"] = GetHash(loginInput.text);
        parameters["problem"] = problemName;
        StartCoroutine(GetRequest(uri, parameters));
    }
    public void Attempt(bool correct)
    {
        string uri = URI + "attempt";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters["id"] = GetHash(loginInput.text);
        parameters["problem"] = problemName;
        parameters["correct"] = correct?"True":"False";
        Debug.Log("correct: " + correct);
        StartCoroutine(GetRequest(uri, parameters));
    }
    public void OnReceiveResponse()
    {
        canAcceptSubmission = true;
        if(submitButton) submitButton.interactable = true;
    }
    IEnumerator GetRequest(string uri, Dictionary<string, string> parameters)
    {
        string fulluri = uri;
        if(parameters.Count > 0)
        {
            fulluri += "?";
        }
        bool first = true;
        foreach (KeyValuePair<string, string> p in parameters)
        {
            fulluri += first ? "" : "&";
            first = false;
            fulluri += p.Key + "=" + p.Value;
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Get(fulluri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            OnReceiveResponse();

            string result = webRequest.downloadHandler.text.Replace("\n", "").Replace("\"", "");

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(pages[page] + ": Connection Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    // Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    if (webRequest.responseCode == 403 && result == "invalid request")
                    {
                        InvalidID();
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    if (result == "user found")
                    {
                        ValidID();
                    }
                    // PrintData(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
    public void InvalidID()
    {
        if (failTextCoroutine != null)
        {
            StopCoroutine(failTextCoroutine);
        }
        failTextCoroutine = StartCoroutine(FailTextDelay());
    }
    public void ValidID()
    {
        isPinging = true;
        nextPingTime = 0;
        mainPanel.SetActive(false);
    }
    public static string GetHash(string text)
    {
        string modifiedText = "1701934689-" + text + "-CREATE";
        byte[] bytes = Encoding.ASCII.GetBytes(modifiedText);
        SHA256Managed sha = new SHA256Managed();
        byte[] hash = sha.ComputeHash(bytes);
        StringBuilder SB = new StringBuilder();
        foreach (byte b in hash)
            SB.Append(b.ToString("x2"));
        return SB.ToString();
    }
    IEnumerator FailTextDelay()
    {
        failText.SetActive(true);
        yield return new WaitForSeconds(5f);
        failText.SetActive(false);
    }
}
