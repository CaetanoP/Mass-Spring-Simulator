using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
    
    [SerializeField]
    private Slider[] sliders;
    public Simulator simulatorScript;

    public void ChangeMassL()
    {
        Debug.Log("ChangeMass");
        Debug.Log(sliders[0].value);
        simulatorScript.mass[0] = sliders[0].value;

    }
    public void ChangeMassR()
    {
        Debug.Log("ChangeMass");
        Debug.Log(sliders[0].value);
        simulatorScript.mass[1] = sliders[1].value;

    }
    public void ChangeKL()
    {
        Debug.Log("ChangeKL");
        //Debug.Log(sliders[2].value);
        simulatorScript.K[0] = sliders[2].value;

    }
    public void ChangeKR()
    {
        Debug.Log("ChangeKR");
        //Debug.Log(sliders[3].value);
        simulatorScript.K[2] = sliders[3].value;

    }
    public void ChangeKMiddle()
    {
        Debug.Log("ChangeKM");
       // Debug.Log(sliders[4].value);
        simulatorScript.K[1] = sliders[4].value;

    }   
    
}
