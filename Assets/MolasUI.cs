using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolasUI : MonoBehaviour
{
   private GameObject molasUI;
   public GameObject[] objects;
   public Simulator simuladorScript;

   public float tamanhoMola;

    void Start()
    {
        molasUI = this.gameObject;
        Debug.Log(simuladorScript.posicoes[0].x-simuladorScript.anteparos[0].x);
    }
    void Update()
    {
        float d =objects[1].transform.position.x- objects[0].transform.position.x ;
        molasUI.transform.position = new Vector3(objects[0].transform.position.x+ d/2 , 0, 0);
        molasUI.transform.localScale = new Vector3(0.12f,d/tamanhoMola, 1);
    }

    
}
