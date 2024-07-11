using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Simulator : MonoBehaviour
{

    //Vectors Arrays
    [SerializeField]
    private Vector3[] velocity, aceleration, lastPosition;
    public Vector3[] positions, partitionWall;
    //Game Objects Arrays
    public GameObject[] prefabBox, objects;

    //Float Arrays
    public float[] mass, K, x_0;
    // Float
    public float collisionDumping;
    private float cronometro = 5f;
    //Float Lists
    private List<float> boxL, boxR;



    void Start()
    {
        //Inicializate the lists
        boxL = new List<float>();
        boxR = new List<float>();
        //Inicialize the array of the objects and the lastPosition
        objects = new GameObject[prefabBox.Length];
        lastPosition = new Vector3[prefabBox.Length];

        for (int i = 0; i < prefabBox.Length; i++)
        {
            //Associate each object with it prefab and the incial position
            objects[i] = prefabBox[i];
            lastPosition[i] = positions[i];
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //Take the time passed since the game started
        float time = Time.time;

        //Reset the new positions array
        Vector3[] newPositions = new Vector3[prefabBox.Length];

        for (int i = 0; i < prefabBox.Length; i++)
        {
            //Iterate the array to set movement for each block
            //Eulers method
            aceleration[i] = new Vector3(SolveSpring(i) / mass[i], 0, 0);

            //Adds velocity to the box
            velocity[i] += Time.fixedDeltaTime * aceleration[i];
            //Adds the positions to the box
            newPositions[i] = positions[i] + velocity[i] * Time.fixedDeltaTime;

            //Reset the aceleration 
            aceleration[i] = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < prefabBox.Length; i++)
        {
            //Aply the new position foreach box
            positions[i] = newPositions[i];
            //Check the collision
            CheckColision(i);
            objects[i].transform.position = new Vector3(positions[i].x, positions[i].y, 0);
        }

        //ExtractPoints(time);

    }

    private void ExtractPoints(float time)
    {
        if (time > cronometro && time < 20f)
        {
            cronometro += 0.5f;
            //Adiciona o tempo atual e a posicao do bloco 0 na array de posições
            boxL.Add(Time.time);
            boxL.Add(positions[0].x);
            //Adiciona o tempo atual e a posicao do bloco 1 na array de posições
            boxR.Add(Time.time);
            boxR.Add(positions[1].x);

        }

        if (time > 20f)
        {

            //Cria um arquivo de texto
            System.IO.StreamWriter file1 = new System.IO.StreamWriter("C:/Users/dante/Mass-Spring-Simulator/Assets/PosicoesL.txt");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("C:/Users/dante/Mass-Spring-Simulator/Assets/PosicoesR.txt");
            System.IO.StreamWriter file3 = new System.IO.StreamWriter("C:/Users/dante/Mass-Spring-Simulator/Assets/TempoL.txt");
            System.IO.StreamWriter file4 = new System.IO.StreamWriter("C:/Users/dante/Mass-Spring-Simulator/Assets/TempoR.txt");
            //Escreve no arquivo de texto
            for (int i = 0; i < boxL.Count; i += 2)
            {
                file3.WriteLine(boxL[i]);
                file1.WriteLine(boxL[i + 1]);
            }
            for (int i = 0; i < boxR.Count; i += 2)
            {
                file4.WriteLine(boxR[i]);
                file2.WriteLine(boxR[i + 1]);
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
        //Solve the force aplied in each block due to spring
        if (i == 0)
        {
            return -K[i] * (Mathf.Abs(partitionWall[0].x - positions[i].x) - x_0[i]) + K[i + 1] * (Mathf.Abs(positions[i + 1].x - positions[i].x) - x_0[i + 1]);
        }
        else if (i == prefabBox.Length - 1)
        {
            return -K[i] * (Mathf.Abs(positions[i - 1].x - positions[i].x) - x_0[i - 1]) + K[i + 1] * (Mathf.Abs(positions[i].x - partitionWall[1].x) - x_0[i]);
        }

        // Add a return statement here to handle the case when none of the conditions are met
        return 0;
    }
    private Vector3 CheckBounds(Vector3 pos)
    {
        //Take the lengh of the box in Unity 
        float d = 0.5f;
        //Check if each box are not trespassing the partition wall 
        if (pos.x < partitionWall[0].x + d)
        {
            return new Vector3(partitionWall[0].x + d, pos.y, pos.z);
        }
        else if (pos.x > partitionWall[1].x - d)
        {
            return new Vector3(partitionWall[1].x - d, pos.y, pos.z);
        }
        //return the position
        return pos;
    }
    private void CheckColision(int box)
    {
        //Take the lengh of the box in Unity 
        float d = 0.5f;
        //Check the collision between the box
        if (positions[0].x > positions[1].x - d)
        {
            Vector3 posL = positions[0];
            Vector3 posR = positions[1];

            positions[0].x = posR.x - d;
            positions[1].x = posL.x + d;

            Vector3 velL = velocity[0] * collisionDumping;
            Vector3 velR = velocity[1] * collisionDumping;

            Vector3 acelL = aceleration[0] * collisionDumping;
            Vector3 acelR = aceleration[1] * collisionDumping;

            velocity[0].x = velR.x;
            velocity[1].x = velL.x;

            aceleration[0].x = acelR.x;
            aceleration[1].x = acelL.x;

        }
        
        //Check the collisio between the box and the partition wall  
        if (positions[0].x < partitionWall[0].x + d)
        {

            positions[0].x = partitionWall[0].x + d;
            velocity[0].x = -velocity[0].x * collisionDumping;

        }
        if (positions[1].x > partitionWall[1].x - d)
        {
            positions[1].x = partitionWall[1].x - d;
            velocity[1].x = -velocity[1].x * collisionDumping;
        }


    }

    public void Restart()
    {
        //Reset the simulation
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
