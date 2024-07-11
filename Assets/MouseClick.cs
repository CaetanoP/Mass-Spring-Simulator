using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseClick : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera; // Armazena a referência à câmera principal
    private Vector3 mousePosition;

    public Simulator simuladorScript;

    void Start()
    {
        //mainCamera = Camera.main; // Obtém a câmera principal no início
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)|| Input.GetKey(KeyCode.LeftShift)) // Se o botão esquerdo do mouse for pressionado
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) // Se o raio lançado pela câmera atingir um objeto
            {
                mousePosition = hit.point; // A posição do mouse na tela é a posição do ponto atingido pelo raio
                //verifica o nome do objeto atingido 
                //Debug.Log(hit.collider.gameObject.name);
                if((hit.collider.gameObject.name == "BlocoL") && Input.GetMouseButton(1))
                {
                    //Debug.Log("Clicou no BlocoL");
                    simuladorScript.posicoes[0] = new Vector3(mousePosition.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                    simuladorScript.posicoes[1] = new Vector3(mousePosition.x + simuladorScript.x_0[1], hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                    
                }
                if((hit.collider.gameObject.name == "BlocoR") && Input.GetMouseButton(1))
                {
                    simuladorScript.posicoes[0] = new Vector3(mousePosition.x- simuladorScript.x_0[1], hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                    simuladorScript.posicoes[1] = new Vector3(mousePosition.x , hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);

                }
                if (hit.collider.gameObject.name == "BlocoL" )
                {
                    //Debug.Log("Clicou no BlocoL");
                   
                    simuladorScript.posicoes[0] = new Vector3(mousePosition.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);

                }
                if (hit.collider.gameObject.name == "BlocoR")
                {
                    //Debug.Log("Clicou no BlocoL");
                   
                    simuladorScript.posicoes[1] = new Vector3(mousePosition.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);

                }
                if(Input.GetKey(KeyCode.LeftShift) && hit.collider.gameObject.name == "BlocoL")
                {
                    simuladorScript.posicoes[0] = new Vector3(mousePosition.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                    simuladorScript.posicoes[1] = new Vector3(simuladorScript.anteparos[1].x - (mousePosition.x -simuladorScript.anteparos[0].x), hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                }
                if(Input.GetKey(KeyCode.LeftShift) && hit.collider.gameObject.name == "BlocoR")
                {
                    simuladorScript.posicoes[1] = new Vector3(mousePosition.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                    simuladorScript.posicoes[0] = new Vector3(simuladorScript.anteparos[0].x + (-mousePosition.x +simuladorScript.anteparos[1].x), hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                }
                
            }



        }
       
       
    }

}