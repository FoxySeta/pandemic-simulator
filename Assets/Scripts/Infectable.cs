using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infectable : MonoBehaviour
{

    public enum Status { Healthy = 0, Sick = 1, Recovered = 2 };
    public bool zeroPatient = false;
    public Material sick, recovered;
    public float recoveryTime = 10; // seconds

    private Status condition = new Status();
    private static int healthyCount = 0, sickCount = 0, recoveredCount = 0;

#pragma warning disable 0414 // = field unused
    [DebugGUIGraph(r: 0.733f, g: 0.392f, b: 0.114f, group: 0)]
    float Sick { get { return (float)GetSick()/ GetTotal(); } }
    [DebugGUIGraph(r: 0.796f, g: 0.541f, b: 0.753f, group: 0)]
    float NotRecoveredYet { get { return 1.0f - (float)recoveredCount / GetTotal(); } }

    private void Start()
    {
        ++healthyCount;
        DebugGUI.LogPersistent("healthy", "Healthy: " + healthyCount);
        if (zeroPatient)
            Infect();
    }

    public static int GetHealthy()
    {
        return healthyCount;
    }

    public static int GetSick()
    {
        return sickCount;
    }

    public static int GetRecovered()
    {
        return recoveredCount;
    }

    public static int GetTotal()
    {
        return healthyCount + sickCount + recoveredCount;
    }

    public Status GetStatus()
    {
        return condition;
    }

    private void Infect()
    {
        condition = Status.Sick;
        GetComponent<MeshRenderer>().material = sick;
        Invoke("Recover", recoveryTime);
        --healthyCount;
        DebugGUI.LogPersistent("healthy", "Healthy: " + healthyCount);
        ++sickCount;
        DebugGUI.LogPersistent("sick", "Sick: " + sickCount);
    }

    private void Recover()
    {
        condition = Status.Recovered;
        GetComponent<MeshRenderer>().material = recovered;
        --sickCount;
        DebugGUI.LogPersistent("sick", "Sick: " + sickCount);
        ++recoveredCount;
        DebugGUI.LogPersistent("recovered", "Recovered: " + recoveredCount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (condition == Status.Healthy)
        {
            Infectable diseaseVector = collision.gameObject.GetComponent<Infectable>();
            if (diseaseVector && diseaseVector.GetStatus() == Status.Sick)
                Infect();
        }
    }

}