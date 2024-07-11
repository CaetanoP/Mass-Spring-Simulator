using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class Simulator : MonoBehaviour
{

    private Vector3[] LastPosition;
    public float dumping;
    //Faz uma array publica de gameobjects
    public GameObject[] PrefabBloco;
    //Faz uma array de velocidade
    [SerializeField]
    private Vector3[] velocidades;

    //Faz uma array de posições
    public Vector3[] posicoes;
    //Faz uma array de acelerações
    [SerializeField]
    private Vector3[] aceleracoes;
    public float[] mass;
    private GameObject[] objetos;
    public float[] K;
    [SerializeField]
    public float[] x_0;

    [SerializeField]
    public Vector3[] anteparos;

    private float cronometro =5f;
    //faz uma lista de float
    private List<float> blocoL;
   private List<float> blocoR;


    void Start()
    {
        //Instancia as listas
        blocoL = new List<float>();
        blocoR = new List<float>();


        //Inicializa a array dos objetos com n elementos
        objetos = new GameObject[PrefabBloco.Length];
        LastPosition = new Vector3[PrefabBloco.Length];
        for (int i = 0; i < PrefabBloco.Length; i++)
        {
            //Associa cada objeto com um bloco
            objetos[i] = PrefabBloco[i];
            //Debug.Log(objetos[i]);
            //objetos[i] = Instantiate(PrefabBloco[i], posicoes[i], Quaternion.identity);
            LastPosition[i] = posicoes[i];
        }


    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float time = Time.time;


        Vector3[] novasPosicoes = new Vector3[PrefabBloco.Length];
        for (int i = 0; i < PrefabBloco.Length; i++)
        {

            //Eulers method
            aceleracoes[i] = new Vector3(SolveSpring(i) / mass[i], 0, 0);
            //Adiciona a velocidade ao bloco
            velocidades[i] += Time.fixedDeltaTime * aceleracoes[i];
            //Adiciona a posição ao bloco
            novasPosicoes[i] = posicoes[i] + velocidades[i] * Time.fixedDeltaTime;
            novasPosicoes[i] = CheckBounds(novasPosicoes[i]);




            //Verler Integration method
            /*
            velocidades[i] = posicoes[i] - LastPosition[i];
            LastPosition[i] = posicoes[i];
            novasPosicoes[i] = posicoes[i] + velocidades[i] + aceleracoes[i] * Time.fixedDeltaTime*Time.fixedDeltaTime;
            */

            aceleracoes[i] = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < PrefabBloco.Length; i++)
        {

            posicoes[i] = novasPosicoes[i];
            objetos[i].transform.position = new Vector3(posicoes[i].x, posicoes[i].y, 0);

        }

        ExtractPoints(time);

    }

    private void ExtractPoints(float time)
    {
        if (time > cronometro && time < 20f)
        {
            cronometro += 0.5f;
            //Adiciona o tempo atual e a posicao do bloco 0 na array de posições
            blocoL.Add(Time.time);
            blocoL.Add(posicoes[0].x);
            //Adiciona o tempo atual e a posicao do bloco 1 na array de posições
            blocoR.Add(Time.time);
            blocoR.Add(posicoes[1].x);

        }

        if (time > 20f)
        {

            //Cria um arquivo de texto
            System.IO.StreamWriter file1 = new System.IO.StreamWriter("C:/Users/dante/Mass-Spring-Simulator/Assets/PosicoesL.txt");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("C:/Users/dante/Mass-Spring-Simulator/Assets/PosicoesR.txt");
            System.IO.StreamWriter file3 = new System.IO.StreamWriter("C:/Users/dante/Mass-Spring-Simulator/Assets/TempoL.txt");
            System.IO.StreamWriter file4 = new System.IO.StreamWriter("C:/Users/dante/Mass-Spring-Simulator/Assets/TempoR.txt");
            //Escreve no arquivo de texto
            for (int i = 0; i < blocoL.Count; i += 2)
            {
                file3.WriteLine(blocoL[i]);
                file1.WriteLine(blocoL[i + 1]);
            }
            for (int i = 0; i < blocoR.Count; i += 2)
            {
                file4.WriteLine(blocoR[i]);
                file2.WriteLine(blocoR[i + 1]);
            }
            //Fecha o arquivo de texto
            file1.Close();
            file2.Close();
            file3.Close();
            file4.Close();
            Debug.Log("Arquivos criados com sucesso");

        }
    }

    float SolveSpring(int i)
    {

        if (i == 0)
        {
            /*
            Debug.Log("Entrou no primeiro");
            Debug.Log(-K[i] * (Mathf.Abs(anteparos[0].x - posicoes[i].x ) - x_0[i]) +K[i+1] * (Mathf.Abs(posicoes[i+1].x - posicoes[i].x) - x_0[i+1]));
            */
            return -K[i] * (Mathf.Abs(anteparos[0].x - posicoes[i].x) - x_0[i]) + K[i + 1] * (Mathf.Abs(posicoes[i + 1].x - posicoes[i].x) - x_0[i + 1]);
        }
        else if (i == PrefabBloco.Length - 1)
        {
            /*
            Debug.Log("Entrou no ultimo");
            Debug.Log(-K[i] * (Mathf.Abs(posicoes[i - 1].x - posicoes[i].x) - x_0[i - 1]) + K[i + 1] * (Mathf.Abs(posicoes[i].x - anteparos[1].x) - x_0[i]));
            */
            return -K[i] * (Mathf.Abs(posicoes[i - 1].x - posicoes[i].x) - x_0[i - 1]) + K[i + 1] * (Mathf.Abs(posicoes[i].x - anteparos[1].x) - x_0[i]);

        }


        // Add a return statement here to handle the case when none of the conditions in the loop are met
        return 0;
    }
    private Vector3 CheckBounds(Vector3 pos)
    {

        //Pega o tamno do bloco no prefab
        float d = 0.5f;
        if( pos.x < anteparos[0].x+d )
        {

            Debug.Log("Bateu no anteparo 0");
            return  new Vector3(anteparos[0].x+ d, pos.y, pos.z)   ;
        }
        else if (pos.x > anteparos[1].x-d)
        {
            Debug.Log("Bateu no anteparo 1");
            return new Vector3(anteparos[1].x - d,pos.y, pos.z);
        }
        
        return pos;

    }

    public void Restart()
    {
        //Restarta a cena 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
